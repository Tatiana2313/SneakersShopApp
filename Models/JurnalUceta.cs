using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersShopApp.Models
{
    public class JurnalUceta
    {
        public string MonthName { get; set; }
        public Sneaker Sneakers { get; set; } 
        public int NumberOfDeliveredSneakers { get; set; }
        public int NumberOfSneakersSold { get; set; }
    }
}
