using System;
using UniRx;
using UniRxExtensions.Core;

namespace UniRxExtensions
{
    // ReSharper disable once InconsistentNaming
    public static class _IObservable_SubscribeWithStateWithoutEvent
    {
        #region for 1 state

        public static IDisposable SubscribeWithStateWithoutEvent<T, TState>(
            this IObservable<T> observable,
            TState state,
            Action<TState> onNext
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            return observable.SubscribeWithState(
                new ValueSet<TState, Action<TState>>(
                    state,
                    onNext
                    ),
                (_, x) => x.Item2(x.Item1)
                );
        }

        public static IDisposable SubscribeWithStateWithoutEvent<T, TState>(
            this IObservable<T> observable,
            TState state,
            Action<TState> onNext,
            Action<Exception, TState> onError
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");

            return observable.SubscribeWithState(
                new ValueSet<TState, Action<TState>, Action<Exception, TState>>(
                    state,
                    onNext,
                    onError
                    ),
                (_, x) => x.Item2(x.Item1),
                (error, x) => x.Item3(error, x.Item1)
                );
        }

        public static IDisposable SubscribeWithStateWithoutEvent<T, TState>(
            this IObservable<T> observable,
            TState state,
            Action<TState> onNext,
            Action<Exception, TState> onError,
            Action<TState> onCompleted
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

            return observable.SubscribeWithState(
                new ValueSet<TState, Action<TState>, Action<Exception, TState>, Action<TState>>(
                    state,
                    onNext,
                    onError,
                    onCompleted
                    ),
                (_, x) => x.Item2(x.Item1),
                (error, x) => x.Item3(error, x.Item1),
                x => x.Item4(x.Item1)
                );
        }

        public static IDisposable SubscribeWithStateWithoutEvent<T, TState>(
            this IObservable<T> observable,
            TState state,
            Action<TState> onNext,
            Action<TState> onCompleted
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            return observable.SubscribeWithState(
                new ValueSet<TState, Action<TState>, Action<TState>>(
                    state,
                    onNext,
                    onCompleted
                    ),
                (_, x) => x.Item2(x.Item1),
                x => x.Item3(x.Item1)
                );
        }

        #endregion

        #region for 2 states

        public static IDisposable SubscribeWithState2WithoutEvent<T, TState1, TState2>(
            this IObservable<T> observable,
            TState1 state1,
            TState2 state2,
            Action<TState1, TState2> onNext
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            return observable.SubscribeWithState(
                new ValueSet<TState1, TState2, Action<TState1, TState2>>(
                    state1,
                    state2,
                    onNext
                    ),
                (_, x) => x.Item3(x.Item1, x.Item2)
                );
        }

        public static IDisposable SubscribeWithState2WithoutEvent<T, TState1, TState2>(
            this IObservable<T> observable,
            TState1 state1,
            TState2 state2,
            Action<TState1, TState2> onNext,
            Action<Exception, TState1, TState2> onError
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");

            return observable.SubscribeWithState(
                new ValueSet<TState1, TState2, Action<TState1, TState2>, Action<Exception, TState1, TState2>>(
                    state1,
                    state2,
                    onNext,
                    onError
                    ),
                (_, x) => x.Item3(x.Item1, x.Item2),
                (error, x) => x.Item4(error, x.Item1, x.Item2)
                );
        }

        public static IDisposable SubscribeWithState2WithoutEvent<T, TState1, TState2>(
            this IObservable<T> observable,
            TState1 state1,
            TState2 state2,
            Action<TState1, TState2> onNext,
            Action<Exception, TState1, TState2> onError,
            Action<TState1, TState2> onCompleted
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

            return observable.SubscribeWithState(
                new ValueSet<
                    TState1,
                    TState2,
                    Action<TState1, TState2>,
                    Action<Exception, TState1, TState2>,
                    Action<TState1, TState2>
                    >(
                    state1,
                    state2,
                    onNext,
                    onError,
                    onCompleted
                    ),
                (_, x) => x.Item3(x.Item1, x.Item2),
                (error, x) => x.Item4(error, x.Item1, x.Item2),
                x => x.Item5(x.Item1, x.Item2)
                );
        }

