using AppUtility.AppModels;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppUtility.AppMessage
{
    public interface IMailTemplateService
    {
        string PasswordResetTemplate(string UserName, string CompLogoPath, string ResetPassLink, string SendBy, string ActiveTime);
        string NewLoginUserTemplate(string UserName, string CompLogoPath, string ResetPassLink, string SendBy, string ActiveTime);
        string UserPassResetTemplate(string UserName, string CompLogoPath, string ResetPassLink, string SendBy, string ActiveTime);
    }

    public class MailTemplateService : IMailTemplateService
    {        
        public MailTemplateService()
        {           
        }

        public string PasswordResetTemplate(string UserName, string CompLogoPath, string ResetPassLink, string SendBy, string ActiveTime)
        {
            string Result = @"<h1><img src='{{CompLogoPath}}' alt='...' />&nbsp;</h1>
                            <h2>Hi&nbsp;&nbsp;{{UserName}}</h2>
                            <h2><span style='background-color: #00ff00;'>Your password reset has been initiated by Admin.</span>&nbsp;</h2>
                            Click the below link to reset your password. This link will be active for {{ActiveTime}}. <br /><br />
                            <a href='{{ResetPassLink}}'>Click here to Reset your Password</a>
                            <p>&nbsp;</p>
                            <p><strong>Send by {{SendBy}}</strong></p>
                            <p>&nbsp;</p>";
            return Result.Replace("{{UserName}}", UserName)
                         .Replace("{{ResetPassLink}}", ResetPassLink)
                         .Replace("{{CompLogoPath}}", CompLogoPath)
                         .Replace("{{ActiveTime}}", ActiveTime)
                         .Replace("{{SendBy}}", SendBy);
        }

        public string NewLoginUserTemplate(string UserName, string CompLogoPath, string ResetPassLink, string SendBy, string ActiveTime)
        {
            string Result = @"<h1><img src='{{CompLogoPath}}' alt='...' />&nbsp;</h1>
                            <h2>Hi&nbsp;&nbsp;{{UserName}}</h2>
                            <h2><span style='background-color: #00ff00;'>Your Login is created by Admin.</span>&nbsp;</h2>
                            Click the below link to reset your password. This link will be active for {{ActiveTime}}. <br /><br />
                            <a href='{{ResetPassLink}}'>Click here to Reset your Password</a>
                            <p>&nbsp;</p>
                            <p><strong>Send by {{SendBy}}</strong></p>
                            <p>&nbsp;</p>";
            return Result.Replace("{{UserName}}", UserName)
                         .Replace("{{ResetPassLink}}", ResetPassLink)
                         .Replace("{{CompLogoPath}}", CompLogoPath)
                         .Replace("{{ActiveTime}}", ActiveTime)
                         .Replace("{{SendBy}}", SendBy);
        }

        public string UserPassResetTemplate(string UserName, string CompLogoPath, string ResetPassLink, string SendBy, string ActiveTime)
        {
            string Result = @"<h1><img src='{{CompLogoPath}}' alt='...' />&nbsp;</h1>
                            <h2>Hi&nbsp;&nbsp;{{UserName}}</h2>
                            <h2><span style='background-color: #00ff00;'>Your Login password is reseted by Admin.</span>&nbsp;</h2>
                            Click the below link to reset your password. This link will be active for {{ActiveTime}}. <br /><br />
                            <a href='{{ResetPassLink}}'>Click here to Reset your Password</a>
                            <p>&nbsp;</p>
                            <p><strong>Send by {{SendBy}}</strong></p>
                            <p>&nbsp;</p>";
            return Result.Replace("{{UserName}}", UserName)
                         .Replace("{{ResetPassLink}}", ResetPassLink)
                         .Replace("{{CompLogoPath}}", CompLogoPath)
                         .Replace("{{ActiveTime}}", ActiveTime)
                         .Replace("{{SendBy}}", SendBy);
        }
    }
}
