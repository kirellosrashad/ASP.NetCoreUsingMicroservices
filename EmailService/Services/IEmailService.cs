using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailService.Services
{
    public interface IEmailService
    {
        Task SendEmail(List<string> to, List<string> cc, List<string> bcc, string subject, string body);
    }
}
