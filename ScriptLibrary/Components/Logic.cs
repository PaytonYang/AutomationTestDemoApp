using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ScriptLibrary
{
    [Serializable]
    public class ActionLogic : LogicBase
    {
        public object Class { set; get; } = null;
        public string ClassName { set; get; } = "";
        public string MethodName { set; get; } = "";
        public List<string> ParametersName { set; get; } = null;
        public List<object>Parameters { set; get; } = null;
        public int DelayTime { set; internal get; } = 1000;


        public override LogicResultList Do(LogicResultList previousResult, ref CancellationTokenSource cancel)
        {
            LogicResultList result = previousResult;
            try
            {
                if (Class != null & !string.IsNullOrEmpty(MethodName))
                {
                    string message = "";

                    if (cancel.IsCancellationRequested) cancel.Token.ThrowIfCancellationRequested();

                    PositionCallBack?.Invoke(new PositionCallbackData { ActionIndex = ActionIndex });

                    //Get method in class by name
                    Type class_type = Class.GetType();
                    MethodInfo method = class_type.GetMethod(MethodName);
                    if (method != null)
                    {
                        //Check parameters of method are correct.
                        ParameterInfo[] parameters = method.GetParameters();
                        if (parameters.Select(x => x.Name).SequenceEqual(ParametersName))
                        {
                            //Dynamic invoke method
                            object output = method?.Invoke(Class, Parameters?.ToArray());
                            if (output != null && output is Dictionary<string, string>)
                            {
                                //Check the return value
                                var details = (Dictionary<string, string>)output;
                                if (details.ContainsKey("Result"))
                                {
                                    result.AddResult(details["Result"] == "Pass" ? Status.ActionPass : Status.ActionFail, details);
                                }
                                else
                                {
                                    result.AddResult(Status.ActionPass, details);
                                }
                            }
                            else { result.AddResult(Status.ActionPass, new Dictionary<string, string>()); }
                            //Get message from return value
                            var last_result = result.GetLast();
                            string detail = last_result.Details.ContainsKey("Detail") ? last_result.Details["Detail"] : "";
                            message = $"{last_result.Status}: {detail}";
                        }
                        else { throw new Exception($"Cannot find correct parameters in method [{MethodName}]"); }
                    }
                    else { throw new Exception($"Cannot find the method [{MethodName}] in Class {class_type.Name}"); }

                    Task.Delay(DelayTime, cancel.Token).Wait();

                    ActionCallbackData action_data = new ActionCallbackData
                    {
                        Logic = this,
                        Result = message,
                        LogicResultList = result,
                    };
                    ActionCompletedCallBack?.Invoke(action_data);
                }
                else { throw new Exception($"Cannot run the method [{MethodName}] in Class {ClassName}"); }
            }
            catch { throw; }
            return result;
        }
    }

    [Serializable]
    public class ForLogic : LogicBase
    {
        public int RunningTimes { set; get; } = 0;
        public bool RetryIfFail { set; get; } = false;

        public override LogicResultList Do(LogicResultList previousResult, ref CancellationTokenSource cancel)
        {
            LogicResultList result = previousResult;

            try
            {
                int count = 0;
                int total_times = RunningTimes;
                LogicResultList one_loop_results = new LogicResultList();
                for (int i = 0; i < total_times; i++)
                {
                    //Initialize result list
                    one_loop_results = new LogicResultList();

                    //Position indicator
                    PositionCallBack?.Invoke(new PositionCallbackData { ActionIndex = ActionIndex });

                    //Do actions in loop
                    one_loop_results = base.Do(one_loop_results, ref cancel);

                    count++;
                    if (RetryIfFail)
                    {
                        //Retry loop will conintue retry the test until all results in loop are pass
                        if (one_loop_results.CheckAllActionsPass()) break;
                    }
                    else
                    {
                        //Once fail is occurred in loop, break the loop
                        if (!one_loop_results.CheckAllActionsPass()) break;
                    }
                }

                ActionCompletedCallBack?.Invoke(new ActionCallbackData { Logic = this, Result = $"Running_Times: {count}" });

                //Review last loop results
                if (one_loop_results.CheckAllActionsPass())
                {
                    result.AddResult(Status.ActionPass, new Dictionary<string, string> { { "Result", "Pass" } });
                }
                else
                {
                    LogicResult fail_action_result = one_loop_results.GetActionFail();
                    result.AddResult(Status.ActionFail, fail_action_result.Details);
                }
            }
            catch { throw; }
            return result;
        }
    }

    [Serializable]
    public class IfLogic : LogicBase
    {
        public string Criteria { set; get; } = "";

        public override LogicResultList Do(LogicResultList previousResult, ref CancellationTokenSource cancel)
        {
            LogicResultList result = previousResult;
            try
            {
                PositionCallBack?.Invoke(new PositionCallbackData { ActionIndex = ActionIndex });

                //Check the previous result. If the result is equal to cirteria, do the actions
                Dictionary<string, string> last_details = previousResult.GetLast()?.Details;
                if (last_details != null && last_details.ContainsKey("Result"))
                {
                    if (last_details["Result"] == Criteria)
                    {
                        result = base.Do(result, ref cancel);
                    }
                }
            }
            catch (Exception) { throw; }
            return result;
        }
    }

    [Serializable]
    public class LogicBase
    {
        public List<LogicBase> InternalLogic { set; get; } = new List<LogicBase>();
        public int ActionIndex { set; get; }
        public bool IsInclude { set; get; } = true;
        public bool NeedRecord { set; get; } = false;
        public Action<PositionCallbackData> PositionCallBack { set; get; } = null;
        public Action<ActionCallbackData> ActionCompletedCallBack { set; get; } = null;

        public virtual LogicResultList Do(LogicResultList previousResult, ref CancellationTokenSource cancel)
        {
            LogicResultList result = previousResult;
            try
            {
                //Do internal actions
                foreach (LogicBase logic in InternalLogic)
                {
                    if (logic.IsInclude)
                    {
                        logic.PositionCallBack = PositionCallBack;
                        logic.ActionCompletedCallBack = ActionCompletedCallBack;
                        result = logic.Do(result, ref cancel);
                    }
                }
            }
            catch { throw; }
            return result;
        }
    }

    [Serializable]
    public class LogicResultList
    {
        public List<LogicResult> ResultList = new List<LogicResult>();

        public void AddResult(Status status, Dictionary<string, string> details) => ResultList.Add(new LogicResult { Status = status, Details = details });

        public LogicResult GetLast() => ResultList.Last();

        public LogicResult GetActionFail() => ResultList.Where(x => x.Status == Status.ActionFail).FirstOrDefault();

        public bool CheckAllActionsPass() => ResultList.Count == 0 || !ResultList.Where(x => x.Status == Status.ActionFail).Any();

        public LogicResult GetActionFail(int startIndex)
        {
            LogicResult result = null;
            if (ResultList.Count != 0 && ResultList.Count > startIndex)
            {
                for (int i = startIndex; i < ResultList.Count; i++)
                {
                    LogicResult logic_result = ResultList[i];
                    if (logic_result.Status == Status.ActionFail)
                    {
                        result = logic_result;
                        break;
                    }
                }
            }
            return result;
        }
    }

    [Serializable]
    public class LogicResult
    {
        public Status Status { set; get; }
        public Dictionary<string, string> Details { set; get; } = new Dictionary<string, string>();
    }

    public enum Status
    {
        ActionPass = 0,
        ActionFail,
        Error
    }

    [Serializable]
    public class PositionCallbackData
    {
        public int ActionIndex;
    }


    [Serializable]
    public class ActionCallbackData
    {
        public LogicBase Logic;
        public string Result = "";
        public LogicResultList LogicResultList;
    }
}