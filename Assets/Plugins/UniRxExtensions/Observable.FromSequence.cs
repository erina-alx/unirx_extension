using System;
using System.Collections.Generic;
using UniRxExtensions.Core;

namespace UniRx
{
    public static partial class Observable
    {
        public static IObservable<T> FromSequence<T>(IEnumerable<T> sequence)
        {
            if (sequence == null)
                throw new ArgumentNullException("sequence");

            return Create<T>(
                observer =>
                {
                    using (var enumerator = sequence.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                            observer.OnNext(enumerator.Current);

                        observer.OnCompleted();

                        return DummyDisposable.Instance;
                    }
                }
                );
        }

        public static IObservable<T> FromSequence<T>(IList<T> sequence)
        {
            if (sequence == null)
                throw new ArgumentNullException("sequence");

            return Create<T>(
                observer =>
                {
                    for (var i = 0; i < sequence.Count; i++)
                        observer.OnNext(sequence[i]);

                    observer.OnCompleted();

                    return DummyDisposable.Instance;
                }
                );
        }

        public static IObservable<T> FromSequence<T>(T[] sequence)
        {
            if (sequence == null)
                throw new ArgumentNullException("sequence");

            return Create<T>(
                observer =>
                {
                    foreach (var x in sequence)
                        observer.OnNext(x);

                    observer.OnCompleted();

                    return DummyDisposable.Instance;
                }
                );
        }
    }
}
