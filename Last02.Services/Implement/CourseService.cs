using Amazon.S3.Model;
using DocumentFormat.OpenXml.Office2010.Excel;
using Last02.Commons;
using Last02.Commons.Extensions;
using Last02.Data;
using Last02.Data.Entities;
using Last02.Models.Dtos;
using Last02.Services.Interfaces;
using Last02.Services.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Language = Last02.Commons.Language;

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

        public async Task<ResponseBase<Course?>> GetCourseByIdAsync(int id)
        {
            var course = await Task.FromResult(GetCoursesQuery()
                .FirstOrDefault(c => c.Id == id));

            if (course == null)
            {
                return ResponseBase<Course?>.Error(await _messageService.GetMessageAsync(MessageCodes.Course.ERR_COURSE_NOT_FOUND), statusCode: 404);
            }

            return ResponseBase<Course?>.Success(course);
        }

        public async Task<IEnumerable<Course>> GetCoursesByIdsAsync(int[] ids)
        {
            return await Task.FromResult(GetCoursesQuery()
                .Where(c => ids.Contains(c.Id))
                .ToList());
        }

        public async Task<DataTablePage<CourseCreateDto>> AdminSearch(string keyword, int page, int size)
        {
            var courses = GetCoursesQuery();
            int totalRecords = courses.Count();
            if (!string.IsNullOrEmpty(keyword))
            {
                courses = courses.Where(c => c != null && c.Title != null && c.Title.Contains(keyword));
            }
            var coursesDto = courses.GroupBy(c => c.Title).Select(g => new CourseCreateDto
            {
                Id = string.Join(",", g.Select(c => c.Id)),
                Title = g.Key ?? "",
                CreatedDate = g.Select(c => c.CreatedDate).First()
            });
            coursesDto = coursesDto.OrderByDescending(c => c.CreatedDate);
            return await Task.FromResult(new DataTablePage<CourseCreateDto>
            {
                Data = [.. coursesDto.Skip((page - 1) * size).Take(size)],
                TotalRecords = totalRecords,
                RecordsFiltered = coursesDto.Count(),
                TotalPages = (int)Math.Ceiling(coursesDto.Count() / (double)size),
                Page = page,
                Size = size
            });
        }

        public async Task<Course> Create(Course course)
        {
            course.CreatedDate = DateTime.UtcNow;
            _context.Course.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> IsTitleDuplicateAsync(string title, List<string> languages, int[]? excludeIds = null)
        {
            var languagesEnum = languages.Select(l => LanguageExtensions.FromCode(l)).ToList();
            return await GetCoursesQuery()
                .AnyAsync(c => c != null && c.Title != null && c.Title.ToLower() == title.ToLower() && (excludeIds == null || !excludeIds.Contains(c.Id)));
        }

        public async Task<Course?> AdminGetById(int id)
        {
            return await GetCoursesQuery()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Course>> AdminGetByIds(int[] ids)
        {
            return await GetCoursesQuery()
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();
        }

        public async Task<Course> Update(Course course)
        {
            _context.Course.Update(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task Delete(int[] ids)
        {
            var courses = await GetCoursesQuery()
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();
            courses.ForEach(s =>
            {
                s.Deleted = true;
            });
            _context.Course.UpdateRange(courses);
            await _context.SaveChangesAsync();
        }

        public async Task<Course> CreateFromDto(CourseCreateDto model)
        {
            var course = new Course
            {
                Title = model.Title,
            };
            await _context.Course.AddAsync(course);
            await _context.SaveChangesAsync();

            return course;
        }

        public async Task<Course> UpdateFromDto(CourseCreateDto model)
        {
            var id = int.Parse(model.Id);
            var course = await _context.Course.FindAsync(id);

            if (course == null)
                throw new Exception("Course not found");

            course.Title = model.Title;

            await _context.SaveChangesAsync();
            return course;
        }
    }
}
