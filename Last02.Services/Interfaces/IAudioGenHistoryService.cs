using Last02.Data.Entities;
using Last02.Models.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Services.Interfaces
{
    public interface IAudioGenHistoryService
    {
        Task<IEnumerable<AudioGenHistory>> GetAllAsync();
        Task<AudioGenHistory?> GetByIdAsync(int id);
        Task AddHistoryAndClearThenAddRangeAudioAsync(AudioGenHistory entity, IEnumerable<Audio> audios, IEnumerable<Audio> audiosToRemove);
        Task<(IEnumerable<AudioGenHistory> Data, int TotalRecords)> SearchAsync(string keyword, int page, int size);
        List<ExcelRowDto<Audio>> GetAudiosFromExcel(IFormFile excelFile);
        Task CreateAudioModelAsync(List<ExcelRowDto<Audio>> audios, int[] courseIds, string fileUrl, string fileName);
    }
}
