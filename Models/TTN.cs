using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakersShopApp.Models
{
    public class TTN
    {
        public int NumDoc { get; set; }          
        public DateTime DatePost { get; set; }   
        public Provider Provider { get; set; } 
        public Sneaker Sneakers { get; set; }  
        public int NumberOfSneakers { get; set; } 
    }
}
