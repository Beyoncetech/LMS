using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppUtility.AppModels
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class EmailAttachment
    {
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public string FileType { get; set; }        
    }

    public class EmailMessage
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Boolean IsHtml { get; set; }
        public List<EmailAttachment> Attachments { get; set; }

        public EmailMessage(IEnumerable<string> to, string subject, string content, Boolean isHtml, List<EmailAttachment> attachments)
        {
            To = new List<MailboxAddress>();

            To.AddRange(to.Select(x => new MailboxAddress(x)));
            Subject = subject;
            Content = content;
            IsHtml = isHtml;
            Attachments = attachments;
        }        
    }    
}
