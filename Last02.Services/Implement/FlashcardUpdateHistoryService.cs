using ClosedXML.Excel;
using Last02.Data;
using Last02.Data.Entities;
using Last02.Models.Dtos;
using Last02.Services.Interfaces;
using Last02.Services.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Services.Implement
{
    public class FlashcardUpdateHistoryService(ApplicationDbContext _context) : IFlashcardUpdateHistoryService
    {
        public async Task<FlashcardUpdateHistory> CreateAsync(FlashcardUpdateHistory history)
        {
            await _context.FlashcardUpdateHistories.AddAsync(history);
            await _context.SaveChangesAsync();
            return history;
        }

        public async Task<FlashcardUpdateHistory?> GetAsync(int id)
        {
            return await _context.FlashcardUpdateHistories.FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<IEnumerable<FlashcardUpdateHistory>> GetAllAsync()
        {
            return await _context.FlashcardUpdateHistories.OrderByDescending(h => h.CreatedDate).ToListAsync();
        }

        public async Task<DataTablePage<FlashcardUpdateHistory>> SearchAsync(string keyword, int page, int size)
        {
            var query = _context.FlashcardUpdateHistories.AsQueryable();
            var totalRecords = await query.CountAsync();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(h => h.CourseTitle != null && h.CourseTitle.Contains(keyword));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query.OrderByDescending(h => h.CreatedDate)
                .Skip((page - 1) * size).Take(size).ToListAsync();
            return new DataTablePage<FlashcardUpdateHistory>
            {
                Data = data,
                TotalRecords = totalRecords,
                RecordsFiltered = recordsFiltered,
                TotalPages = (int)Math.Ceiling(recordsFiltered / (double)size),
                Page = page,
                Size = size
            };
        }

        public async Task CreateTopicAndFlashcardModelAsync(
            Dictionary<string, TopicNames> topicMap,
            List<ExcelRowDto<Flashcard>> flashcards,
            int[] courseIds,
            string fileUrl,
            string fileName)
        {
            List<string> topicColors =
            [
                "#FECA45", "#3FCD2F", "#2FCDA5", "#2F98CD", "#2F41CD",
        "#892FCD", "#E06DCC", "#F35A5A", "#F3955A", "#FF7B16"
            ];

            var listCourses = new List<Course>();

            if (flashcards == null || flashcards.Count == 0)
                throw new Exception("Flashcards are null or empty");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var courseId in courseIds)
                {
                    var course = await _context.Course.FindAsync(courseId)
                                 ?? throw new Exception("Course not found");
                    listCourses.Add(course);

                    var sheetNames = flashcards.Select(f => f.SheetName).Distinct().ToList();

                    var topics = await _context.Topics.Where(t => t.CourseId == courseId).ToListAsync();

                    // remove flashcards in topics
                    foreach (var topic in topics)
                    {
                        var flashcardsInTopic = await _context.Flashcards
                            .Where(f => f.Topic != null && f.Topic.Id == topic.Id)
                            .ToListAsync();

                        _context.Flashcards.RemoveRange(flashcardsInTopic);
                    }

                    _context.Topics.RemoveRange(topics);

                    for (int i = 0; i < sheetNames.Count; i++)
                    {
                        var sheet = sheetNames[i];

                        var flashcardsInSheet = flashcards
                            .Where(f => f.SheetName == sheet)
                            .Select(f => new Flashcard
                            {
                                Front = f.Data.Front,
                                Furigana = f.Data.Furigana,
                                MeaningVi = f.Data.MeaningVi,
                                MeaningEn = f.Data.MeaningEn,
                                Example = f.Data.Example,
                                ExampleEn = f.Data.ExampleEn,
                                ExampleVi = f.Data.ExampleVi,
                                ImageUrl = f.Data.ImageUrl,
                            })
                            .ToList();

                        TopicNames? tnames = null;
                        if (topicMap != null)
                            topicMap.TryGetValue(NormalizeKey(sheet), out tnames);

                        var topicCode = string.Concat("TP" + courseId + "-", Guid.NewGuid().ToString().AsSpan(0, 4));

                        var topic = new Topic
                        {
                            CourseId = courseId,
                            TopicCode = topicCode,
                            Title = tnames?.Title ?? sheet,
                            TitleVi = tnames?.TitleVi,
                            TitleEn = tnames?.TitleEn,
                            Description = "",
                            HexColorCode = topicColors[i % topicColors.Count],
                            IsFree = true
                        };

                        await _context.Topics.AddAsync(topic);
                        await _context.SaveChangesAsync();

                        if (flashcardsInSheet.Count > 0)
                        {
                            foreach (var flashcard in flashcardsInSheet)
                                flashcard.TopicId = topic.Id;

                            await _context.Flashcards.AddRangeAsync(flashcardsInSheet);
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                var history = new FlashcardUpdateHistory
                {
                    CourseTitle = listCourses.FirstOrDefault()?.Title ?? string.Empty,
                    FileUrl = fileUrl,
                    FileName = fileName,
                    CreatedDate = DateTime.UtcNow
                };
                await CreateAsync(history);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        // ======================================================================
        // EXCEL READERS
        // ======================================================================
        // NOTE: This version reads by fixed column indexes (A..D) instead of header names.
        // Expected layout in the "Topics" sheet:
        //   Column A: Vietnamese (VI)
        //   Column B: English (EN)
        // Row 1 is assumed to be header -> data starts at (firstRow + 1).
        public Dictionary<string, TopicNames> GetTopicsFromExcel(IFormFile excelFile)
        {
            var result = new Dictionary<string, TopicNames>(StringComparer.OrdinalIgnoreCase);

            using var workbook = new XLWorkbook(excelFile.OpenReadStream());

            // Prefer a worksheet explicitly named "Topics" or "TopicNames"
            var topicSheet = workbook.Worksheets
                .FirstOrDefault(ws => IsTopicsSheet(ws.Name));

            // If there is no topics sheet, just return empty map (caller will fallback to sheet names)
            if (topicSheet == null)
                return result;

            var used = topicSheet.RangeUsed();
            if (used == null) return result;

            int firstRow = used.FirstRow().RowNumber();
            int lastRow = used.LastRow().RowNumber();

            // Fixed indices (1-based): JA=A(1), VI=B(2), EN=C(3)
            const int colJa = 1;
            const int colVi = 2;
            const int colEn = 3;

            string Cell(int r, int c)
                => (topicSheet.Cell(r, c).GetString() ?? string.Empty).Trim();

            // Assume row 1 is header; start reading from row 2
            for (int r = firstRow + 1; r <= lastRow; r++)
            {
                var ja = Cell(r, colJa);
                if (string.IsNullOrEmpty(ja)) continue;

                var names = new TopicNames
                {
                    Title = ja,
                    TitleVi = Cell(r, colVi),
                    TitleEn = Cell(r, colEn),
                };

                var key = NormalizeKey(ja);
                if (!result.ContainsKey(key))
                    result[key] = names;
            }

            return result;
        }

        public List<ExcelRowDto<Flashcard>> GetFlashcardsFromExcel(IFormFile excelFile)
        {
            var flashcards = new List<ExcelRowDto<Flashcard>>();

            var workbook = new XLWorkbook(excelFile.OpenReadStream());
            var worksheets = workbook.Worksheets.ToList() ?? throw new Exception("Worksheet not found");

            foreach (var worksheet in worksheets)
            {
                if (IsTopicsSheet(worksheet.Name)) continue;

                var used = worksheet.RangeUsed();
                if (used == null) continue;

                int firstRow = used.FirstRow().RowNumber();
                int lastRow = used.LastRow().RowNumber();
                int lastCol = used.LastColumn().ColumnNumber();

                int firstDataRow = firstRow + 1;

                string Cell(int r, int c)
                    => c <= lastCol ? worksheet.Cell(r, c).GetString() ?? string.Empty : string.Empty;

                for (int r = firstDataRow; r <= lastRow; r++)
                {
                    var front = Cell(r, 1);  // A
                    var furigana = Cell(r, 2);  // B
                    var meaningVi = Cell(r, 3);  // C
                    var meaningEn = Cell(r, 4);  // D
                    var example = Cell(r, 5);  // E
                    var exampleVi = Cell(r, 6);  // F
                    var exampleEn = Cell(r, 7);  // G
                    var imageUrl = Cell(r, 8);  // H

                    if (string.IsNullOrEmpty(front))
                        continue;

                    flashcards.Add(new ExcelRowDto<Flashcard>
                    {
                        SheetName = worksheet.Name,
                        RowNumber = r,
                        Data = new Flashcard
                        {
                            Front = front,
                            Furigana = furigana,
                            MeaningVi = meaningVi,
                            MeaningEn = meaningEn,
                            Example = example,
                            ExampleVi = exampleVi,
                            ExampleEn = exampleEn,
                            ImageUrl = imageUrl
                        },
                    });
                }
            }
            return flashcards;
        }

        public async Task<FlashcardUpdateHistory?> GetByFileUrlAsync(string fileUrl)
        {
            return await _context.FlashcardUpdateHistories.FirstOrDefaultAsync(h => h.FileUrl == fileUrl);
        }

        private static string NormalizeKey(string s)
        {
            // Normalize to compare sheet name vs. JA title: lowercase + trim + remove whitespaces
            if (string.IsNullOrWhiteSpace(s)) return "";
            return new string(s.Trim().ToLowerInvariant().Where(ch => !char.IsWhiteSpace(ch)).ToArray());
        }

        private static bool IsTopicsSheet(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            var n = name.Trim();
            return n.Equals("Topics", StringComparison.OrdinalIgnoreCase)
                   || n.Equals("TopicNames", StringComparison.OrdinalIgnoreCase);
        }

        public class TopicNames
        {
            public string Title { get; set; } = "";
            public string TitleEn { get; set; } = "";
            public string TitleVi { get; set; } = "";
        }
    }
}
