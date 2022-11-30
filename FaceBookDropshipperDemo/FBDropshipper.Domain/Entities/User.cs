using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FBDropshipper.Domain.Constant;
using FBDropshipper.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FBDropshipper.Domain.Entities
{
    [Table("users")]
    public class User: IdentityUser, IBase
    {
        public User()
        {
        }
        public string FullName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual ICollection<UserRole> UserRoles { get; set; }

        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public virtual ICollection<UserClaim> UserClaims { get; set; }
        public bool IsEnabled { get; set; }
        public IEnumerable<AppTransaction> AppTransactions { get; set; }
        public IEnumerable<UserSubscription> UserSubscriptions { get; set; }
        public IEnumerable<Team> Teams { get; set; }
        public TeamMember TeamMember { get; set; }
        public UserCard UserCard { get; set; }
        public IEnumerable<Catalog> Catalogs { get; set; }
        public ICollection<TeamMemberPermission> TeamMemberPermissions { get; set; }
        public IEnumerable<UserNotification> Notifications { get; set; }
    }
}