using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ScriptLibrary
{
    [Serializable]
    public class Step
    {
        public int StepNumber { set; get; } = -1;
        public List<LogicBase> Content { set; get; } = new List<LogicBase>();
        public StepConfigure Configure { set; get; } = new StepConfigure
        {
            StepName = "",
            StepIndex = -1,
            DefaultActionDelay = 1000,
            StopIfFail = true,
            IsInclude = true,
            IsPreStep = false,
            IsFinalStep = false
        };

        public Action<StepCallbackData> StepCompletedCallback { set; get; } = null;
        public Action<PositionCallbackData> PositionCallback { set; get; } = null;
        public Action<ActionCallbackData> ActionCompletedCallback { set; get; } = null;
        public Func<bool, string, FailDialogResult> FailDialogCallback { set; get; } = null;

        public string Run(ref CancellationTokenSource cancel)
        {
            LogicResultList results_in_step = new LogicResultList();
            FailDialogResult dialog_result = null;
            bool step_pass = true;
            string step_result = "Pass";

            try
            {
                foreach (LogicBase logic in Content)
                {
                    if (logic.IsInclude)
                    {
                        //Run logics in step
                        int start_index = results_in_step.ResultList.Count;
                        logic.PositionCallBack = PositionCallback;
                        logic.ActionCompletedCallBack = ActionCompletedCallback;
                        try
                        {
                            results_in_step = logic.Do(results_in_step, ref cancel);
                        }
                        catch (Exception error)
                        {
                            Exception base_error = error.GetBaseException();
                            if (!(base_error is OperationCanceledException | base_error is TaskCanceledException)) throw;
                        }

                        //Check fail result in first level of logic
                        LogicResult fail_result = results_in_step.GetActionFail(start_index);
                        if (fail_result != null)
                        {
                            string detail = fail_result.Details.ContainsKey("Detail") ? fail_result.Details["Detail"] : "";
                            string fail_message = $"Fail Step: {StepNumber} {Configure.StepName}" + Environment.NewLine + detail;

                            //Fail Dialog
                            dialog_result = FailDialogCallback?.Invoke(Configure.StopIfFail, fail_message);
                            if (dialog_result == null || dialog_result.ReallyFail)
                            {
                                step_pass = false;
                                step_result = $"Fail: {detail}";
                            }
                            if (dialog_result != null && dialog_result.StopTest)
                            {
                                break;
                            }
                        }
                    }
                }

                StepCallbackData callback = new StepCallbackData
                {
                    StepNumber = StepNumber,
                    StepIndex = Configure.StepIndex,
                    IsPass = step_pass,
                    StepName = Configure.StepName,
                    StepResult = step_result
                };
                StepCompletedCallback?.Invoke(callback);

                if (dialog_result != null && dialog_result.StopTest) throw new OperationCanceledException();
            }
            catch (Exception) { throw; }
            return step_result;
        }
    }

    [Serializable]
    public class StepConfigure
    {
        public string StepName = "";
        public int StepIndex;
        public int DefaultActionDelay;
        public bool StopIfFail;
        public bool IsInclude;
        public bool IsPreStep;
        public bool IsFinalStep;
    }

    [Serializable]
    public class StepCallbackData
    {
        public int StepNumber;
        public int StepIndex;
        public bool IsPass;
        public string StepName = "";
        public string StepResult = "";
    }

    [Serializable]
    public class FailDialogResult
    {
        public bool StopTest;
        public bool ReallyFail;
    }
}