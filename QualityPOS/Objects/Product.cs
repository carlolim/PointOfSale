using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityPOS.Objects
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public String Code { get; set; }
        //public decimal AvailableStock { get; set; }
        public int CategoryID { get; set; }
        public int BrandID { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal Cost { get; set; }
        public double MarkUp { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? UserCreatedID { get; set; }
        public DateTime? DateModified { get; set; }
        public int? UserModifiedID { get; set; }
        public DateTime? DateDeleted { get; set; }
        public int? UserDeletedID { get; set; }
        public bool IsDeleted { get; set; }
    }
}
