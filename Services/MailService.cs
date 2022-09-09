

using AutoMapper;
using INVENTORY.SERVER.Services.Interfaces;
using INVENTORY.SHARED.Dto;
using MailKit.Net.Smtp;
using MimeKit;

namespace INVENTORY.SERVER.Services
{
    public class MailService : BaseService, IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IMapper mapper,  ILoggerManager loggerManager, IConfiguration configuration )
            : base(mapper, loggerManager)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_configuration.GetValue<string>("MailSettings:Mail"));
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_configuration.GetValue<string>("MailSettings:Host"), _configuration.GetValue<int>("MailSettings:Port"), true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_configuration.GetValue<string>("MailSettings:Mail"), _configuration.GetValue<string>("MailSettings:Password"));
                    await client.SendAsync(email);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"{ex.Message}");
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
