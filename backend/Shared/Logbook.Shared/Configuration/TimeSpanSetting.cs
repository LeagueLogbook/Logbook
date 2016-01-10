using System;
using JetBrains.Annotations;

namespace Logbook.Shared.Configuration
{
    /// <summary>
    /// A <see cref="TimeSpan"/> setting.
    /// </summary>
    public class TimeSpanSetting : BaseSetting<TimeSpan>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSpanSetting"/> class.
        /// </summary>
        /// <param name="appSettingsKey">The application settings key.</param>
        /// <param name="defaultValue">The default value.</param>
        public TimeSpanSetting([NotNull] string appSettingsKey, TimeSpan defaultValue)
            : base(appSettingsKey, defaultValue)
        {
        }

        /// <summary>
        /// Tries to parse the specified <paramref name="stringValue" />.
        /// Returns whether parsing was successfull.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected override bool TryParse(string stringValue, out TimeSpan value)
        {
            return TimeSpan.TryParse(stringValue, out value);
        }
    }
}