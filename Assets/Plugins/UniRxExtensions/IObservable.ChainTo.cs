using System;
using System.Collections.Generic;
using UniRx;

namespace UniRxExtensions
{
    // ReSharper disable once InconsistentNaming
    public static class _IObservable_ChainTo
    {
        public static IObservable<TNext> ChainTo<T, TNext>(
            this IObservable<T> source,
            IObservable<TNext> nextStream
            )
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (nextStream == null)
                throw new ArgumentNullException("nextStream");

            return source.ChainTo_Core((_, __) => nextStream, useAllEvent: false);
        }

        public static IObservable<TNext> ChainTo<T, TNext>(
            this IObservable<T> source,
            Func<IObservable<TNext>> nextStreamSelector
            )
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (nextStreamSelector == null)
                throw new ArgumentNullException("nextStreamSelector");

            return source.ChainTo_Core((_, __) => nextStreamSelector(), useAllEvent: false);
        }

        public static IObservable<TNext> ChainTo<T, TNext>(
            this IObservable<T> source,
            Func<T, IObservable<TNext>> nextStreamSelector
            )
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (nextStreamSelector == null)
                throw new ArgumentNullException("nextStreamSelector");

            return source.ChainTo_Core((lastEvent, _) => nextStreamSelector(lastEvent), useAllEvent: false);
        }

        public static IObservable<TNext> ChainTo<T, TNext>(
            this IObservable<T> source,
            Func<T, List<T>, IObservable<TNext>> nextStreamSelector
            )
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (nextStreamSelector == null)
                throw new ArgumentNullException("nextStreamSelector");

            return source.ChainTo_Core(nextStreamSelector, useAllEvent: true);
        }

        private static IObservable<TNext> ChainTo_Core<T, TNext>(
            this IObservable<T> source,
            Func<T, List<T>, IObservable<TNext>> nextStreamSelector,
            bool useAllEvent
            )
        {
            return Observable.Create<TNext>(
                observer =>
                {
                    var state = new Set<T, TNext>(
                        observer,
                        useAllEvent ? new List<T>() : null,
                        new CompositeDisposable(capacity: 2),
                        nextStreamSelector
                        );

                    source.SubscribeWithState(
                        state,
                        (@event, x) =>
                        {
                            if (!x.CompositeDisposable.IsDisposed)
                                if (x.Events != null)
                                    x.Events.Add(@event);

                            x.LastEvent = @event;
                        },
                        (error, x) =>
                        {
                            if (!x.CompositeDisposable.IsDisposed)
                                x.Observer.OnError(error);
                        },
                        x =>
                        {
                            if (!x.CompositeDisposable.IsDisposed)
                                x.NextStreamSelector(x.LastEvent, x.Events)
                                    .SubscribeWithState(
                                        x,
                                        // ReSharper disable once InconsistentNaming
                                        (_event, _x) =>
                                        {
                                            if (!_x.CompositeDisposable.IsDisposed)
                                                _x.Observer.OnNext(_event);
                                        },
                                        // ReSharper disable once InconsistentNaming
                                        (error, _x) =>
                                        {
                                            if (!_x.CompositeDisposable.IsDisposed)
                                                _x.Observer.OnError(error);
                                        },
                                        // ReSharper disable once InconsistentNaming
                                        _x =>
                                        {
                                            if (!_x.CompositeDisposable.IsDisposed)
                                                _x.Observer.OnCompleted();
                                        }
                                    )
                                    .AddTo(x.CompositeDisposable);
                        }
                        )
                        .AddTo(state.CompositeDisposable);

                    return state.CompositeDisposable;
                }
                );
        }

        private class Set<TPrevious, TNext>
        {
            public readonly IObserver<TNext> Observer;
            public readonly List<TPrevious> Events;
            public readonly CompositeDisposable CompositeDisposable;
            public readonly Func<TPrevious, List<TPrevious>, IObservable<TNext>> NextStreamSelector;
            public TPrevious LastEvent;

            public Set(
                IObserver<TNext> observer,
                List<TPrevious> events,
                CompositeDisposable compositeDisposable,
                Func<TPrevious, List<TPrevious>, IObservable<TNext>> nextStreamSelector
                )
            {
                Observer = observer;
                Events = events;
                CompositeDisposable = compositeDisposable;
                NextStreamSelector = nextStreamSelector;
            }
        }
    }
}
