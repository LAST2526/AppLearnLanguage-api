using DocumentFormat.OpenXml.Wordprocessing;
using FluentValidation.Resources;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Last02.Api
{
    public class CustomLanguageManager : LanguageManager
    {
        public CustomLanguageManager()
        {
            Enabled = true;

            var resourceBaseName = "Last02.Models.Resources.ValidationMessages";
            var assembly = Assembly.Load("Last02.Models");
            var resourceManager = new ResourceManager(resourceBaseName, assembly);

            var supportedCultures = new[] { "vi", "en", "ja" };

            foreach (var cultureCode in supportedCultures)
            {
                var culture = new CultureInfo(cultureCode);

                // Load all key-value pairs from the .resx file for this culture
                var resourceSet = resourceManager.GetResourceSet(culture, true, true);
                if (resourceSet == null) continue;

                foreach (System.Collections.DictionaryEntry entry in resourceSet)
                {
                    string key = entry.Key.ToString()!;
                    string value = entry.Value?.ToString()!;
                    AddTranslation(cultureCode, key, value);
                }
            }
        }
    }
}
