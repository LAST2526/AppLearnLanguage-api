using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Commons.Extensions
{
    public static class LanguageExtentions
    {
        //From enum to code
        public static string GetLanguageCode(this Language lang)
        {
            var member = lang.GetType().GetMember(lang.ToString()).FirstOrDefault();
            var attr = member?.GetCustomAttribute<LanguageCodeAttribute>();

            if (attr == null)
                throw new InvalidOperationException($"Missing LanguageCode attribute on enum {lang}");

            return attr.Code;
        }

        //From code to enum
        public static Language FromCode(string code)
        {
            foreach (var value in Enum.GetValues(typeof(Language)).Cast<Language>())
            {
                if (string.Equals(value.GetLanguageCode(), code, StringComparison.OrdinalIgnoreCase))
                {
                    return value;
                }
            }

            throw new ArgumentException($"Invalid language code: {code}");
        }

        // From code to enum (safe)
        public static bool TryFromCode(string code, out Language lang)
        {
            try
            {
                lang = FromCode(code);
                return true;
            }
            catch
            {
                lang = default;
                return false;
            }
        }

        // List all codes
        public static IEnumerable<string> GetAllCodes()
        {
            return Enum.GetValues(typeof(Language))
                .Cast<Language>()
                .Select(e => e.GetLanguageCode());
        }

        // Validate
        public static bool IsValidCode(string code)
        {
            return GetAllCodes().Contains(code, StringComparer.OrdinalIgnoreCase);
        }
    }
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class LanguageCodeAttribute : Attribute
    {
        public string Code { get; }

        public LanguageCodeAttribute(string code)
        {
            Code = code;
        }
    }
}
