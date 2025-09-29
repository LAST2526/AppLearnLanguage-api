using Last02.Commons;
using Last02.Data.Entities;
using Last02.Models.Dtos;
using Last02.Models.RequestDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Services.Interfaces
{
    public interface IFlashcardService : IBaseService
    {
        Task<ResponseBase<IEnumerable<FlashcardDto>>> GetByTopicIdAsync(int userId, int topicId, GetFlashcardByTopicIdRequest request);
        Task<int> CountByTopicIdAsync(int userId, int topicId, GetFlashcardByTopicIdRequest request);
        Task<ResponseBase<MemberFlashcard>> UpdateFlashcardStatusAsync(int userid, int flashcardId, FlashcardAction status); 
    }
}
