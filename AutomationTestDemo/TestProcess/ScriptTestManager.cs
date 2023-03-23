using ScriptLibrary;
using ScriptLibrary.Executor;

namespace AutomationTestDemo.TestProcess;

public class ScriptTestManager : TestManager
{
    private List<bool> _allTestStepResult = new List<bool>();
    
    //ScriptTestManager function. If there are other test modes, it will have own RunTest function.
    public void RunTest(Script script)
    {
        _passNumber = 0;
        _failNumber = 0;
        _allTestStepResult.Clear();
        try
        {
            //Initialize All Actions
            Dictionary<string, ExecutorBase.ClassInfo> parameters = _getAllClassInfo();
            _executor = new ScriptExecutor(script);
            _executor.ActivateAllClasses(script.AllClassNameInScript, parameters);

            //Initialize Script
            ((ScriptExecutor)_executor).InitializeScript();

            //Run Test
            _executor.ScriptCompletedCallBack = _onScriptCompleted;
            _executor.TestCompletedCallBack = _onTestCompleted;
            _executor.StepCompletedCallBack = _onStepCompleted;
            _executor.PositionCallBack = _onPosition;
            _executor.ActionCompletedCallBack = _onActionCompleted;
            _executor.FailDialogCallBack = _onFailDialog;
            _executor.AlarmErrorHandle = _onErrorHandle;
            ((ScriptExecutor)_executor).RunScript();
        }
        catch (Exception error)
        {
            Exception base_error = error.GetBaseException();
            Task.Run(() => _onErrorHandle(base_error));
        }
    }

    //Override test completed and calcualte pass/fail number in script test mode
    protected override void _onTestCompleted(int currentTestNumbers)
    {
        try
        {
            bool any_fail = _allTestStepResult.Any(x => x == false);
            if (any_fail)
            {
                _failNumber = _failNumber + 1;
            }
            else
            {
                _passNumber = _passNumber + 1;
            }
            base._onTestCompleted(currentTestNumbers);
            _allTestStepResult.Clear();
        }
        catch { throw; }
    }

    //Override step complated and calcualte pass/fail number in script test mode
    protected override void _onStepCompleted(StepCallbackData data)
    {
        try
        {
            _allTestStepResult.Add(data.IsPass);
            base._onStepCompleted(data);
        }
        catch { throw; }
    }
}
