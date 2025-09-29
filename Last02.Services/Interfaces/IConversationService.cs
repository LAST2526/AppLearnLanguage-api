using Last02.Commons;
using Last02.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Data.Entities;

namespace Last02.Services.Interfaces
{
    public interface IConversationService : IBaseService
    {
        Task<IEnumerable<Audio>> GetAllConversationsAsync();
        Task<ResponseBase<IEnumerable<ConversationDto>>> GetByCourseIdAsync(int userId, int courseId, int? pageSize, int? pageNumber);
        Task<ResponseBase<ConversationDto>> GetByIdAsync(int id);
        Task<ResponseBase<ConversationDto>> GetByConversationCodeAsync(string code);
    }
}
