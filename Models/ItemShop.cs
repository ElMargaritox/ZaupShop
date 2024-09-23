using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaupShop.Models
{
    public class ItemShop
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public decimal Cost { get; set; } 
        public decimal BuyBack { get; set; } 
    }

}
