using Last02.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Models.RequestDtos
{
    public class UpdateFlashcardStatusRequest
    {
        public int FlashcardId { get; set; }
        public FlashcardAction Action { get; set; }
    }
}
