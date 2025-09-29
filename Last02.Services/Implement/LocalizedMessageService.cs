using Last02.Data;
using Last02.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Services.Implement
{
    public class LocalizedMessageService : ILocalizedMessageService
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalizedMessageService(IMemoryCache cache, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetCurrentLanguageCode()
        {
            var acceptLanguage = _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString();

            if (string.IsNullOrEmpty(acceptLanguage))
                return "en";

            // Ex. Accept-Language: "vi,en;q=0.9,ja;q=0.8"
            // Get first culture code đầu tiên: "vi"
            var firstLang = acceptLanguage.Split(',').FirstOrDefault();
            if (string.IsNullOrEmpty(firstLang))
                return "en";

            // Remove q=
            var langCode = firstLang.Split(';').FirstOrDefault()?.Trim();

            // If invalid, return "en"
            if (string.IsNullOrEmpty(langCode))
                return "en";


            return langCode;
        }

        public async Task<string> GetMessageAsync(string code)
        {
            var lang = GetCurrentLanguageCode();

            var cacheKey = $"msg:{code}:{lang}";
            if (_cache.TryGetValue(cacheKey, out string? message))
                return message ?? $"[{code}]";

            var result = await _context.LocalizedMessages
                .Where(m => m.Code == code && m.LanguageCode == lang)
                .Select(m => m.Message)
                .FirstOrDefaultAsync();

            if (result != null)
                _cache.Set(cacheKey, result, TimeSpan.FromHours(1));

            return result ?? $"[{code}]";
        }
    }
}
