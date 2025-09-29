using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models.RequestDtos
{
    public class RegisterAndLoginRequest : CreateUserRequest
    {
        public bool? DoLogin { get; set; } = true;
    }
}
