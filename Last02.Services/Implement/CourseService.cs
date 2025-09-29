using Last02.Data;
using Last02.Data.Entities;
using Last02.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Services.Implement
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILocalizedMessageService _messageService;

        public CourseService(ApplicationDbContext context, ILocalizedMessageService messageService)
        {
            _context = context;
            _messageService = messageService;
        }

        private IQueryable<Course> GetCoursesQuery()
        {
            return _context.Course.Where(c => !c.Deleted);
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await Task.FromResult(GetCoursesQuery()
                .ToList());
        }
    }
}
