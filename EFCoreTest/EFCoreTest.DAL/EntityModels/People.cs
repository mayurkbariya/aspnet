using System;
using System.Collections.Generic;

namespace EFCoreTest.DAL.EntityModels
{
    public partial class People
    {
        public People()
        {
            CustomersAlternateContactPerson = new HashSet<Customers>();
            CustomersLastEditedByNavigation = new HashSet<Customers>();
            CustomersPrimaryContactPerson = new HashSet<Customers>();
            InverseLastEditedByNavigation = new HashSet<People>();
            OrdersContactPerson = new HashSet<Orders>();
            OrdersLastEditedByNavigation = new HashSet<Orders>();
            OrdersPickedByPerson = new HashSet<Orders>();
            OrdersSalespersonPerson = new HashSet<Orders>();
        }

        public int PersonId { get; set; }
        public string FullName { get; set; }
        public string PreferredName { get; set; }
        public string SearchName { get; set; }
        public bool IsPermittedToLogon { get; set; }
        public string LogonName { get; set; }
        public bool IsExternalLogonProvider { get; set; }
        public byte[] HashedPassword { get; set; }
        public bool IsSystemUser { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsSalesperson { get; set; }
        public string UserPreferences { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public byte[] Photo { get; set; }
        public string CustomFields { get; set; }
        public string OtherLanguages { get; set; }
        public int LastEditedBy { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public People LastEditedByNavigation { get; set; }
        public ICollection<Customers> CustomersAlternateContactPerson { get; set; }
        public ICollection<Customers> CustomersLastEditedByNavigation { get; set; }
        public ICollection<Customers> CustomersPrimaryContactPerson { get; set; }
        public ICollection<People> InverseLastEditedByNavigation { get; set; }
        public ICollection<Orders> OrdersContactPerson { get; set; }
        public ICollection<Orders> OrdersLastEditedByNavigation { get; set; }
        public ICollection<Orders> OrdersPickedByPerson { get; set; }
        public ICollection<Orders> OrdersSalespersonPerson { get; set; }
    }
}
