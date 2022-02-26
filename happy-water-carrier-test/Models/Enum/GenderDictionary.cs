using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace happy_water_carrier_test.Models.Enum
{
    public class GenderDictionary
    {
        public static Dictionary<int, string> Dictionary { get; } = new Dictionary<int, string>
        {
            { (int) Gender.male, "Мужчина" },
            { (int) Gender.female, "Женщина" }
        };
    }
}
