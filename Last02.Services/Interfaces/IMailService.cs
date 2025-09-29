using Last02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Services.Interfaces
{
    public interface IMailService
    {
        bool SendMail(MailData mailData, bool htmlMsg = false);
        EmailSettings GetEmailSettings();
    }
}
