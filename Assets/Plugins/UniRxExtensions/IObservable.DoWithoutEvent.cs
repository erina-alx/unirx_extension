using System;
using UniRx;

namespace UniRxExtensions
{
    // ReSharper disable once InconsistentNaming
    public static class _IObservable_DoWithoutEvent
    {
        public static IObservable<T> DoWithoutEvent<T>(
            this IObservable<T> observable,
            Action onNext
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            return observable.DoWithState(onNext, (_, x) => x());
        }

        public static IObservable<T> DoWithoutEvent<T>(
            this IObservable<T> observable,
            Action onNext,
            Action<Exception> onError
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");

            return observable.DoWithState2(
                onNext,
                onError,
                (_, x, __) => x(),
                (error, _, x) => x(error)
                );
        }

        public static IObservable<T> DoWithoutEvent<T>(
            this IObservable<T> observable,
            Action onNext,
            Action onCompleted
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            return observable.DoWithState2(
                onNext,
                onCompleted,
                (_, x, __) => x(),
                (_, x) => x()
                );
        }

        public static IObservable<T> DoWithoutEvent<T>(
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
            if (onError == null)
                throw new ArgumentNullException("onError");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            return observable.DoWithState3(
                onNext,
                onError,
                onCompleted,
                (_, x, __, ___) => x(),
                (error, _, x, __) => x(error),
                (_, __, x) => x()
                );
        }
    }
}
