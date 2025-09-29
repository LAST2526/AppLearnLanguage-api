using Last02.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Data.Entities
{
    [Table("MemberFlashcards")]
    public class MemberFlashcard
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MemberId { get; set; }

        [Required]
        public int FlashcardId { get; set; }

        public FlashcardStatus Status { get; set; } = FlashcardStatus.New;

        public DateTime LastReviewedAt { get; set; } = DateTime.UtcNow;

        public int RememberCount { get; set; } = 0;

        public DateTime? NextReviewAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(MemberId))]
        public virtual Member Member { get; set; } = null!;

        [ForeignKey(nameof(FlashcardId))]
        public virtual Flashcard Flashcard { get; set; } = null!;
    }
}
