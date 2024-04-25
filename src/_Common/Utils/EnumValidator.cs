namespace _Common.Utils
    ;

public static class EnumValidator
{
    public static bool IsStringInEnum<T>(string value) where T : Enum
    {
        // Get the names of the enum members
        string[] enumNames = Enum.GetNames(typeof(T));

        // Check if the provided value exists in the enum names
        return Array.Exists(enumNames, name => name.Equals(value, StringComparison.OrdinalIgnoreCase));
    }
}
