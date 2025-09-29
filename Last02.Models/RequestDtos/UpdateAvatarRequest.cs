using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models.RequestDtos
{
    public class UpdateAvatarRequest
    {
        public IFormFile? AvatarImage { get; set; }
    }
}
