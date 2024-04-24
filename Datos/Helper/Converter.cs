using System.Text;

namespace DevHelper;

/// <summary>
/// This class is intended to help make development faster.
/// </summary>
public static class Converter
{
    /// <summary>
    /// Converts a string to int <br/>
    /// This method will never return a null value. <br/><br/>
    /// Usage example
    /// <code>
    /// var priceString="55";
    /// var priceInt=priceString.ToInt();
    /// </code>
    /// The priceInt  is now equal to 55
    /// </summary>
    /// <param name="str">the string to convert is passed automatically</param>
    /// <param name="outValue">Return value in case of not being able to convert the string to integer</param>
    /// <returns>An integer result of the conversion.</returns>
    public static int ToInt(this string str, int outValue = 0)
    {
        if (int.TryParse(str, out int result))
            return result;
        else
            return outValue;
    }

    /// <summary>
    /// Converts a string to Decimal <br/>
    /// This method will never return a null value. <br/><br/>
    /// Usage example
    /// <code>
    /// var priceString="55";
    /// var priceDec=priceString.ToDecimal();
    /// </code>
    /// The priceDec  is now equal to 55.00m
    /// </summary>
    /// <param name="str">the string to convert is passed automatically</param>
    /// <param name="outValue">Return value in case of not being able to convert the string to Decimal</param>
    /// <returns>An Decimal result of the conversion.</returns>
    public static decimal ToDecimal(this string str, decimal outValue = 0)
    {
        if (decimal.TryParse(str, out decimal result))
            return result;
        else
            return outValue;
    }

    /// <summary>
    /// Converts a string to Float <br/>
    /// This method will never return a null value. <br/><br/>
    /// Usage example
    /// <code>
    /// var priceString="55";
    /// var priceFloat=priceString.ToDecimal();
    /// </code>
    /// The priceFloat  is now equal to 55.00f
    /// </summary>
    /// <param name="str">the string to convert is passed automatically</param>
    /// <param name="outValue">Return value in case of not being able to convert the string to Float</param>
    /// <returns>An Float result of the conversion.</returns>
    public static float ToFloat(this string str, float outValue = 0)
    {
        if (float.TryParse(str, out float result))
            return result;
        else
            return outValue;
    }

    /// <summary>
    /// Extracts all numbers from a string<br/>
    /// If there is no number, it returns a blank string.<br/>
    /// It returns them in the same order in which it finds them.<br/><br/>
    /// Usage example
    /// <code>
    /// var alphanumeric="A-1B2-C3-D4X5";
    /// var result=alphanumeric.GetNumber();
    /// </code>
    /// The result  is now equal to new string 12345
    /// </summary>
    /// <param name="str">the string to convert is passed automatically</param>
    /// <returns>returns a new string with the numbers found in the original string</returns>
    public static string GetNumber(this string cadena)
    {
        if (string.IsNullOrEmpty(cadena))
            return string.Empty;

        StringBuilder numeros = new StringBuilder();

        foreach (char c in cadena)
        {
            if (Char.IsDigit(c))
                numeros.Append(c);
        }

        return numeros.ToString();
    }
}