namespace FBDropshipper.Application.Shared.CsvModel
{
    public class ProductCsv
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public string ModelNo { get; set; }
        public string Size { get; set; }
        public string Image { get; set; }
        public string ExpiryDate { get; set; }
        public string Remarks { get; set; }
    }
    public class CustomerCsv
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public string UnitNo { get; set; }
        public string Remarks { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    public class CompanyCsv
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Postal { get; set; }
        public string UnitNo { get; set; }
        public string Remarks { get; set; }
        public string Country { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string PhoneNumber { get; set; }
        public string ServiceType { get; set; }
        public int NoOfLicenses { get; set; }
        public string DeviceImeiNos { get; set; }
    }
}