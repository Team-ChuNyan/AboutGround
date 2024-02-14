using System.Collections.Generic;

public static class StringManager
{
    private static Dictionary<StringType, string> _string = new()
    {
        { StringType.Example, "굿이다"}
    };

    public static string GetString(StringType type)
    {
        return _string[type];
    }
}
