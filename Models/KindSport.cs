using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersShopApp.Models
{
    public class KindSport
    {
        public int CodKindSport { get; set; }
        public string NameSport { get; set; }

        public override string ToString() => NameSport;
    }
}
