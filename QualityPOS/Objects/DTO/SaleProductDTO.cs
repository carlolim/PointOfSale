using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityPOS.Objects.DTO
{
    public class SaleProductDTO: SaleProduct
    {
        public string DateString { get; set; }
        public string TimeString { get; set; }
        public string Items { get; set; }
        public string UserFullname { get; set; }
        public string ProductName { get; set; }
        public decimal SalesPrice { get; set; }
    }
}
