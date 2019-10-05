using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityPOS.Objects.DTO
{
    public class StoreSummaryDTO
    {
        public int StoreId { get; set; }
        public DateTime DateOpen { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public decimal TotalSales { get; set; }
        public decimal NetSales { get; set; }
    }
}
