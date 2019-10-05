using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityPOS.Objects.DTO
{
    public class ProductDTO : Product
    {
        public bool AutomaticCode { get; set; }
        public string AvailableStockStr { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string SalesPriceStr { get; set; }
        public bool AutomaticSalesPrice { get; set; }
        public string CostStr { get; set; }
        public string MarkUpStr { get; set; }
        public decimal InStore { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal UnitsLeft { get; set; }
        public decimal AvailableStock { get; set; }
    }
}
