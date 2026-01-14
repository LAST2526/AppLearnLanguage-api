using ClosedXML.Excel;
using DocumentFormat.OpenXml.Math;
using Last02.Commons;
using Last02.Data;
using Last02.Data.Entities;
using Last02.Models.Dtos;
using Last02.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Services.Implement
{
    public class AudioGenHistoryService : IAudioGenHistoryService
    {
        private readonly ApplicationDbContext _context;
        public AudioGenHistoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Data.Entities.AudioGenHistory>> GetAllAsync()
        {
            return await _context.AudioGenHistories.OrderByDescending(x => x.CreatedDate).ToListAsync();

        }

        public async Task<Data.Entities.AudioGenHistory?> GetByIdAsync(int id)
        {
            return await _context.AudioGenHistories.FindAsync(id);

        }

        public async Task AddHistoryAndClearThenAddRangeAudioAsync(Data.Entities.AudioGenHistory entity, IEnumerable<Audio> audios, IEnumerable<Audio> audiosToRemove)
        {
            _context.AudioGenHistories.Add(entity);
            _context.Audios.RemoveRange(audiosToRemove);
            await _context.Audios.AddRangeAsync(audios);
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Data.Entities.AudioGenHistory> Data, int TotalRecords)> SearchAsync(string keyword, int page, int size)
        {
            var query = _context.AudioGenHistories.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.FileName.Contains(keyword));
            }
            var totalRecords = await query.CountAsync();
            var data = await query.OrderByDescending(x => x.CreatedDate)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
            return (data, totalRecords);
        }

        public List<ExcelRowDto<Audio>> GetAudiosFromExcel(IFormFile excelFile)
        {
            var audios = new List<ExcelRowDto<Audio>>();

            var workbook = new XLWorkbook(excelFile.OpenReadStream());
            var worksheets = workbook.Worksheets.ToList() ?? throw new Exception("Worksheet not found");
            foreach (var worksheet in worksheets)
            {
                var used = worksheet.RangeUsed();
                if (used == null) continue;

                int firstRow = used.FirstRow().RowNumber();
                int lastRow = used.LastRow().RowNumber();
                int lastCol = used.LastColumn().ColumnNumber();

                int firstDataRow = firstRow + 1;

                string Cell(int r, int c)
                    => c <= lastCol ? worksheet.Cell(r, c).GetString() ?? string.Empty : string.Empty;

                var rowNumberKaiwa = 0;
                var rowNumberGrammar = 0;
                var isFreeKaiwa = true;
                var isFreeGrammar = true;

                for (int r = firstDataRow; r <= lastRow; r++)
                {
                    var qrCode = Cell(r, 1); // A
                    var title = Cell(r, 2); // B
                    var titleVi = Cell(r, 3); // C
                    var titleEn = Cell(r, 4); // D
                    var script = Cell(r, 5); // E
                    var scriptVi = Cell(r, 6); // F
                    var scriptEn = Cell(r, 7); // G
                    var fileUrl = Cell(r, 8); // H

                    if (string.IsNullOrEmpty(qrCode))
                    {
                        continue;
                    }

                    var type = qrCode.StartsWith("R") ? AudioType.Grammar : AudioType.Kaiwa;

                    //if (type == AudioType.Grammar)
                    //{
                    //    rowNumberGrammar++;
                    //    if (rowNumberGrammar > 5) isFreeGrammar = false;
                    //}
                    //else
                    //{
                    //    rowNumberKaiwa++;
                    //    if (rowNumberKaiwa > 5) isFreeKaiwa = false;
                    //}

                    var audioCode = qrCode[..1] + "-" + Guid.NewGuid();

                    var audio = new Audio
                    {
                        Title = title,
                        TitleVi = titleVi,
                        TitleEn = titleEn,
                        AudioType = type,
                        AudioCode = audioCode,
                        FileUrl = fileUrl,
                        SortOrder = type == AudioType.Grammar ? rowNumberGrammar : rowNumberKaiwa,
                        IsFree = type == AudioType.Grammar ? isFreeGrammar : isFreeKaiwa,
                        Script = script,
                        ScriptVi = scriptVi,
                        ScriptEn = scriptEn,
                    };

                    audios.Add(new ExcelRowDto<Audio>
                    {
                        SheetName = worksheet.Name,
                        RowNumber = r,
                        Data = audio
                    });
                }
            }

            return audios;
        }

        public async Task CreateAudioModelAsync(List<ExcelRowDto<Audio>> audios, int[] courseIds, string fileUrl, string fileName)
        {
            var listCourses = new List<Course>();

            if (audios == null || audios.Count == 0)
            {
                throw new Exception("Audios are null or empty");
            }
            var course = _context.Course.Where(c => !c.Deleted).FirstOrDefault(c => c.Id == courseIds.First());
            var history = new AudioGenHistory
            {
                CourseTitle = course?.Title ?? string.Empty,
                FileUrl = fileUrl,
                FileName = fileName,
                CreatedDate = DateTime.UtcNow
            };

            using var transaction = await _context.Database.BeginTransactionAsync();
            var audiosToAdd = new List<Audio>();
            var audiosToRemove = new List<Audio>();
            foreach (var courseId in courseIds)
            {
                audiosToAdd.AddRange(audios.Select(x =>
                {
                    var data = new Audio
                    {
                        Title = x.Data.Title,
                        TitleVi = x.Data.TitleVi,
                        TitleEn = x.Data.TitleEn,
                        AudioType = x.Data.AudioType,
                        CourseId = courseId,
                        AudioCode = x.Data.AudioCode,
                        FileUrl = x.Data.FileUrl,
                        SortOrder = x.Data.SortOrder,
                        IsFree = x.Data.IsFree,
                        Script = x.Data.Script,
                        ScriptVi = x.Data.ScriptVi,
                        ScriptEn = x.Data.ScriptEn,
                    };
                    return data;
                }));
                audiosToRemove.AddRange(await _context.Audios.Where(a => a.CourseId == courseId).ToListAsync());
            }
            await AddHistoryAndClearThenAddRangeAudioAsync(history, audiosToAdd, audiosToRemove);

            await transaction.CommitAsync();
        }
    }
}
