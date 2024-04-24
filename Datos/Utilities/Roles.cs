using System.Runtime.Serialization;

namespace Datos.Utilities;
public class Roles
{
    public enum UserRoles
    {
        [EnumMember(Value = "google")]
        google,
        [EnumMember(Value = "dev")]
        dev,
        [EnumMember(Value = "admin")]
        admin,
        [EnumMember(Value = "judge")]
        judge,
        [EnumMember(Value = "officer")]
        officer,
        [EnumMember(Value = "anonymous")]
        anonymous
    }

    public static List<UserRoles> RolesList = Enum.GetValues(typeof(UserRoles))
                                                  .Cast<UserRoles>()
                                                  .ToList();
}