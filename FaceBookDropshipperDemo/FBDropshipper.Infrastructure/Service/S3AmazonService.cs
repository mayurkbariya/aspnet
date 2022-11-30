using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Application.Shared;
using FBDropshipper.Application.Templates;
using FBDropshipper.Common.Extensions;
using FBDropshipper.Infrastructure.Option;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FBDropshipper.Infrastructure.Service
{
    public class UrlService : IUrlService
    {
        private readonly IOptions<SendGridOption> _options;

        public UrlService(IOptions<SendGridOption> options)
        {
            _options = options;
        }
        public string GetVerificationUrl(string userId, string token)
        {
            return _options.Value.FrontEndHostUrl + $"/register/verification?userId={userId}&token={token}";
        }

        public string GeneratePasswordResetUrl(string userId, string token)
        {
            return _options.Value.FrontEndHostUrl + $"/reset-password?userId={userId}&token={token}";
        }
    }
    public class EmailService : IEmailService
    {
        private readonly IOptions<SendGridOption> _options;
        private readonly ILogger<EmailService> _logger;
        public EmailService(IOptions<SendGridOption> options, ILogger<EmailService> logger)
        {
            _options = options;
            _logger = logger;
        }

  

        public async Task<bool> SendEmailByTemplate(string email, EmailTemplateType type, object data)
        {
            string templateId = "";
            switch (type)
            {
                case EmailTemplateType.PasswordResetTemplate:
                    templateId = _options.Value.PasswordResetTemplate;
                    break;
                case EmailTemplateType.AccountVerificationTemplate:
                    templateId = _options.Value.AccountVerificationTemplate;
                    break;
                case EmailTemplateType.TeamInviteTemplate:
                    templateId = _options.Value.TeamInviteTemplate;
                    break;
            }
            if (string.IsNullOrWhiteSpace(templateId))
            {
                return false;
            }
            var client = new SendGridClient(_options.Value.ClientKey);
            var from = new EmailAddress(_options.Value.FromEmail, _options.Value.FromName);
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, data);
            var response = await client.SendEmailAsync(msg);
            var statusCode = response.StatusCode.ToInt();
            if (statusCode is >= 200 and < 300)
            {
                return true;
            }
            _logger.LogError("SendGrid" + ": " + await response.Body.ReadAsStringAsync());
            return false;
        }

        public Task<bool> SendEmailByTemplateWithAttachment(string email, EmailTemplateType type, string base64, string fileName,
            string fileType, object data)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendEmailToSelf(string subject, string body)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendEmailToSelfWithAttachment(string subject, string body, params AttachmentModel[] files)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendEmail(string email, string subject, string body)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendEmail(string fromEmail, string fromName, string toEmail, string subject, string body)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendEmailImageAttachment(string email, string subject, string body, string filePath, string fileName,
            string fileType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendEmailImageAttachmentBase64(string email, string subject, string body, string base64, string fileName,
            string fileType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendEmailImageAttachment(string fromEmail, string fromName, string toEmail, string subject, string body,
            string filePath, string fileName, string fileType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendEmailImageAttachmentBase64(string fromEmail, string fromName, string toEmail, string subject, string body,
            string filePath, string fileName, string fileType)
        {
            throw new NotImplementedException();
        }
    }
    public class S3AmazonService : IS3AmazonService
    {
        private readonly IOptions<S3Option> _options;
        private readonly ILogger<S3AmazonService> _logger;
        private readonly AmazonS3Client _client;
        public S3AmazonService(IOptions<S3Option> options, ILogger<S3AmazonService> logger)
        {
            _options = options;
            _logger = logger;
            var credentials = new BasicAWSCredentials(_options.Value.AccessKey, _options.Value.SecretKey);
            _client = new AmazonS3Client(credentials, RegionEndpoint.GetBySystemName(_options.Value.Region));
        }

        public async Task<string> SaveImage(string base64)
        {
            var fileName = Guid.NewGuid().ToString("N") + ".jpg";
            var fullBucket = _options.Value.BucketName + "/images";
            var transferUtility = new TransferUtility(_client);
            try
            {
                var uploadRequest = new TransferUtilityUploadRequest()
                {
                    Key = fileName,
                    BucketName = fullBucket,
                    StorageClass = S3StorageClass.Standard,
                    CannedACL = S3CannedACL.PublicRead,
                };
                using (var ms = new MemoryStream(LoadImage(base64)))
                {
                    uploadRequest.InputStream = ms;
                    await transferUtility.UploadAsync(uploadRequest);
                }
                //https://viteqdev.s3.us-east-2.amazonaws.com/images/950b83f5749e469bb0ce292df6902ab0.jpg
                return
                    $"https://{_options.Value.BucketName}.s3.{_options.Value.Region}.amazonaws.com/images/{fileName}";
            }
            catch (Exception exception)
            {
                _logger.LogError("An AmazonS3Exception was thrown: {0}", exception.Message);
            }
            return "";
        }
        byte[] LoadImage(string baseString)
        {
            baseString = baseString.Remove(0, baseString.IndexOf(',') + 1);
            return Convert.FromBase64String(baseString);
        }
        public async Task DeleteImage(string url)
        {
            try
            {
                var fullBucket = _options.Value.BucketName + "/images";
                var fileName = url.Split('/').Last();
                await _client.DeleteObjectAsync(new Amazon.S3.Model.DeleteObjectRequest() { BucketName = fullBucket, Key = fileName});
            }
            catch (Exception e)
            {
                _logger.LogError("An AmazonS3Exception was thrown: {0}", e.Message);
            }
        }
    }
}