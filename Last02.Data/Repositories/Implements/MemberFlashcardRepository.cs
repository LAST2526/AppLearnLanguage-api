using Last02.Data.Entities;
using Last02.Data.Repositories.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Data.Repositories.Implements
{
    public class MemberFlashcardRepository : Repository<MemberFlashcard>, IMemberFlashcardRepository
    {
        private readonly ApplicationDbContext _context;
        public MemberFlashcardRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
