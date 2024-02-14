using System.Collections.Generic;

public static class StringManager
{
    private static Dictionary<TextType, string> _string = new()
    {
        { TextType.Example, "굿이다"}
    };

    public static string GetString(TextType type)
    {
        return _string[type];
    }
}
