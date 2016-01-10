namespace Logbook.Shared.Configuration
{
    /// <summary>
    /// A <see cref="bool"/> setting.
    /// </summary>
    public class BoolSetting : BaseSetting<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoolSetting"/> class.
        /// </summary>
        /// <param name="appSettingsKey">The application settings key.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        public BoolSetting(string appSettingsKey, bool defaultValue)
            : base(appSettingsKey, defaultValue)
        {
        }

        /// <summary>
        /// Tries to parse the specified <paramref name="stringValue" />. 
        /// Returns whether parsing was successfull.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        /// <param name="value">The value.</param>
        protected override bool TryParse(string stringValue, out bool value)
        {
            return bool.TryParse(stringValue, out value);
        }
    }
}