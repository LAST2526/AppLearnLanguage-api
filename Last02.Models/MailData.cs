using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models
{
    public class MailData
    {
        public string ReceiverMailName { get; set; } = default!;
        public string ReceiverMailAddr { get; set; } = default!;
        public string EmailSubject { get; set; } = default!;
        public string EmailBody { get; set; } = default!;
    }
}
