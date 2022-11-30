using System;
using System.Collections.Generic;
using FBDropshipper.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FBDropshipper.Domain.Entities
{
    public class Role : IdentityRole, IBase
    {
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RoleClaim> RoleClaims { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}