using System;
using System.Collections.Generic;
using UniRxExtensions;

namespace UniRx
{
    public static partial class Observable
    {
        public static IObservable<T> ChainAll<T>(this IEnumerable<IObservable<T>> enumerable)
        {
            if (enumerable == null)
                throw new ArgumentNullException("enumerable");

            return ChainAll(enumerable.GetEnumerator(), null, default(T), lastEventExists: false);
        }

        public static IObservable<T[]> ChainAllAndCollectAllEvents<T>(
            this IEnumerable<IObservable<T>> enumerable
            )
        {
            if (enumerable == null)
                throw new ArgumentNullException("enumerable");

            var list = new List<T>();

            return ChainAll(
                enumerable.GetEnumerator(),
                list,
                default(T),
                lastEventExists: false
                )
                .Select(_ => list.ToArray());
        }

        private static IObservable<T> ChainAll<T>(
            IEnumerator<IObservable<T>> enumerator,
            ICollection<T> events,
            T lastEvent,
            bool lastEventExists
            )
        {
            if (enumerator.MoveNext())
            {
                if (enumerator.Current == null)
                    throw new InvalidOperationException();

                return enumerator.Current.ChainTo(
                    (@event, _) =>
                    {
                        if (events != null)
                            events.Add(@event);

                        return ChainAll(enumerator, events, @event, lastEventExists: true);
                    }
                    );
            }

            if (lastEventExists)
                return Return(lastEvent);

            return Empty<T>();
        }

        public static IObservable<T> ChainAll<T>(this IEnumerable<Func<IObservable<T>>> enumerable)
        {
            if (enumerable == null)
                throw new ArgumentNullException("enumerable");

            return ChainAll(enumerable.GetEnumerator(), null, default(T), lastEventExsits: false);
        }

        public static IObservable<T[]> ChainAllAndCollectAllEvents<T>(
            this IEnumerable<Func<IObservable<T>>> enumerable
            )
        {
            if (enumerable == null)
                throw new ArgumentNullException("enumerable");

            var list = new List<T>();

            return ChainAll(
                enumerable.GetEnumerator(),
                list,
                default(T),
                lastEventExsits: false
                )
                .Select(_ => list.ToArray());
        }

        private static IObservable<T> ChainAll<T>(
            IEnumerator<Func<IObservable<T>>> enumerator,
            ICollection<T> events,
            T lastEvent,
            bool lastEventExsits
            )
        {
            if (enumerator.MoveNext())
            {
                if (enumerator.Current == null)
                    throw new InvalidOperationException();

                return enumerator.Current().ChainTo(
                    (@event, _) =>
                    {
                        if (events != null)
                            events.Add(@event);

                        return ChainAll(enumerator, events, @event, lastEventExsits: true);
                    }
                    );
            }

            if (lastEventExsits)
                return Return(lastEvent);

            return Empty<T>();
        }
    }
}
