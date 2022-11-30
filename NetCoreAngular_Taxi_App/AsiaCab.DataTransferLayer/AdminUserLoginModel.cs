using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaCab.DataTransferLayer
{
    public class AdminUserModel
    {
        public string AdminName { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public int? AdminType { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string CountryCode { get; set; }
        public bool ActiveFlag { get; set; }
    }
    public class AdminUserResponseModel
    {
        public Guid Id { get; set; }
        public string AdminName { get; set; }
        public string Password { get; set; }
        public int? AdminType { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string CountryCode { get; set; }
        public bool ActiveFlag { get; set; }
        public string Token { get; set; }
    }

    public class AdminUserLoginResponseModel
    {
        public Guid Id { get; set; }
        public string AdminName { get; set; }
        public int? AdminType { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string CountryCode { get; set; }
        public bool ActiveFlag { get; set; }
        public string Token { get; set; }
    }

    public class AdminUserTokenModel
    {
        public string AdminName { get; set; }
        public string Password { get; set; }
        public int AdminType { get; set; }
        public Guid Id { get; set; }
    }

    public class AdminUserLoginModel
    {
        public string AdminName { get; set; }
        public string Password { get; set; }
    }
}
