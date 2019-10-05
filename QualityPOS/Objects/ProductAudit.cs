using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityPOS.Objects
{
    public class ProductAudit
    {
        public int ProductAuditID { get; set; }
        public int ProductID { get; set; }
        public decimal Quantity { get; set; }
        public DateTime DateCreated { get; set; }
        public int? UserCreatedID { get; set; }
    }
}
