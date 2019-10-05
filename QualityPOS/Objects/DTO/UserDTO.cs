using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityPOS.Objects.DTO
{
    public class UserDTO : User
    {
        public UserRole UserRole { get; set; }
        public string UserRoleStr { get; set; }
        public string ReTypePassword { get; set; }
    }
}
