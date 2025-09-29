using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models
{
    public class EmailSettings
    {
        public string SenderEmail { get; set; } = default!;
        public string SenderName { get; set; } = default!;
        public string User { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string SmtpServer { get; set; } = default!;
        public int SmtpPort { get; set; }
        public int? SecureSocketOptions { get; set; }
        public string AdminEmail { get; set; } = default!;
    }
}
