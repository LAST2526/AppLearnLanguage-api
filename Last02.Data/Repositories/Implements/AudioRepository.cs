using Last02.Data.Entities;
using Last02.Data.Repositories.RepositoryBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Data.Repositories.Implements
{
    public class AudioRepository : Repository<Audio>, IAudioRepository
    {
        private readonly ApplicationDbContext _context;
        public AudioRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> CountAsync(Expression<Func<Audio, bool>> predicate)
        {
            return await _context.Audios.CountAsync(predicate);
        }
    }
}
