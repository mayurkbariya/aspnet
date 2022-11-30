using System;
using System.Collections.Generic;

namespace AsiaCab.EnityModel.TempEntity
{
    public partial class Role
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool? ActiveFlag { get; set; }
        public string UserType { get; set; }
    }
}
