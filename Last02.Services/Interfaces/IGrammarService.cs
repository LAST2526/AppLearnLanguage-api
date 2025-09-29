using Last02.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Data.Entities;
using Last02.Models.Dtos;

namespace Last02.Services.Interfaces
{
    public interface IGrammarService : IBaseService
    {
        Task<IEnumerable<Audio>> GetAllGrammarsAsync();
        Task<ResponseBase<IEnumerable<GrammarDto>>> GetByCourseIdAsync(int userId, int courseId, int? pageSize, int? pageNumber);
        Task<ResponseBase<GrammarDto>> GetByIdAsync(int id);
        Task<ResponseBase<GrammarDto>> GetByGrammarCodeAsync(string code);
    }
}
