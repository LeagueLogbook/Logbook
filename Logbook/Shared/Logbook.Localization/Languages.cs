using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logbook.Shared.Extensions;

namespace Logbook.Localization
{
    public static class Languages
    {
        /// <summary>
        /// Returns all available languages.
        /// </summary>
        public static CultureInfo[] GetSupportedLanguages()
        {
            return new[]
            {
                CultureInfo.CreateSpecificCulture("en"),
                CultureInfo.CreateSpecificCulture("de"),
            };
        }
        /// <summary>
        /// Returns the default language.
        /// </summary>
        public static CultureInfo GetDefaultLanguage()
        {
            return GetSupportedLanguages().First();
        }
        /// <summary>
        /// Returns the language that matches the specified <paramref name="languageName"/>.
        /// </summary>
        /// <param name="languageName">Name of the language in the form of <c>en</c> or <c>en-US</c>.</param>
        public static CultureInfo GetLanguageByName(string languageName)
        {
            List<CultureInfo> allLanguages = GetSupportedLanguages().Traverse(f => new[] { f.Parent }).ToList();

            CultureInfo foundLanguage = allLanguages
                .FirstOrDefault(f => string.Equals(f.Name, languageName, StringComparison.InvariantCultureIgnoreCase));

            if (foundLanguage != null)
                return foundLanguage;

            if (languageName.Contains("-"))
                languageName = languageName.Split('-')[0];

            foundLanguage = allLanguages.FirstOrDefault(f => string.Equals(f.Name, languageName, StringComparison.InvariantCultureIgnoreCase));

            if (foundLanguage != null)
                return foundLanguage;

            return GetDefaultLanguage();
        }
    }
}
