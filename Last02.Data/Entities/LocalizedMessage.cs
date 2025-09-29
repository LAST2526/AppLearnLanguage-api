using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Data.Entities
{
    [Table("LocalizedMessages")]
    public class LocalizedMessage
    {
        [Required]
        public string Code { get; set; } = default!;

        [Required]
        public string LanguageCode { get; set; } = default!;

        [Required]
        public string Type { get; set; } = default!; // "error", "success", etc.

        [Required]
        public string Message { get; set; } = default!;

        public string? Module { get; set; }

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
