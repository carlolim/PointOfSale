﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityPOS.Objects
{
    public class UserRole
    {
        public int UserRoleID { get; set; }
        public string RoleName { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? UserCreatedID { get; set; }
        public DateTime? DateModified { get; set; }
        public int? UserModifiedID { get; set; }
        public DateTime? DateDeleted { get; set; }
        public int? UserDeletedID { get; set; }
        public bool IsDeleted { get; set; }
    }
}
