using Last02.Commons;
using Last02.Data.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? Mail { get; set; }
        public bool IsExist { get; set; }

        public Provider Provider { get; set; }
        public UserType UserType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public MemberDto? Member { get; set; }
        public List<int> RoleIds { get; set; } = new();
    }
}
