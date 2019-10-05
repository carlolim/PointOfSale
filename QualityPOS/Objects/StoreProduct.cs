using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityPOS.Objects
{
    public class StoreProduct
    {
        public int StoreProductID { get; set; }
        public int StoreID { get; set; }
        public int ProductID { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitsSold { get; set; }
        public decimal UnitsLeft { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? UserCreatedID { get; set; }
        public DateTime? DateModified { get; set; }
        public int? UserModifiedID { get; set; }
        public DateTime? DateDeleted { get; set; }
        public int? UserDeletedID { get; set; }
        public bool IsDeleted { get; set; }
    }
}
