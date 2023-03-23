using ScriptLibrary;
using ScriptLibrary.Executor;
using System.Text.Json;
using ScriptLibrary.ReadWrite;

namespace AutomationTestDemo.TestProcess;

public class TestManager
{
    protected ExecutorBase? _executor = null;
    protected int _passNumber;
    protected int _failNumber;

    public Action<int>? ActionPositionCallBack { get; set; }
    public Action<string>? ErrorHandleCallBack { get; set; }
    public Action? ScriptCompletedCallBack { get; set; }
    public Action<int, string>? StepCompletedCallBack { get; set; }
    public Action<int, int>? TestCompletedCallBack { get; set; }
    public Func<string, FailDialogResult>? FailStopCallBack { get; set; }
    public Action<int, string>? ActionCompletedCallBack { get; set; }
    public bool IsRunning { get { return _executor != null; } }

    public void StopTest()
    {
        try
        {
            _executor?.StopScript();
        }
        catch { throw; }
    }

    public void PauseTest()
    {
        try
        {
            _executor?.PauseScript();
        }
        catch { throw; }
    }

    public void ContinueTest()
    {
        try
        {
            _executor?.ContinueScript();
        }
        catch { throw; }
    }

    //Report one test run completed to UI
    protected virtual void _onTestCompleted(int currentTestNumbers)
    {
        try
        {
            TestCompletedCallBack?.Invoke(_passNumber, _failNumber);
        }
        catch { throw; }
    }
    //Report one step completed to UI
    protected virtual void _onStepCompleted(StepCallbackData data)
    {
        try
        {
            StepCompletedCallBack?.Invoke(data.StepIndex, (data.IsPass) ? "Pass" : "Fail");
        }
        catch { throw; }
    }
    //Report one action completed to UI
    protected virtual void _onActionCompleted(ActionCallbackData data)
    {
        try
        {
            ActionCompletedCallBack?.Invoke(data.Logic.ActionIndex, data.Result);
        }
        catch { throw; }
    }
    //Report all script test completed to UI
    protected void _onScriptCompleted()
    {
        try
        {
            _executor?.StopScript();
            ScriptCompletedCallBack?.Invoke();
            _executor = null;
        }
        catch { throw; }
    }
    //Report current test position to UI
    protected void _onPosition(PositionCallbackData data)
    {
        try
        {
            ActionPositionCallBack?.Invoke(data.ActionIndex);
        }
        catch { throw; }
    }
    //Report fail dialog to UI
    protected FailDialogResult _onFailDialog(bool failStop, string failMessage)
    {
        try
        {
            FailDialogResult dialog_result = new FailDialogResult
            {
                ReallyFail = true,
                StopTest = false
            };
            if (failStop & FailStopCallBack != null)
            {
                string message = $"Fail Time: {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}" + Environment.NewLine + $"{failMessage}, Continue Test?";
                dialog_result = FailStopCallBack!.Invoke(message);
            }
            return dialog_result;
        }
        catch { throw; }
    }
    //Report error dialog to UI
    protected void _onErrorHandle(Exception exception)
    {
        try
        {
            _executor?.StopScript();
            Exception base_error = exception.GetBaseException();
            string error_message = $"Time: {DateTime.Now}" + Environment.NewLine + $"Message: {base_error.Message}" + Environment.NewLine + "Stack Trace:" + Environment.NewLine + $"{base_error.StackTrace}";
            ErrorHandleCallBack?.Invoke(error_message);
            _executor?.Close();
            _executor = null;
        }
        catch { throw; }
    }
    //Get all class information form json file
    protected Dictionary<string, ExecutorBase.ClassInfo> _getAllClassInfo()
    {
        try
        {
            string file_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configure\AllClassInfo.json");
            if (File.Exists(file_path))
            {
                var all_class_info = JsonSerializer.Deserialize<Dictionary<string, ExecutorBase.ClassInfo>>(File.ReadAllText(file_path));
                if (all_class_info != null)
                {
                    foreach (var class_info in all_class_info)
                    {
                        class_info.Value.Parameters.ForEach(p =>
                        {
                            string format = p.Format;
                            p.ParameterValue = ActionReader.DataFormat(p.ParameterValue.ToString(), format);
                        });
                    }
                    return all_class_info;
                }
                else
                {
                    throw new Exception("Cannot read configuation file AllClassInfo.json");
                }
            }
            else { throw new Exception($"Configuration file is not existed in path: [{file_path}]"); }
        }
        catch { throw; }
    }
}
