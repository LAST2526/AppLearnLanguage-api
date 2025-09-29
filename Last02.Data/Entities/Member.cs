using Last02.Commons;
using Last02.Data.Repositories.RepositoryBase;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Data.Entities
{
    public class Member : IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual Users? User { get; set; }
        [Required]
        [MaxLength(255)]
        public string FullName { get; set; } = default!;
        public string? AvatarUrl { get; set; }
        public Gender Gender { get; set; }
        [ForeignKey(nameof(RoleId))]
        public virtual IdentityRole<int> Role { get; set; } = null!;
        public DateTime DOB { get; set; }
        [Required]
        [MaxLength(255)]
        public string Nationality { get; set; } = default!;
        public bool IsActive { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool MemberLastActive { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime? TimesIsLogoutEnd { get; set; }
        public virtual ICollection<MemberCourse> MemberCourses { get; set; } = new List<MemberCourse>();

    }
}
