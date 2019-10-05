using QualityPOS.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QualityPOS
{
    public static class Global
    {
        public static User User { get; set; }
        public static Store Store { get; set; }// = new Store() { StoreID = 1 };
        
    }

    public enum UserRoleEnum
    {
        Admin = 1,
        Manager = 2,
        Employee = 3
    }
}
