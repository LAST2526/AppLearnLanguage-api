using Last02.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models.RequestDtos
{
    public class GetFlashcardByTopicIdRequest
    {
        public FlashcardStatus? Status { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
