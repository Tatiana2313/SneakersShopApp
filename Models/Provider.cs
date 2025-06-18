using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersShopApp.Models
{
    public class Provider
    {
        public int CodProdiver { get; set; }
        public string ProviderName { get; set; }
        public string Addres { get; set; }
        public string PhoneFax { get; set; }
        public string RascetScet { get; set; }

        public override string ToString() => ProviderName;
    }
}
