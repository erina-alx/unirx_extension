using System;

namespace UniRxExtensions.Core
{
    internal static class DummyDisposable
    {
        public static readonly IDisposable Instance = new Disposable();

        private class Disposable :
            IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
