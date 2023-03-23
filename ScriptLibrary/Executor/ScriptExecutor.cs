using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScriptLibrary.Executor
{
    public class ScriptExecutor : ExecutorBase
    {
        private Script _script = null;

        public ScriptExecutor(Script script) : base()
        {
            _script = script;
        }
        //Initialize script: find action logic in script and assign class instance to action logic class
        public void InitializeScript()
        {
            if (_script != null)
            {
                try
                {
                    foreach (Step step in _script.Content)
                    {
                        _findActionLogic(step.Content, step.Configure.DefaultActionDelay);
                    }
                }
                catch { throw; }
            }
        }
        //Run script
        public void RunScript()
        {
            try
            {
                if (_cancelScript.IsCancellationRequested) _cancelScript = new CancellationTokenSource();
                _pauseScript = new EventWaitHandle(true, EventResetMode.ManualReset);
                Task.Run(() => _runScriptProcess(ref _cancelScript)).ContinueWith(t => _onScriptCompleted(t));
            }
            catch { throw; }
        }
        //Override the base class, and re-initialize the CancelToken property in class
        public override void ResumeForceStopScript()
        {
            try
            {
                base.ResumeForceStopScript();
                _reInitializeScript(_script);
            }
            catch { throw; }
        }

        private void _runScriptProcess(ref CancellationTokenSource cancel)
        {
            if (_script == null || _script.Content == null) return;
            //Find pre-steps, loop-steps and final steps
            List<Step> pre_steps = _script.Content.Where(x => x.Configure.IsPreStep).ToList();
            List<Step> loop_steps = _script.Content.Where(x => (x.Configure.IsPreStep == false & x.Configure.IsFinalStep == false)).ToList();
            List<Step> final_steps = _script.Content.Where(x => x.Configure.IsFinalStep).ToList();
            int test_times = 0;
            try
            {
                //Run the pre-steps before the test is started
                int step_number = 1;
                foreach (Step step in pre_steps)
                {
                    _runStep(step, ref cancel, step_number);
                    step_number++;
                }
                //Start test in loops
                if (loop_steps.Count > 0)
                {
                    int initial_step = step_number;
                    while (test_times < _script.LoopTimes)
                    {
                        foreach (Step step in loop_steps)
                        {
                            _runStep(step, ref cancel, step_number);
                            step_number++;
                        }
                        step_number = initial_step;
                        test_times++;
                        TestCompletedCallBack?.Invoke(test_times);
                    }
                }
                //Run the final-steps after the test is finished
                step_number = step_number + loop_steps.Count;
                foreach (Step step in final_steps)
                {
                    _runStep(step, ref cancel, step_number);
                    step_number++;
                }
            }
            catch (Exception error)
            {
                Exception base_error = error.GetBaseException();
                //Ignore the operation cancel
                if (!(base_error is OperationCanceledException | base_error is TaskCanceledException))
                {
                    AlarmErrorHandle?.Invoke(base_error);
                }
                test_times++;
                TestCompletedCallBack?.Invoke(test_times);
            }
        }

        private void _runStep(Step step, ref CancellationTokenSource cancel, int stepNumber)
        {
            if (step.Configure.IsInclude)
            {
                try
                {
                    step.StepNumber = stepNumber;
                    step.FailDialogCallback = FailDialogCallBack;
                    step.StepCompletedCallback = StepCompletedCallBack;
                    step.PositionCallback = PositionCallBack;
                    step.ActionCompletedCallback = ActionCompletedCallBack;
                    string result = step.Run(ref cancel);
                    if (_pauseScript != null) _pauseScript.WaitOne();
                }
                catch (Exception) { throw; }
            }
        }

        private void _findActionLogic(List<LogicBase> stepContent, int defaultDelayTime)
        {
            if (stepContent != null)
            {
                try
                {
                    foreach (LogicBase logic in stepContent)
                    {
                        if (logic.InternalLogic != null)
                        {
                            _findActionLogic(logic.InternalLogic, defaultDelayTime);
                        }
                        else
                        {
                            //Find action logic and assign the class instance to action logic
                            Type logic_type = logic.GetType();
                            if (logic_type == typeof(ActionLogic) && logic.IsInclude)
                            {
                                ActionLogic action = (ActionLogic)logic;
                                action.DelayTime = defaultDelayTime;
                                action.Class = _classes[action.ClassName];
                            }
                        }
                    }
                }
                catch (Exception) { throw; }
            }
        }

        private void _reInitializeScript(Script script)
        {
            try
            {
                foreach (Step step in script.Content)
                {
                    _reInitializeActionLogic(step.Content);
                }
            }
            catch (Exception) { throw; }
        }

        private void _reInitializeActionLogic(List<LogicBase> stepContent)
        {
            if (stepContent != null)
            {
                try
                {
                    foreach (LogicBase logic in stepContent)
                    {
                        if (logic.InternalLogic != null)
                        {
                            _reInitializeActionLogic(logic.InternalLogic);
                        }
                        else
                        {
                            //Reset the CancelToken property
                            Type logic_type = logic.GetType();
                            if (logic_type == typeof(ActionLogic) && logic.IsInclude)
                            {
                                ActionLogic action = (ActionLogic)logic;
                                PropertyInfo prop = action.Class.GetType().GetProperty("CancelToken", BindingFlags.Public | BindingFlags.Instance);
                                if (prop != null && prop.CanWrite)
                                {
                                    prop.SetValue(action.Class, _cancelScript.Token, null);
                                }
                            }
                        }
                    }
                }
                catch (Exception) { throw; }
            }
        }
    }
}
