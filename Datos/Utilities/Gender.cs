using System.Runtime.Serialization;

namespace Datos.Utilities;

public class Gender
{
    public enum GenderName
    {
        [EnumMember(Value = "hombre")]
        hombre,
        [EnumMember(Value = "mujer")]
        mujer
    }

    public static List<GenderName> Genders = Enum.GetValues(typeof(GenderName))
                                                 .Cast<GenderName>()
                                                 .ToList();
}