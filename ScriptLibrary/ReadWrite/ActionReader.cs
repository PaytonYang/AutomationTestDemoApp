using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Drawing;

namespace ScriptLibrary.ReadWrite
{
    public class ActionReader
    {
        //Read all class from test library
        public static List<Tuple<string, Type>> ReadActions(string dllName, string namespaceName)
        {
            try
            {
                string file_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{dllName}.dll");
                if (File.Exists(file_path))
                {
                    Assembly dll = Assembly.LoadFile(file_path);
                    Type[] class__types = dll.GetTypes();
                    if (class__types != null && class__types.Length > 0)
                    {
                        return class__types.Where(x => x.Namespace == namespaceName).Select(x => Tuple.Create(x.Name, x)).ToList();
                    }
                    else { throw new Exception($"No actions in DLL: {dllName}, Namespace: {namespaceName}"); }
                }
                else { throw new Exception($"DLL: {dllName} is not found in {file_path}"); }
            }
            catch { throw; }
        }
        //Read all methods of class from test library
        public static List<Tuple<string, MethodInfo>> ReadActionMethods(Type classType)
        {
            try
            {
                MethodInfo[] methods = classType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                if (methods != null && methods.Length > 0)
                {
                    return methods.Select(x => Tuple.Create(x.Name, x)).ToList(); 
                }
                else { throw new Exception($"No methods in action: {classType.Name}"); }
            }
            catch { throw; }
        }
        //Read all parameters of method from test library
        public static List<Tuple<string, string>> ReadMethodParameters(MethodInfo methodInfo)
        {
            try
            {
                if (methodInfo.GetParameters() != null && methodInfo.GetParameters().Length > 0)
                {
                    return methodInfo.GetParameters().Select(x => Tuple.Create(x.Name, x.ParameterType.Name)).ToList();
                }
                else { return new List<Tuple<string, string>>(); }
            }
            catch { throw; }
        }

        public static object DataFormat(object data, string format)
        {
            try
            {
                object result = null;
                if (data != null)
                {
                    switch (format)
                    {
                        case "String":
                            result = Convert.ToString(data);
                            break;
                        case "Byte":
                            result = Convert.ToByte(data);
                            break;
                        case "Int32":
                            result = Convert.ToInt32(data);
                            break;
                        case "Boolean":
                            result = Convert.ToBoolean(data);
                            break;
                        case "DataTime":
                            result = Convert.ToDateTime(data);
                            break;
                        case "Int64":
                            result = Convert.ToInt64(data);
                            break;
                        case "String Array":
                            result = (Convert.ToString(data)).Split(',');
                            break;
                        case "Double":
                            result = Convert.ToDouble(data);
                            break;
                        case "Int32 Array":
                            result = (Convert.ToString(data)).Split(',').Select(x => Convert.ToInt32(x)).ToArray();
                            break;
                        default:
                            break;
                    }
                }
                return result;
            }
            catch { throw; }
        }
    }
}
