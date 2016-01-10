using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Logbook.Shared.Common
{
    public class CultureAwaiter : INotifyCompletion
    {
        private readonly TaskAwaiter _awaiter;
        private CultureInfo _culture;
        private CultureInfo _uiCulture;

        public CultureAwaiter(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            this._awaiter = task.GetAwaiter();
        }

        public CultureAwaiter GetAwaiter()
        {
            return this;
        }

        public bool IsCompleted
        {
            get { return this._awaiter.IsCompleted; }
        }

        public void OnCompleted(Action continuation)
        {
            this._culture = Thread.CurrentThread.CurrentCulture;
            this._uiCulture = Thread.CurrentThread.CurrentUICulture;

            this._awaiter.OnCompleted(continuation);
        }

        public void GetResult()
        {
            if (this._culture != null)
                Thread.CurrentThread.CurrentCulture = this._culture;

            if (this._uiCulture != null)
                Thread.CurrentThread.CurrentUICulture = this._uiCulture;

            this._awaiter.GetResult();
        }
    }

    public class CultureAwaiter<T> : INotifyCompletion
    {
        private readonly TaskAwaiter<T> _awaiter;
        private CultureInfo _culture;
        private CultureInfo _uiCulture;

        public CultureAwaiter(Task<T> task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));


            this._awaiter = task.GetAwaiter();
        }

        public CultureAwaiter<T> GetAwaiter()
        {
            return this;
        }

        public bool IsCompleted
        {
            get { return this._awaiter.IsCompleted; }
        }

        public void OnCompleted(Action continuation)
        {
            this._culture = Thread.CurrentThread.CurrentCulture;
            this._uiCulture = Thread.CurrentThread.CurrentUICulture;

            this._awaiter.OnCompleted(continuation);
        }

        public T GetResult()
        {
            if (this._culture != null)
                Thread.CurrentThread.CurrentCulture = this._culture;

            if (this._uiCulture != null)
                Thread.CurrentThread.CurrentUICulture = this._uiCulture;

            return this._awaiter.GetResult();
        }
    }
}