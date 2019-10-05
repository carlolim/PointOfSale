using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityPOS.Objects.DTO
{
    public class SaleDTO : Sale
    {
        public string DateStr { get; set; }
        public string TimeStr { get; set; }
        public string Items { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductCost { get; set; }
        public decimal Net { get; set; }
    }
}
