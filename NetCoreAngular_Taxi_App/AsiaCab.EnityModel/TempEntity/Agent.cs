using System;
using System.Collections.Generic;

namespace AsiaCab.EnityModel.TempEntity
{
    public partial class Agent
    {
        public int AgentId { get; set; }
        public int? Cvcode { get; set; }
        public int? EmployeePrefixId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? AgentTypeId { get; set; }
        public string HomePhoneNo { get; set; }
        public string MobilePhoneNo { get; set; }
        public int? TaxId { get; set; }
        public string Email { get; set; }
        public string FaxNo { get; set; }
        public string FirstNameOwner { get; set; }
        public string LastNameOwner { get; set; }
        public string PhoneNoOwner { get; set; }
        public string FirstNameContact { get; set; }
        public string LastNameContact { get; set; }
        public string PhoneNoContact { get; set; }
        public string Sdname { get; set; }
        public string Smname { get; set; }
        public string Dmname { get; set; }
        public string Gmname { get; set; }
        public string Avpname { get; set; }
        public string HouseNo { get; set; }
        public string Village { get; set; }
        public string VillageNo { get; set; }
        public string Alley { get; set; }
        public string Road { get; set; }
        public string SubDistrict { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public int? PostalId { get; set; }
        public string Region { get; set; }
        public string HouseNoInvoice { get; set; }
        public string VillageInvoice { get; set; }
        public string VillageNoInvoice { get; set; }
        public string AlleyInvoice { get; set; }
        public string RoadInvoice { get; set; }
        public string SubDistrictInvoice { get; set; }
        public string DistrictInvoice { get; set; }
        public string ProvinceInvoice { get; set; }
        public int? PostalIdinvoice { get; set; }
        public string RegionInvoice { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? AgentStatusId { get; set; }
        public DateTime? ResignmentDate { get; set; }
        public int? DocTypeId { get; set; }
        public int? NoSmallCasePledge { get; set; }
        public int? NoLargeCasePledge { get; set; }
        public int? AmountPledge { get; set; }
        public string RoomSize { get; set; }
        public string CashGuarantee { get; set; }
        public string BankGuarantee { get; set; }
        public int? BankId { get; set; }
        public int? NoDaysPayment { get; set; }
        public string CreditLimit { get; set; }
        public string Details { get; set; }
        public int? SegmentId { get; set; }
        public int? AgentGradeId { get; set; }
        public DateTime? StartEffectiveDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateBy { get; set; }
        public bool? ActiveFlag { get; set; }
        public string ConcessionPlace { get; set; }
        public int? RecommendedOrder { get; set; }
    }
}
