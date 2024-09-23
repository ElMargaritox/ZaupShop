using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaupShop.Models
{
    public class VehicleShop
    {
        public int Id { get; set; }
        public string VehicleName { get; set; }
        public decimal Cost { get; set; } = 100.00m; 
    }

}
