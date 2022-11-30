namespace FBDropshipper.Infrastructure.Option
{
    public class StripeOption
    {
        public string PublishableKey { get; set; }
        public string SecretKey { get; set; }
        public string WebHookKey { get; set; }
    }

    public class RainforestOption
    {
        public string ApiKey { get; set; }
    }
}