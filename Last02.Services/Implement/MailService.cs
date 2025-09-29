using Last02.Data.UnitOfWork;
using Last02.Models;
using Last02.Services.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

namespace Last02.Services.Implement
{
    public class MailService : IMailService
    {
        private readonly IUnitOfWork _uow;
        EmailSettings _emailSettings = null!;
        ILogger<MailService> _logger;

        public MailService(ILogger<MailService> logger, IOptions<EmailSettings> options, IUnitOfWork uow)
        {
            _uow = uow;
            _emailSettings = options.Value;
            _logger = logger;
        }

        public bool SendMail(MailData mailData, bool htmlMsg = false)
        {
            try
            {
                if (mailData == null || string.IsNullOrEmpty(mailData.EmailBody)) return false;
                //MimeMessage - a class from Mimekit
                MimeMessage emailMessage = new MimeMessage();

                // Set sender mail info
                MailboxAddress email_From = new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail);
                emailMessage.From.Add(email_From);

                // Set receiver mail info
                MailboxAddress email_To = new MailboxAddress(mailData.ReceiverMailName, mailData.ReceiverMailAddr);
                emailMessage.To.Add(email_To);

                // Set mail subject
                emailMessage.Subject = mailData.EmailSubject;
                BodyBuilder emailBodyBuilder = new BodyBuilder();

                // Set mail body
                if (htmlMsg)
                {
                    emailBodyBuilder.HtmlBody = mailData.EmailBody;
                }
                else
                {
                    emailBodyBuilder.TextBody = mailData.EmailBody;
                }
                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                //this is the SmtpClient class from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                using (SmtpClient mailClient = new())
                {
                    if (!_emailSettings.SecureSocketOptions.HasValue)
                    {
                        _emailSettings.SecureSocketOptions = 1; //Auto
                    }
                    if (_emailSettings.SecureSocketOptions.HasValue && _emailSettings.SecureSocketOptions.Value == 2)
                    {
                        mailClient.ServerCertificateValidationCallback = (s, h, c, e) => true;
                    }

                    SecureSocketOptions secureSocketOptions = (SecureSocketOptions)Enum.Parse(typeof(SecureSocketOptions), _emailSettings.SecureSocketOptions.Value.ToString());
                    mailClient.Connect(_emailSettings.SmtpServer, _emailSettings.SmtpPort, secureSocketOptions);

                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    mailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                    mailClient.Authenticate(_emailSettings.User, _emailSettings.Password);

                    mailClient.Send(emailMessage);
                    mailClient.Disconnect(true);
                    mailClient.Dispose();

                    return true;
                }
            }
            catch (Exception ex)
            {
                // Exception Details
                _logger.LogError("Send mail error" + ex.Message);
                return false;
            }
        }

        public EmailSettings GetEmailSettings()
        {
            return _emailSettings;
        }
    }
}
