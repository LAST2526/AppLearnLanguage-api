using Last02.Data.Repositories.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Data.Entities;

namespace Last02.Data.Repositories.Implements
{
    public class FlashcardRepository : Repository<Flashcard>, IFlashcardRepository
    {
        private readonly ApplicationDbContext _context;
        public FlashcardRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
