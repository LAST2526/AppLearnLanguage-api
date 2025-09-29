using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Commons;
using Last02.Data.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Last02.Data.Entities
{
    [Table("Users")]
    public class Users : IdentityUser<int>
    {
        [MaxLength(50)]
        public string? Email { get; set; }
        public string? Password { get; set; }
        [Required]
        public Provider Provider { get; set; }
        [MaxLength(8000)]
        public string? SocialProviderToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string? TemporaryPasswordHash { get; set; }
        public DateTime? TemporaryPasswordExpires { get; set; }
        public DateTime? LastForgotPasswordRequestAt { get; set; }
        public UserType UserType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public virtual Member? Member { get; set; }
        public virtual ICollection<IdentityUserRole<int>> UserRoles { get; set; }

    }
}
