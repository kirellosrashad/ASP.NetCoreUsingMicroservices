using EmailService.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailService.Services
{
    public class EmailService : IEmailService
    {
        MailSetting mailSetting;
        //public EmailService(IOptions<MailSetting> _mailSetting)
        //{
        //    mailSetting = _mailSetting.Value;
        //}
        public EmailService(MailSetting _mailSetting)
        {
            mailSetting = _mailSetting;
        }

        public async Task SendEmail(List<string> to, List<string> cc, List<string> bcc, string subject, string body)
        {
            MailMessage email = new MailMessage()
            {
                From = new MailAddress(mailSetting.Sender, mailSetting.DisplayName),
                Subject = subject,
                Body = body
            };
            foreach (var toEmail in to)
            {
                email.To.Add(toEmail);
            }
            foreach (var ccEmail in cc)
            {
                email.CC.Add(ccEmail);
            }
            foreach (var bccEmail in bcc)
            {
                email.Bcc.Add(bccEmail);
            }

            NetworkCredential credentials = new NetworkCredential(mailSetting.Sender, mailSetting.Password);

            SmtpClient smtp = new SmtpClient()
            {
                Host = mailSetting.Host,
                Port = mailSetting.Port,
                Credentials = credentials
            };

            await smtp.SendMailAsync(email);
        }

        
    }
}
