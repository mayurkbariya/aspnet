namespace FBDropshipper.Infrastructure.Option;

public class SendGridOption
{
    public string ClientKey { get; set; }
    public string FrontEndHostUrl { get; set; }
    public string FromEmail { get; set; }
    public string FromName { get; set; }
    public string PasswordResetTemplate { get; set; }
    public string AccountVerificationTemplate { get; set; }
    public string TeamInviteTemplate { get; set; }
}