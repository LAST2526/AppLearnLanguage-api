using Last02.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Data.Repositories.RepositoryBase
{
    public interface IAudioRepository : IRepository<Audio>
    {
        Task<int> CountAsync(Expression<Func<Audio, bool>> predicate);
    }
}
