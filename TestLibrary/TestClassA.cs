namespace TestLibrary;

public class TestClassA
{
    public TestClassA(string ipAddress, int port) { }

    public Dictionary<string, string> PassMethod_TwoParameters(string para1, int para2)
    {
        try
        {
            return new Dictionary<string, string> { { "Result", "Pass" }, { "Detail", $"Executed with parameters: {para1}, {para2}" } };
        }
        catch { throw; }
    }

    public Dictionary<string, string> PassMethod_OneParameters(int highLimit)
    {
        try
        {
            return new Dictionary<string, string> { { "Result", "Pass" }, { "Detail", $"Within limitation: {highLimit}" } };
        }
        catch { throw; }
    }

    public Dictionary<string, string> FailMethod(int highLimit, int lowLimit)
    {
        try
        {
            return new Dictionary<string, string> { { "Result", "Fail" }, { "Detail", $"Out of specification: {lowLimit}-{highLimit}" } };
        }
        catch { throw; }
    }

    public Dictionary<string, string> PassMethod_NoParameters()
    {
        return new Dictionary<string, string> { { "Result", "Pass" } };
    }

    public Dictionary<string, string> ErrorMethod()
    {
        throw new Exception("ErrorMethod is executed");
    }
}