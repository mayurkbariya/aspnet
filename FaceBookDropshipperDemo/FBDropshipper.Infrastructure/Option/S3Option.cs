namespace FBDropshipper.Infrastructure.Option
{
    public class S3Option
    {
        public string BucketName { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Region { get; set; }
    }
}