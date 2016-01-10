namespace Logbook.Shared.Configuration
{
    /// <summary>
    /// A <see cref="int"/> setting.
    /// </summary>
    public class IntSetting : BaseSetting<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntSetting"/> class.
        /// </summary>
        /// <param name="appSettingsKey">The application settings key.</param>
        /// <param name="defaultValue">The default value.</param>
        public IntSetting(string appSettingsKey, int defaultValue)
            : base(appSettingsKey, defaultValue)
        {
        }

        /// <summary>
        /// Tries to parse the specified <paramref name="stringValue" />. 
        /// Returns whether parsing was successfull.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        /// <param name="value">The value.</param>
        protected override bool TryParse(string stringValue, out int value)
        {
            return int.TryParse(stringValue, out value);
        }
    }
}