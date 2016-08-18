using System;
using UniRx;

namespace UniRxExtensions
{
    // ReSharper disable once InconsistentNaming
    public static class _IObservable_SubscribeWithoutEvent
    {
        public static IDisposable SubscribeWithoutEvent<T>(
            this IObservable<T> observable,
            Action onNext
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            return observable.SubscribeWithState(onNext, (_, x) => x());
        }

        public static IDisposable SubscribeWithoutEvent<T>(
            this IObservable<T> observable,
            Action onNext,
            Action<Exception> onError
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            return observable.SubscribeWithState2(
                onNext,
                onError,
                (_, x, __) => x(),
                (error, _, x) => x(error)
                );
        }

        public static IDisposable SubscribeWithoutEvent<T>(
            this IObservable<T> observable,
            Action onNext,
            Action<Exception> onError,
            Action onCompleted
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            return observable.SubscribeWithState3(
                onNext,
                onError,
                onCompleted,
                (_, x, __, ___) => x(),
                (error, _, x, __) => x(error),
                (_, __, x) => x()
                );
        }

        public static IDisposable SubscribeWithoutEvent<T>(
            this IObservable<T> observable,
            Action onNext,
            Action onCompleted
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            return observable.SubscribeWithState2(
                onNext,
                onCompleted,
                (_, x, __) => x(),
                (_, x) => x()
                );
        }
    }
}
