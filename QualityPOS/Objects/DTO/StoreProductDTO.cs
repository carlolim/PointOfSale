using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityPOS.Objects.DTO
{
    public class StoreProductDTO
    {
        public int StoreProductID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal SalesPrice { get; set; }
        public string SalesPriceStr { get; set; }
        public double Quantity { get; set; }
        public string QuantityStr { get; set; }
        public double UnitsSold { get; set; }
        public string UnitsSoldStr { get; set; }
        public double UnitsLeft { get; set; }
        public string UnitsLeftStr { get; set; }
        public decimal TotalPriceSold { get; set; }
        public decimal TotalPriceLeft { get; set; }
        public int ProductID { get; set; }
        public int StoreID { get; set; }
    }
}
