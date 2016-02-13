namespace Logbook.Server.Contracts.Encryption
{
    public interface ISaltCombiner : IService
    {
        /// <summary>
        /// Combines the specified <paramref name="password"/> with the specified <paramref name="salt"/>.
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <param name="iterationCount">The iteration count.</param>
        /// <param name="password">The password.</param>
        byte[] Combine(byte[] salt, int iterationCount, string password);
    }
}