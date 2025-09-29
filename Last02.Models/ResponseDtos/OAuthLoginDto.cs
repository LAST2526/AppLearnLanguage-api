using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models.ResponseDtos
{
    public class OAuthLoginDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public bool IsExist { get; set; }
    }
}
