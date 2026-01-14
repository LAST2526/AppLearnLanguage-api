using Last02.Commons;
using Last02.Data.Entities;
using Last02.Models.Dtos;
using Last02.Services.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<ResponseBase<Course?>> GetCourseByIdAsync(int id);
        Task<IEnumerable<Course>> GetCoursesByIdsAsync(int[] ids);
        Task<DataTablePage<CourseCreateDto>> AdminSearch(string keyword, int page, int size);
        Task<Course> Create(Course course);
        Task<bool> IsTitleDuplicateAsync(string title, List<string> languages, int[]? excludeIds = null);
        Task<Course?> AdminGetById(int id);
        Task<List<Course>> AdminGetByIds(int[] ids);
        Task<Course> Update(Course course);
        Task Delete(int[] ids);
        Task<Course> CreateFromDto(CourseCreateDto model);
        Task<Course> UpdateFromDto(CourseCreateDto model);
    }
}
