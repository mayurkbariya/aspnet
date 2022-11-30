namespace FBDropshipper.Application.Shared
{
    public class DateCountModel
    {
        public string Date { get; set; }
        public int Count { get; set; }
    }

    public class CustomerCountModel
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string Id { get; set; }
    }

    public class CompanyCountModel
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int Id { get; set; }
    }
}