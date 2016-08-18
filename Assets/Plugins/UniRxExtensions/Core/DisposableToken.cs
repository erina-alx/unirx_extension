using System;

namespace UniRxExtensions.Core
{
    internal class DisposableToken :
        IDisposable
    {
        private bool disposed;

        public void Dispose()
        {
            disposed = true;
        }

        public bool IsDisposed
        {
            get { return disposed; }
        }
    }
}
