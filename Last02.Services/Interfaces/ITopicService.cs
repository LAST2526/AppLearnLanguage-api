using Last02.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Models.Dtos;

namespace Last02.Services.Interfaces
{
    public interface ITopicService : IBaseService
    {
        Task<ResponseBase<IEnumerable<TopicListItemDto>>> GetByCourseIdAsync(int userId, int courseId, int? pageSize, int? pageNumber);

    }
}
