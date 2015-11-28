namespace Logbook.Shared.Results
{
    public enum ResultState
    {
        /// <summary>
        /// The operation was successfull.
        /// </summary>
        Success,

        /// <summary>
        /// The operation was successfull, but there is a warning message.
        /// </summary>
        Warning,

        /// <summary>
        /// The operation failed and there is a error message.
        /// </summary>
        Error
    }
}