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
    public interface IEmailSender
    {
        void InitDBContext();
        void SendEmail(EmailMessage message);
        Task<int> SendEmailAsync(EmailMessage message);
        void SetEmailConfigFromDB();
        string GetLastError();
    }

    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IAppSettingService _AppSettingService;
        private string ErrMsg;

        public EmailSender(EmailConfiguration emailConfig, IAppSettingService ObjAppSettingService)
        {
            _emailConfig = emailConfig;
            _AppSettingService = ObjAppSettingService;
        }

        public void InitDBContext()
        {
            _AppSettingService.InitDBContext();
        }

        public void SetEmailConfigFromDB()
        {
            var oMailSetup = _AppSettingService.GetMailSettingSync();
            _emailConfig.From = oMailSetup.FromMailID;
            _emailConfig.SmtpServer = oMailSetup.SmtpServer;
            _emailConfig.Port = Convert.ToInt32(oMailSetup.SmtpServerPort);
            _emailConfig.UserName = oMailSetup.MailUserID;
            _emailConfig.Password = oMailSetup.MailPassword;
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
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
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

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

        private async Task<int> SendAsync(MimeMessage mailMessage)
        {
            int Result = 0;
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                    await client.SendAsync(mailMessage);
                    Result = 1;
                }
                catch (Exception ex)
                {
                    ErrMsg = ex.Message;
                    Result = -1;
                    //log an error message or throw an exception, or both.
                    //throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
            return Result;
        }

        public void SendEmail(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }

        public async Task<int> SendEmailAsync(EmailMessage message)
        {
            var mailMessage = CreateEmailMessage(message);

            var Result = await SendAsync(mailMessage);

            return Result;
        }

        public string GetLastError()
        {
            return ErrMsg;
        }
    }
}
