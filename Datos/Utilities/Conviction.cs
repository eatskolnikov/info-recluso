using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Utilities
{
    public class Conviction
    {
        public enum Status
        {
            [EnumMember(Value = "Preventivo")]
            Preventivo,
            [EnumMember(Value = "Condenado")]
            Condenado
        }

        public static List<Status> GetStatus = Enum.GetValues(typeof(Status))
                                                     .Cast<Status>()
                                                     .ToList();
    }
}
