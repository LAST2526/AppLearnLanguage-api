using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models
{
    public class GoogleAuthSettings
    {
        public string ClientIdWeb { get; set; } = default!;
        public string ClientIdAndroid { get; set; } = default!;
        public string ClientIdIos { get; set;} = default!;
        public string ClientSecret { get; set; } = default!;

        public List<string> GetAllClientIds()
        {
            return new List<string> { ClientIdWeb, ClientIdAndroid, ClientIdIos };
        }
    }
}
