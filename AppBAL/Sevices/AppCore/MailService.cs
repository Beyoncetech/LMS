using AppModel;
using AppUtility.AppModels;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBAL.Sevices.AppCore
{
    public interface IEmailService
    {
        void SendEmail(EmailMessage message);
        Task SendEmailAsync(EmailMessage message);
        string GetLastError();
        Task Initialize();
    }

    public class EmailService : IEmailService
    {
        private readonly IAppSettingService _AppSettingService;
        private MailSettingBM _MailConfig;
        private string ErrMsg;

        public EmailService(EmailConfiguration emailConfig, IAppSettingService ObjAppSettingService)
        {
            _AppSettingService = ObjAppSettingService;
        }

        public async Task Initialize()
        {
            if (_MailConfig == null)
                _MailConfig = await _AppSettingService.GetMailSetting().ConfigureAwait(false);
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_MailConfig.FromMailID));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            BodyBuilder bodyBuilder = null;

            if (message.IsHtml)
                bodyBuilder = new BodyBuilder { HtmlBody = message.Content };
            else
                bodyBuilder = new BodyBuilder { TextBody = message.Content };

            if (message.Attachments != null && message.Attachments.Any())
            {
                foreach (var attachment in message.Attachments)
                {
                    bodyBuilder.Attachments.Add(attachment.FileName, attachment.FileData, ContentType.Parse(attachment.FileType));
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            ErrMsg = string.Empty;
            using (var client = new SmtpClient())
            {
                try
                {
                    int ServerPort = Convert.ToInt32(_MailConfig.SmtpServerPort);
                    client.Connect(_MailConfig.SmtpServer, ServerPort, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_MailConfig.MailUserID, _MailConfig.MailPassword);

                    client.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    ErrMsg = ex.Message;
                    //log an error message or throw an exception or both.
                    //throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            ErrMsg = string.Empty;
            using (var client = new SmtpClient())
            {
                try
                {
                    int ServerPort = Convert.ToInt32(_MailConfig.SmtpServerPort);
                    await client.ConnectAsync(_MailConfig.SmtpServer, ServerPort, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_MailConfig.MailUserID, _MailConfig.MailPassword);

                    await client.SendAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    ErrMsg = ex.Message;
                    //log an error message or throw an exception, or both.
                    //throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

        public void SendEmail(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var mailMessage = CreateEmailMessage(message);

            await SendAsync(mailMessage);
        }

        public string GetLastError()
        {
            return ErrMsg;
        }
    }
}
