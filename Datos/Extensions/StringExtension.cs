namespace Datos.Extensions;

public static class StringExtension
{
    public static string TrimSpace(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        return value.Replace(" ", "");
    }

    public static string RemoveQuotes(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        return value.Replace("\"", "");
    }
}