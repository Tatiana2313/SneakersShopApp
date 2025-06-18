using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace SneakersShopApp.Models
{
    public class Sneaker
    {
        public int CodSneakers { get; set; }
        public string SneakersName { get; set; }
        public string VendorCode { get; set; }
        public string Brand { get; set; }
        public string ProducingCountry { get; set; }
        public KindSport KindOfSport { get; set; }
        public string Material { get; set; }
        public string Gender { get; set; }
        public int Size { get; set; }
        public string Color { get; set; }
        public decimal UnitPrice { get; set; }
        public string Photo { get; set; }

        public override string ToString() => SneakersName;


        public string ImagePath
        {
            get
            {
                if (string.IsNullOrEmpty(Photo))
                    return "/Images/sneaker_placeholder.png";

                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fullPath = Path.Combine(baseDirectory, "Images", Photo).Replace("\\", "/");
                return fullPath;
            }
        }
    }
}
