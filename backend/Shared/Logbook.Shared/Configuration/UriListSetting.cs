using System;
using System.Collections.Generic;
using System.Linq;

namespace Logbook.Shared.Configuration
{
    /// <summary>
    /// A setting for <see cref="List{T}"/>.
    /// </summary>
    public class UriListSetting : BaseSetting<List<Uri>>
    {
        private readonly string _separator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UriListSetting"/> class.
        /// </summary>
        /// <param name="appSettingsKey">The application settings key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="separator">The separator.</param>
        public UriListSetting(string appSettingsKey, List<Uri> defaultValue, string separator)
            : base(appSettingsKey, defaultValue)
        {
            this._separator = separator;
        }

        /// <summary>
        /// Tries the parse the specified <paramref name="stringValue" />. Returns whether parsing was successfull.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        /// <param name="value">The value.</param>
        protected override bool TryParse(string stringValue, out List<Uri> value)
        {
            string[] parts = stringValue.Split(new[] { this._separator }, StringSplitOptions.RemoveEmptyEntries);

            var result = new List<Uri>();

            foreach (string part in parts)
            {
                Uri uri;
                if (Uri.TryCreate(part, UriKind.Absolute, out uri))
                {
                    result.Add(uri);
                }
            }

            value = result;

            return result.Any();
        }
    }
}