using Last02.Data.Repositories.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Data.Entities;

namespace Last02.Data.Repositories.Implements
{
    public class TopicRepository : Repository<Topic>, ITopicRepository
    {
        private readonly ApplicationDbContext _context;
        public TopicRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
