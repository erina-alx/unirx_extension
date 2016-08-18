using System;
using UniRx;

namespace UniRxExtensions.Tests.Utils
{
    public static class TestStream
    {
        public static IObservable<T> Make<T>(params T[] events)
        {
            var replaySubject = new ReplaySubject<T>();

            foreach (var @event in events)
                replaySubject.OnNext(@event);

            replaySubject.OnCompleted();

            return replaySubject;
        }

        public static IObservable<Unit> MakeWithError(Func<Exception> errorSelector)
        {
            return MakeWithError<Unit>(errorSelector);
        }

        public static IObservable<T> MakeWithError<T>(
            Func<Exception> errorSelector,
            params T[] events
            )
        {
            var replaySubject = new ReplaySubject<T>();

            foreach (var @event in events)
                replaySubject.OnNext(@event);

            replaySubject.OnError(errorSelector());

            return replaySubject;
        }
    }
}
