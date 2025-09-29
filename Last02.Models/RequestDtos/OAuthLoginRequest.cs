using Last02.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models.RequestDtos
{
    public class OAuthLoginRequest
    {
        public Provider Provider { get; set; }
        public required string SocialProviderToken { get; set; }
    }
}
