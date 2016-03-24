using System;

namespace Logbook.Shared.Common
{
    public class NullDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}