        public static IDisposable SubscribeWithState2WithoutEvent<T, TState1, TState2>(
            this IObservable<T> observable,
            TState1 state1,
            TState2 state2,
            Action<TState1, TState2> onNext,
            Action<TState1, TState2> onCompleted
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            return observable.SubscribeWithState(
                new ValueSet<TState1, TState2, Action<TState1, TState2>, Action<TState1, TState2>>(
                    state1,
                    state2,
                    onNext,
                    onCompleted
                    ),
                (_, x) => x.Item3(x.Item1, x.Item2),
                x => x.Item4(x.Item1, x.Item2)
                );
        }

        #endregion

        #region for 3 states

        public static IDisposable SubscribeWithState3WithoutEvent<T, TState1, TState2, TState3>(
            this IObservable<T> observable,
            TState1 state1,
            TState2 state2,
            TState3 state3,
            Action<TState1, TState2, TState3> onNext
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            return observable.SubscribeWithState(
                new ValueSet<TState1, TState2, TState3, Action<TState1, TState2, TState3>>(
                    state1,
                    state2,
                    state3,
                    onNext
                    ),
                (_, x) => x.Item4(x.Item1, x.Item2, x.Item3)
                );
        }

        public static IDisposable SubscribeWithState3WithoutEvent<T, TState1, TState2, TState3>(
            this IObservable<T> observable,
            TState1 state1,
            TState2 state2,
            TState3 state3,
            Action<TState1, TState2, TState3> onNext,
            Action<Exception, TState1, TState2, TState3> onError
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");

            return observable.SubscribeWithState(
                new ValueSet<
                    TState1,
                    TState2,
                    TState3,
                    Action<TState1, TState2, TState3>,
                    Action<Exception, TState1, TState2, TState3>
                    >(
                    state1,
                    state2,
                    state3,
                    onNext,
                    onError
                    ),
                (_, x) => x.Item4(x.Item1, x.Item2, x.Item3),
                (error, x) => x.Item5(error, x.Item1, x.Item2, x.Item3)
                );
        }

        public static IDisposable SubscribeWithState3WithoutEvent<T, TState1, TState2, TState3>(
            this IObservable<T> observable,
            TState1 state1,
            TState2 state2,
            TState3 state3,
            Action<TState1, TState2, TState3> onNext,
            Action<Exception, TState1, TState2, TState3> onError,
            Action<TState1, TState2, TState3> onCompleted
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

            return observable.SubscribeWithState(
                new ValueSet<
                    TState1,
                    TState2,
                    TState3,
                    Action<TState1, TState2, TState3>,
                    Action<Exception, TState1, TState2, TState3>,
                    Action<TState1, TState2, TState3>
                    >(
                    state1,
                    state2,
                    state3,
                    onNext,
                    onError,
                    onCompleted
                    ),
                (_, x) => x.Item4(x.Item1, x.Item2, x.Item3),
                (error, x) => x.Item5(error, x.Item1, x.Item2, x.Item3),
                x => x.Item6(x.Item1, x.Item2, x.Item3)
                );
        }

        public static IDisposable SubscribeWithState3WithoutEvent<T, TState1, TState2, TState3>(
            this IObservable<T> observable,
            TState1 state1,
            TState2 state2,
            TState3 state3,
            Action<TState1, TState2, TState3> onNext,
            Action<TState1, TState2, TState3> onCompleted
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            return observable.SubscribeWithState(
                new ValueSet<
                    TState1,
                    TState2,
                    TState3,
                    Action<TState1, TState2, TState3>,
                    Action<TState1, TState2, TState3>
                    >(
                    state1,
                    state2,
                    state3,
                    onNext,
                    onCompleted
                    ),
                (_, x) => x.Item4(x.Item1, x.Item2, x.Item3),
                x => x.Item5(x.Item1, x.Item2, x.Item3)
                );
        }

        #endregion
    }
}
