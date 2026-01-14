using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Last02.Models.Members
{
    public class MemberSearchView
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        [JsonConverter(typeof(Commons.DateTimeConverter))]
        public DateTime? CreatedAt { get; set; }
    }
}
