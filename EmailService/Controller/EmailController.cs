using EmailService.Model;
using EmailService.RabbitMQReceiver;
using EmailService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        MailSetting mailSetting;
        IConfiguration config;
        public EmailController(MailSetting _mailsetting, IConfiguration _config)
        {
            mailSetting = _mailsetting;
            config = _config;
        }

        //[Route("Send")]
        public IActionResult SendEmail()
        {
            try
            {
                mailSetting.Sender = config.GetValue<string>("SMTPConfig:Sender");
                mailSetting.Password = config.GetValue<string>("SMTPConfig:Password");
                mailSetting.Host = config.GetValue<string>("SMTPConfig:Host");
                mailSetting.Port = Convert.ToInt32(config.GetValue<string>("SMTPConfig:Port"));
                mailSetting.DisplayName = config.GetValue<string>("SMTPConfig:DisplayName");

                var factory = new ConnectionFactory { HostName = "localhost" };
                MessageReceiver msgReceiver = new MessageReceiver(factory, mailSetting);

                if (!string.IsNullOrEmpty(msgReceiver.Message))
                    return Ok(msgReceiver.Message);
                else
                    return NotFound("No Queue Found");
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        //public async Task<IActionResult> SendEmail()
        //{
        //    var x = config["SMTPConfig"];
        //    IEmailService e = new Services.EmailService(JsonConvert.DeserializeObject(x) as MailSetting);
        //    List<string> to = new List<string>();
        //    to.Add("kirellos_rashad@hotmail.com");
        //    List<string> cc = new List<string>();
        //    List<string> bcc = new List<string>();
        //    await e.SendEmail(to, cc, bcc, "New item added", "New item added");
        //    return Ok();
        //}

        //[Route("Send")]
        //public async Task<IActionResult> SendEmail()
        //{
        //    try
        //    {
        //        mailSetting.Sender = config.GetValue<string>("SMTPConfig:Sender");
        //        mailSetting.Password = config.GetValue<string>("SMTPConfig:Password");
        //        mailSetting.Host = config.GetValue<string>("SMTPConfig:Host");
        //        mailSetting.Port = Convert.ToInt32(config.GetValue<string>("SMTPConfig:Port"));
        //        mailSetting.DisplayName = config.GetValue<string>("SMTPConfig:DisplayName");
        //        IEmailService e = new Services.EmailService(mailSetting);
        //        List<string> to = new List<string>();
        //        to.Add("kirellos_rashad@hotmail.com");
        //        List<string> cc = new List<string>();
        //        List<string> bcc = new List<string>();
        //        await e.SendEmail(to, cc, bcc, "New item added", "New item added");
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}
