using System.Configuration;
using JetBrains.Annotations;

namespace Logbook.Shared.Configuration
{
    /// <summary>
    /// A base class for settings in the AppSettings area.
    /// </summary>
    /// <typeparam name="T">The actual setting type.</typeparam>
    public abstract class BaseSetting<T>
    {
        #region Fields
        private readonly string _appSettingsKey;
        private readonly T _defaultValue;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSetting{T}"/> class.
        /// </summary>
        /// <param name="appSettingsKey">The application settings key.</param>
        /// <param name="defaultValue">The default value.</param>
        protected BaseSetting([NotNull]string appSettingsKey, [NotNull]T defaultValue)
        {
            this._appSettingsKey = appSettingsKey;
            this._defaultValue = defaultValue;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the settings value.
        /// </summary>
        [NotNull]
        public T GetValue()
        {
            string stringValue = ConfigurationManager.AppSettings.Get(this._appSettingsKey) ?? string.Empty;

            T value;
            if (this.TryParse(stringValue, out value))
                return value;

            return this._defaultValue;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Tries to parse the specified <paramref name="stringValue"/>.
        /// Returns whether parsing was successfull.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        /// <param name="value">The value.</param>
        protected abstract bool TryParse(string stringValue, out T value);
        #endregion

        #region Operators
        /// <summary>
        /// Performs an implicit conversion from <see cref="BaseSetting{T}"/> to <see cref="T"/>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        public static implicit operator T(BaseSetting<T> setting)
        {
            return setting.GetValue();
        }
        #endregion
    }
}