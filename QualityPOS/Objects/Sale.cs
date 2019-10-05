using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityPOS.Objects
{
    public class Sale
    {
        public int SaleID { get; set; }
        public int StoreID { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Total { get; set; }
        public double AmountPaid { get; set; }
        public double Change { get; set; }
        public int UserID { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? UserCreatedID { get; set; }
        public DateTime? DateModified { get; set; }
        public int? UserModifiedID { get; set; }
        public DateTime? DateDeleted { get; set; }
        public int? UserDeletedID { get; set; }
        public bool IsDeleted { get; set; }
    }
}
