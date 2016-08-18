using System;
using System.Collections.Generic;
using UniRxExtensions.Core;

namespace UniRx
{
    public static partial class Observable
    {
        public static IObservable<T> FromEnumerator<T>(
            IEnumerator<T> enumerator,
            bool disposeOnEnd = false
            )
        {
            if (enumerator == null)
                throw new ArgumentNullException("enumerator");

            return Create<T>(
                observer =>
                {
                    try
                    {
                        while (enumerator.MoveNext())
                            observer.OnNext(enumerator.Current);

                        observer.OnCompleted();

                        return DummyDisposable.Instance;
                    }
                    finally
                    {
                        if (disposeOnEnd)
                            enumerator.Dispose();
                    }
                }
                );
        }

        public static IObservable<T> FromListEnumerator<T>(
            List<T>.Enumerator enumerator,
            bool disposeOnEnd = false
            )
        {
            return Create<T>(
                observer =>
                {
                    try
                    {
                        while (enumerator.MoveNext())
                            observer.OnNext(enumerator.Current);

                        observer.OnCompleted();

                        return DummyDisposable.Instance;
                    }
                    finally
                    {
                        if (disposeOnEnd)
                            enumerator.Dispose();
                    }
                }
                );
        }

        public static IObservable<T> FromLinkedListEnumerator<T>(
            LinkedList<T>.Enumerator enumerator,
            bool disposeOnEnd = false
            )
        {
            return Create<T>(
                observer =>
                {
                    try
                    {
                        while (enumerator.MoveNext())
                            observer.OnNext(enumerator.Current);

                        observer.OnCompleted();

                        return DummyDisposable.Instance;
                    }
                    finally
                    {
                        if (disposeOnEnd)
                            enumerator.Dispose();
                    }
                }
                );
        }
    }
}
