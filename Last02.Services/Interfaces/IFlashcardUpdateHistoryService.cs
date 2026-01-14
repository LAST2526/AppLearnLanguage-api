using Last02.Services.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last02.Data.Entities;
using Last02.Models.Dtos;
using Last02.Services.Implement;

namespace Last02.Services.Interfaces
{
    public interface IFlashcardUpdateHistoryService
    {
        Task<FlashcardUpdateHistory> CreateAsync(FlashcardUpdateHistory history);
        Task<FlashcardUpdateHistory?> GetAsync(int id);
        Task<IEnumerable<FlashcardUpdateHistory>> GetAllAsync();
        Task<DataTablePage<FlashcardUpdateHistory>> SearchAsync(string keyword, int page, int size);
        Task CreateTopicAndFlashcardModelAsync(Dictionary<string, FlashcardUpdateHistoryService.TopicNames> topicMap, List<ExcelRowDto<Flashcard>> flashcards, int[] courseIds, string fileUrl, string fileName);

        Dictionary<string, FlashcardUpdateHistoryService.TopicNames> GetTopicsFromExcel(IFormFile excelFile);
        List<ExcelRowDto<Flashcard>> GetFlashcardsFromExcel(IFormFile excelFile);

        Task<FlashcardUpdateHistory?> GetByFileUrlAsync(string fileUrl);
    }
}
