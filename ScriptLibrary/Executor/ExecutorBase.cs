using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScriptLibrary.Executor
{
    public class ExecutorBase
    {
        protected Dictionary<string, object> _classes = null;
        protected EventWaitHandle _pauseScript = null;
        protected CancellationTokenSource _cancelScript = new CancellationTokenSource();

        //Callback when error occurred
        public Action<Exception> AlarmErrorHandle { set; internal get; } = null;
        //Callback when script completed
        public Action ScriptCompletedCallBack { set; internal get; } = null;
        //Callback when one test run completed
        public Action<int> TestCompletedCallBack { set; internal get; } = null;
        //Callback when one step completed
        public Action<StepCallbackData> StepCompletedCallBack { set; internal get; } = null;
        //Callback when current test position in script
        public Action<PositionCallbackData> PositionCallBack { set; internal get; } = null;
        //Callback when one action completed
        public Action<ActionCallbackData> ActionCompletedCallBack { set; internal get; } = null;
        //Callback when test failed
        public Func<bool, string, FailDialogResult> FailDialogCallBack { set; internal get; } = null;

        //All class infomation in test library
        public class ClassInfo
        {
            public string DLLName { get; set; }
            public string Namespace { get; set; }
            public string ClassName { get; set; }
            public List<ClassParameters> Parameters { get; set; }
        }
        public class ClassParameters
        {
            public string ParameterName { get; set; }
            public object ParameterValue { get; set; }
            public string Format { get; set; }
        }
        //Close all class when executor is closed
        public void Close()
        {
            if (_classes != null)
            {
                _classes.ToList().ForEach(_class =>
                {
                    _deactivateClass(_class);
                });
                _classes = null;
            }
        }
        //Activate all classes in script
        public Dictionary<string, object> ActivateAllClasses(List<string> classNamesInScript, Dictionary<string, ClassInfo> allClassInfo)
        {
            try
            {
                _classes = new Dictionary<string, object>();
                foreach (string class_name in classNamesInScript)
                {
                    object class_instance = _activateClass(class_name, allClassInfo,_onError);
                    _classes.Add(class_name, class_instance);
                }
                return _classes;
            }
            catch
            {
                Close();
                throw;
            }
        }

        private object _activateClass(string classNameInScript, Dictionary<string, ClassInfo> allClassInfo, Action<Exception> errorHandleCallBack)
        {
            try
            {
                //Check class in script is already decritbe in configuration file
                if (allClassInfo.ContainsKey(classNameInScript))
                {
                    ClassInfo class_info = allClassInfo[classNameInScript];
                    Assembly DLL = Assembly.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("{0}.dll", class_info.DLLName)));
                    Type dynamic_class = DLL.GetType(string.Format("{0}.{1}", class_info.DLLName, class_info.ClassName));
                    List<string> parameter_names = dynamic_class.GetConstructors()[0].GetParameters().Select(x => x.Name).ToList();
                    List<object> parameter_values = null;
                    //Check class parameters
                    foreach (string name in parameter_names)
                    {
                        object value = null;

                        if (name == "errorCallBack")
                        {
                            value = errorHandleCallBack;
                        }
                        else
                        {
                            if (class_info.Parameters.Where(x => x.ParameterName == name).Count() == 0)
                            {
                                throw new Exception($"Parameter name [{name}] is not found in class [{classNameInScript}]");
                            }
                            value = class_info.Parameters.First(x => x.ParameterName == name).ParameterValue;
                        }
                        if (parameter_values == null)
                        {
                            parameter_values = new List<object> { value };
                        }
                        else
                        {
                            parameter_values.Add(value);
                        }
                    }
                    //Activate class with parameters
                    object class_instance = Activator.CreateInstance(dynamic_class, parameter_values?.ToArray());
                    //Check CancelToken property in class
                    PropertyInfo prop = class_instance.GetType().GetProperty("CancelToken", BindingFlags.Public | BindingFlags.Instance);
                    if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(class_instance, _cancelScript.Token, null);
                    }
                    return class_instance;
                }
                else
                {
                    throw new Exception($"Class [{classNameInScript}] is not set in configuration file");
                }                
            }
            catch { throw; }
        }

        private void _deactivateClass(object classObject)
        {
            try
            {
                //If class has close function, invoke the method
                Type class_type = classObject.GetType();
                MethodInfo methods = class_type.GetMethod("Close");
                if (methods != null) methods.Invoke(classObject, null);
            }
            catch { }
        }
        //Force stop script
        public void StopScript()
        {
            if (_classes != null)
            {
                _classes.ToList().ForEach(action =>
                {
                    try
                    {
                        //If class has stop function, invoke the method
                        Type class_type = action.Value.GetType();
                        MethodInfo methods = class_type.GetMethod("Stop");
                        if (methods != null) methods.Invoke(action.Value, null);
                    }
                    catch (Exception) { }
                });
            }
            _cancelScript.Cancel();
            _pauseScript = null;
        }
        //Pause script
        public void PauseScript()
        {
            _pauseScript.Reset();
        }
        //Continue script when script is paused
        public void ContinueScript()
        {
            _pauseScript.Set();
        }
        //Resume force stop status in class. If class has ResetForceStop function, invoke this method
        public virtual void ResumeForceStopScript()
        {
            if (_classes != null)
            {
                _classes.ToList().ForEach(action =>
                {
                    try
                    {
                        Type class_type = action.Value.GetType();
                        MethodInfo methods = class_type.GetMethod("ResetForceStop");
                        if (methods != null) methods.Invoke(action.Value, null);
                    }
                    catch (Exception) { }
                });
            }
            _cancelScript = new CancellationTokenSource();
        }

        protected void _onScriptCompleted(Task task)
        {
            ScriptCompletedCallBack?.Invoke();
        }

        protected void _onError(Exception error)
        {
            AlarmErrorHandle?.Invoke(error);
        }
    }
}
