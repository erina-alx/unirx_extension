using System;
using System.Collections.Generic;

namespace UniRx
{
    public static partial class Observable
    {
        public static IDisposable Synchronize<T>(
            IReactiveProperty<T> first,
            IReactiveProperty<T> second
            )
        {
            return Synchronize(
                first,
                second,
                GetDefaultComparer<T>()
                );
        }

        public static IDisposable Synchronize<T>(
            IReactiveProperty<T> first,
            IReactiveProperty<T> second,
            IEqualityComparer<T> comparer
            )
        {
            return Synchronize(
                first,
                second,
                comparer,
                x => second.Value = x,
                x => first.Value = x
                );
        }

        public static IDisposable Synchronize<T>(
            IObservable<T> first,
            IObservable<T> second,
            Action<T> mutatorForSecond,
            Action<T> mutatorForFirst
            )
        {
            return Synchronize(
                first,
                second,
                GetDefaultComparer<T>(),
                mutatorForSecond,
                mutatorForFirst
                );
        }

        public static IDisposable Synchronize<T>(
            IObservable<T> first,
            IObservable<T> second,
            IEqualityComparer<T> comparer,
            Action<T> mutatorForSecond,
            Action<T> mutatorForFirst
            )
        {
            return Synchronize(
                first,
                comparer,
                DefaultSelector<T>.Get(),
                second,
                comparer,
                DefaultSelector<T>.Get(),
                mutatorForSecond,
                mutatorForFirst
                );
        }

        public static IDisposable Synchronize<TFirst, TSecond>(
            IReactiveProperty<TFirst> first,
            Func<TFirst, TSecond> firstValueSelector,
            IReactiveProperty<TSecond> second,
            Func<TSecond, TFirst> secondValueSelector
            )
        {
            return Synchronize(
                first,
                GetDefaultComparer<TFirst>(),
                firstValueSelector,
                second,
                GetDefaultComparer<TSecond>(),
                secondValueSelector,
                x => second.Value = x,
                x => first.Value = x
                );
        }

        public static IDisposable Synchronize<TFirst, TSecond>(
            IReactiveProperty<TFirst> first,
            IEqualityComparer<TFirst> comparerForFirst,
            Func<TFirst, TSecond> firstValueSelector,
            IReactiveProperty<TSecond> second,
            IEqualityComparer<TSecond> comparerForSecond,
            Func<TSecond, TFirst> secondValueSelector
            )
        {
            return Synchronize(
                first,
                comparerForFirst,
                firstValueSelector,
                second,
                comparerForSecond,
                secondValueSelector,
                x => second.Value = x,
                x => first.Value = x
                );
        }

        public static IDisposable Synchronize<TFirst, TSecond>(
            IObservable<TFirst> first,
            Func<TFirst, TSecond> firstValueSelector,
            IObservable<TSecond> second,
            Func<TSecond, TFirst> secondValueSelector,
            Action<TSecond> mutatorForSecond,
            Action<TFirst> mutatorForFirst
            )
        {
            return Synchronize(
                first,
                GetDefaultComparer<TFirst>(),
                firstValueSelector,
                second,
                GetDefaultComparer<TSecond>(),
                secondValueSelector,
                mutatorForSecond,
                mutatorForFirst
                );
        }

        public static IDisposable Synchronize<TFirst, TSecond>(
            IObservable<TFirst> first,
            IEqualityComparer<TFirst> comparerForFirst,
            Func<TFirst, TSecond> firstValueSelector,
            IObservable<TSecond> second,
            IEqualityComparer<TSecond> comparerForSecond,
            Func<TSecond, TFirst> secondValueSelector,
            Action<TSecond> mutatorForSecond,
            Action<TFirst> mutatorForFirst
            )
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (comparerForFirst == null)
                throw new ArgumentNullException("comparerForFirst");
            if (firstValueSelector == null)
                throw new ArgumentNullException("firstValueSelector");
            if (second == null)
                throw new ArgumentNullException("second");
            if (comparerForSecond == null)
                throw new ArgumentNullException("comparerForSecond");
            if (secondValueSelector == null)
                throw new ArgumentNullException("secondValueSelector");
            if (mutatorForSecond == null)
                throw new ArgumentNullException("mutatorForSecond");
            if (mutatorForFirst == null)
                throw new ArgumentNullException("mutatorForFirst");

            var compositeDisposable = new CompositeDisposable();

            first.DistinctUntilChanged(comparerForFirst)
                .Select(firstValueSelector)
                .Subscribe(mutatorForSecond)
                .AddTo(compositeDisposable);

            second.DistinctUntilChanged(comparerForSecond)
                .Select(secondValueSelector)
                .Subscribe(mutatorForFirst)
                .AddTo(compositeDisposable);

            return compositeDisposable;
        }

        private static IEqualityComparer<T> GetDefaultComparer<T>()
        {
#if !UniRxLibrary
            return UnityEqualityComparer.GetDefault<T>();
#else
            return EqualityComparer<T>.Default;
#endif
        }

        private static class DefaultSelector<T>
        {
            private static readonly Func<T, T> Func = x => x;

            public static Func<T, T> Get()
            {
                return Func;
            }
        }
    }
}
