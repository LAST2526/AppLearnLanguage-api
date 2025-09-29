using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Commons;

namespace Last02.Data.Entities
{
    [Table("UserDeviceTokens")]
    public class UserDeviceToken
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey(nameof(UserId))]
        public virtual Users? User { get; set; }
    }
}
