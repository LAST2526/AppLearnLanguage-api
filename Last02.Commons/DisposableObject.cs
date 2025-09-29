using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last02.Commons
{
    public abstract class DisposableObject : IDisposable
    {
        protected DisposableObject()
        {
            Disposables = [];
        }
        public virtual void Dispose(bool isDisposing)
        {
            if (_isDisposed) return;

            _isDisposed = true;

            if (isDisposing)
            {
                foreach (var disposable in Disposables)
                {
                    disposable.Dispose();
                }
            }

            Disposables = [];
        }
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        void IDisposable.Dispose()
        {
            Dispose();
        }

        private bool _isDisposed = false;

        protected IList<IDisposable> Disposables { get; private set; }
    }
}
