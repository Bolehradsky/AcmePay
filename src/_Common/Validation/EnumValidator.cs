namespace _Common.Validation ;

public static class EnumValidator
{
    public static bool IsStringInEnum<T>(string value) where T : Enum
    {
        string[] enumNames = Enum.GetNames(typeof(T));
        return Array.Exists(enumNames, name => name.Equals(value, StringComparison.OrdinalIgnoreCase));
    }
}
