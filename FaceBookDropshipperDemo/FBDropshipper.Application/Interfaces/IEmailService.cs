using System.Threading.Tasks;
using FBDropshipper.Application.Shared;
using FBDropshipper.Application.Templates;

namespace FBDropshipper.Application.Interfaces
{
    public interface IUrlService
    {
        string GetVerificationUrl(string userId, string token);
        string GeneratePasswordResetUrl(string userId, string token);
    }
    public interface IEmailService
    {
        Task<bool> SendEmailByTemplate(string email, EmailTemplateType type, object data);
        Task<bool> SendEmailByTemplateWithAttachment(string email, EmailTemplateType type, string base64, string fileName, string fileType, object data);
        Task<bool> SendEmailToSelf(string subject, string body);
        Task<bool> SendEmailToSelfWithAttachment(string subject, string body, params AttachmentModel[] files);
        Task<bool> SendEmail(string email, string subject, string body);
        Task<bool> SendEmail(string fromEmail, string fromName, string toEmail, string subject, string body);
        Task<bool> SendEmailImageAttachment(string email, string subject, string body, string filePath, string fileName, string fileType);
        Task<bool> SendEmailImageAttachmentBase64(string email, string subject, string body, string base64, string fileName, string fileType);
        Task<bool> SendEmailImageAttachment(string fromEmail, string fromName, string toEmail, string subject, string body, string filePath, string fileName, string fileType);
        Task<bool> SendEmailImageAttachmentBase64(string fromEmail, string fromName, string toEmail, string subject, string body, string filePath, string fileName, string fileType);
        
    }
}