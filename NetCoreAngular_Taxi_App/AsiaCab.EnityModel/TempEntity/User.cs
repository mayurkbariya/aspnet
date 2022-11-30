using System;
using System.Collections.Generic;

namespace AsiaCab.EnityModel.TempEntity
{
    public partial class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Compare { get; set; }
        public byte[] SaltHash { get; set; }
        public byte[] SaltAes { get; set; }
        public DateTime? PasswordUpdateDate { get; set; }
        public int? MemberId { get; set; }
        public int? RoleId { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateBy { get; set; }
        public bool ActiveFlag { get; set; }
        public string UserType { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AuthProvder { get; set; }
    }
}
