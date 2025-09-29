using Last02.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models.RequestDtos
{
    public class CreateUserRequest
    {
        public Provider? Provider { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Gender? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public int? CourseId { get; set; }
        public string? AvatarUrl { get; set; } = string.Empty;
    }
}
