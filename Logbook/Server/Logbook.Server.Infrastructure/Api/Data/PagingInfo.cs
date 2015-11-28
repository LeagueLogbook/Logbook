namespace Logbook.Server.Infrastructure.Api.Data
{
    public class PagingInfo
    {
        #region Properties
        /// <summary>
        /// Gets the amount of elements to skip.
        /// </summary>
        public int Skip { get; private set; }
        /// <summary>
        /// Gets the amounf of elements to take.
        /// </summary>
        public int Take { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PagingInfo"/> class.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        public PagingInfo(int skip, int take)
        {
            this.Skip = skip;
            this.Take = take;
        }
        #endregion
    }
}