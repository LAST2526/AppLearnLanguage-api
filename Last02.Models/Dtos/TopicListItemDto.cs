using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models.Dtos
{
    public class TopicDto
    {
        public int Id { get; set; }
        public string TopicCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsFree { get; set; }
        public string? HexColorCode { get; set; }
        public int? CourseId { get; set; }
    }

    public class TopicListItemDto : TopicDto
    {
        public int CompletedFlashcardCnt { get; set; }
        public int FlashcardCnt { get; set; }
    }
}
