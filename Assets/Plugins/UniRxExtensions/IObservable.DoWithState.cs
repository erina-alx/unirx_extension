using System;
using UniRx;
using UniRxExtensions.Core;
using UniRxExtensions.Operators;

namespace UniRxExtensions
{
    // ReSharper disable once InconsistentNaming
    public static class _IObservable_DoWithState
    {
        #region for 1 state

        public static IObservable<T> DoWithState<T, TState>(
            this IObservable<T> source,
            TState state,
            Action<T, TState> onNext
            )
        {
            return new DoWithStateObservable<T, TState>(
                source,
                state,
                onNext,
                Stubs<TState>.Throw,
                Stubs<TState>.Nop
                );
        }

        public static IObservable<T> DoWithState<T, TState>(
            this IObservable<T> source,
            TState state,
            Action<T, TState> onNext,
            Action<Exception, TState> onError
            )
        {
            return new DoWithStateObservable<T, TState>(
                source,
                state,
                onNext,
                onError,
                Stubs<TState>.Nop
                );
        }

        public static IObservable<T> DoWithState<T, TState>(
            this IObservable<T> source,
            TState state,
            Action<T, TState> onNext,
            Action<TState> onCompleted
            )
        {
            return new DoWithStateObservable<T, TState>(
                source,
                state,
                onNext,
                Stubs<TState>.Throw,
                onCompleted
                );
        }

        public static IObservable<T> DoWithState<T, TState>(
            this IObservable<T> source,
            TState state,
            Action<T, TState> onNext,
            Action<Exception, TState> onError,
            Action<TState> onCompleted
            )
        {
            return new DoWithStateObservable<T, TState>(
                source,
                state,
                onNext,
                onError,
                onCompleted
                );
        }

        #endregion

        #region for 2 states

        public static IObservable<T> DoWithState2<T, TState1, TState2>(
            this IObservable<T> source,
            TState1 state1,
            TState2 state2,
            Action<T, TState1, TState2> onNext
            )
        {
            return source.DoWithState(
                new ValueSet<TState1, TState2, Action<T, TState1, TState2>>(state1, state2, onNext),
                (@event, x) => x.Item3(@event, x.Item1, x.Item2)
                );
        }

        public static IObservable<T> DoWithState2<T, TState1, TState2>(
            this IObservable<T> source,
            TState1 state1,
            TState2 state2,
            Action<T, TState1, TState2> onNext,
            Action<Exception, TState1, TState2> onError
            )
        {
            return source.DoWithState(
                new ValueSet<
                    TState1,
                    TState2,
                    Action<T, TState1, TState2>,
                    Action<Exception, TState1, TState2>
                    >(state1, state2, onNext, onError),
                (@event, x) => x.Item3(@event, x.Item1, x.Item2),
                (error, x) => x.Item4(error, x.Item1, x.Item2)
                );
        }

        public static IObservable<T> DoWithState2<T, TState1, TState2>(
            this IObservable<T> source,
            TState1 state1,
            TState2 state2,
            Action<T, TState1, TState2> onNext,
            Action<TState1, TState2> onCompleted
            )
        {
            return source.DoWithState(
                new ValueSet<
                    TState1,
                    TState2,
                    Action<T, TState1, TState2>,
                    Action<TState1, TState2>
                    >(state1, state2, onNext, onCompleted),
                (@event, x) => x.Item3(@event, x.Item1, x.Item2),
                x => x.Item4(x.Item1, x.Item2)
                );
        }

        public static IObservable<T> DoWithState2<T, TState1, TState2>(
            this IObservable<T> source,
            TState1 state1,
            TState2 state2,
            Action<T, TState1, TState2> onNext,
            Action<Exception, TState1, TState2> onError,
            Action<TState1, TState2> onCompleted
            )
        {
            return source.DoWithState(
                new ValueSet<
                    TState1,
                    TState2,
                    Action<T, TState1, TState2>,
                    Action<Exception, TState1, TState2>,
                    Action<TState1, TState2>
                    >(state1, state2, onNext, onError, onCompleted),
                (@event, x) => x.Item3(@event, x.Item1, x.Item2),
                (error, x) => x.Item4(error, x.Item1, x.Item2),
                x => x.Item5(x.Item1, x.Item2)
                );
        }

        #endregion

        #region for 3 states

        public static IObservable<T> DoWithState3<T, TState1, TState2, TState3>(
            this IObservable<T> source,
            TState1 state1,
            TState2 state2,
            TState3 state3,
            Action<T, TState1, TState2, TState3> onNext
            )
        {
            return source.DoWithState(
                new ValueSet<
                    TState1,
                    TState2,
                    TState3,
                    Action<T, TState1, TState2, TState3>
                    >(state1, state2, state3, onNext),
                (@event, x) => x.Item4(@event, x.Item1, x.Item2, x.Item3)
                );
        }

        public static IObservable<T> DoWithState3<T, TState1, TState2, TState3>(
            this IObservable<T> source,
            TState1 state1,
            TState2 state2,
            TState3 state3,
            Action<T, TState1, TState2, TState3> onNext,
            Action<Exception, TState1, TState2, TState3> onError
            )
        {
            return source.DoWithState(
                new ValueSet<
                    TState1,
                    TState2,
                    TState3,
                    Action<T, TState1, TState2, TState3>,
                    Action<Exception, TState1, TState2, TState3>
                    >(state1, state2, state3, onNext, onError),
                (@event, x) => x.Item4(@event, x.Item1, x.Item2, x.Item3),
                (error, x) => x.Item5(error, x.Item1, x.Item2, x.Item3)
                );
        }

        public static IObservable<T> DoWithState3<T, TState1, TState2, TState3>(
            this IObservable<T> source,
            TState1 state1,
            TState2 state2,
            TState3 state3,
            Action<T, TState1, TState2, TState3> onNext,
            Action<TState1, TState2, TState3> onCompleted
            )
        {
            return source.DoWithState(
                new ValueSet<
                    TState1,
                    TState2,
                    TState3,
                    Action<T, TState1, TState2, TState3>,
                    Action<TState1, TState2, TState3>
                    >(state1, state2, state3, onNext, onCompleted),
                (@event, x) => x.Item4(@event, x.Item1, x.Item2, x.Item3),
                x => x.Item5(x.Item1, x.Item2, x.Item3)
                );
        }

        public static IObservable<T> DoWithState3<T, TState1, TState2, TState3>(
            this IObservable<T> source,
            TState1 state1,
            TState2 state2,
            TState3 state3,
            Action<T, TState1, TState2, TState3> onNext,
            Action<Exception, TState1, TState2, TState3> onError,
            Action<TState1, TState2, TState3> onCompleted
            )
        {
            return source.DoWithState(
                new ValueSet<
                    TState1,
                    TState2,
                    TState3,
                    Action<T, TState1, TState2, TState3>,
                    Action<Exception, TState1, TState2, TState3>,
                    Action<TState1, TState2, TState3>
                    >(state1, state2, state3, onNext, onError, onCompleted),
                (@event, x) => x.Item4(@event, x.Item1, x.Item2, x.Item3),
                (error, x) => x.Item5(error, x.Item1, x.Item2, x.Item3),
                x => x.Item6(x.Item1, x.Item2, x.Item3)
                );
        }

        #endregion

        private static class Stubs<TState>
        {
            public static readonly Action<TState> Nop = _ => { };
            public static readonly Action<Exception, TState> Throw = (error, _) => Stubs.Throw(error);
        }
    }
}
