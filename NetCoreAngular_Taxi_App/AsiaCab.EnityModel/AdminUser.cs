using System;
using System.Collections.Generic;

namespace AsiaCab.EnityModel
{
    public partial class AdminUser
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string AdminName { get; set; }
        public string Password { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? PasswordUpdateDate { get; set; }
        public bool ActiveFlag { get; set; }
        public string Compare { get; set; }
        public byte[] SaltHash { get; set; }
        public byte[] SaltAes { get; set; }
        public string CountryCode { get; set; }
        public int? AdminType { get; set; }
    }
}
