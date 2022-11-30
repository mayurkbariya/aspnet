using System;
using System.Collections.Generic;

namespace AsiaCab.EnityModel.TempEntity
{
    public partial class Customer
    {
        public int Id { get; set; }
        public int? CustomerTypeId { get; set; }
        public int? CustomerStatusId { get; set; }
        public int? ResidenceTypeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string ContactName { get; set; }
        public string DateOfBirth { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string HouseNumberHome { get; set; }
        public string AddressHome { get; set; }
        public string VillageHome { get; set; }
        public string VillageNumberHome { get; set; }
        public string AlleyHome { get; set; }
        public string RoadHome { get; set; }
        public string SubDistrictHome { get; set; }
        public string DistrictHome { get; set; }
        public string ProvinceHome { get; set; }
        public string PostalIdHome { get; set; }
        public string HouseNumberShipment { get; set; }
        public string AddressShipment { get; set; }
        public string VillageShipment { get; set; }
        public string VillageNumberShipment { get; set; }
        public string AlleyShipment { get; set; }
        public string RoadShipment { get; set; }
        public string SubDistrictShipment { get; set; }
        public string DistrictShipment { get; set; }
        public string ProvinceShipment { get; set; }
        public string PostalIdShipment { get; set; }
        public int? UserId { get; set; }
        public string Spname { get; set; }
        public string RouteName { get; set; }
        public int? PaymentTypeId { get; set; }
        public int? BillingTypeId { get; set; }
        public int? BillingDayId { get; set; }
        public int? BillingDateId { get; set; }
        public int? CreditTerm { get; set; }
        public int? MaxAmountOfCredit { get; set; }
        public string Cvcode { get; set; }
        public bool? ActiveFlag { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateBy { get; set; }
    }
}
