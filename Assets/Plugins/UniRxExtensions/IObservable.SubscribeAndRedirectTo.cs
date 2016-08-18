using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine.Events;

namespace UniRxExtensions
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Extension methods to redirect events to other object from stream.
    /// </summary>
    public static class _IObservable_SubscribeAndRedirectTo
    {
        /// <summary>
        /// Redirect events to a instance of ISubject{T}.
        /// </summary>
        public static IDisposable SubscribeAndRedirectTo<T>(
            this IObservable<T> observable,
            ISubject<T> subject
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (subject == null)
                throw new ArgumentNullException("subject");

            return observable.SubscribeWithState(
                subject,
                (@event, x) => x.OnNext(@event)
                );
        }

        /// <summary>
        /// Redirect events to an instance of UnityEvent{T}.
        /// </summary>
        public static IDisposable SubscribeAndRedirectTo<T>(
            this IObservable<T> observable,
            UnityEvent<T> unityEvent
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (unityEvent == null)
                throw new ArgumentNullException("unityEvent");

            return observable.SubscribeWithState(
                unityEvent,
                (@event, x) => x.Invoke(@event)
                );
        }

        /// <summary>
        /// Redirect events to an instance of ICollection{T}.
        /// </summary>
        public static IDisposable SubscribeAndRedirectTo<T>(
            this IObservable<T> observable,
            ICollection<T> collection
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (collection == null)
                throw new ArgumentNullException("collection");

            return observable.SubscribeWithState(
                collection,
                (@event, x) => x.Add(@event)
                );
        }

        /// <summary>
        /// Redirect events to an instance of Stack{T}.
        /// </summary>
        public static IDisposable SubscribeAndRedirectTo<T>(
            this IObservable<T> observable,
            Stack<T> stack
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (stack == null)
                throw new ArgumentNullException("stack");

            return observable.SubscribeWithState(
                stack,
                (@event, x) => x.Push(@event)
                );
        }

        /// <summary>
        /// Redirect events to an instance of Queue{T}.
        /// </summary>
        public static IDisposable SubscribeAndRedirectTo<T>(
            this IObservable<T> observable,
            Queue<T> queue
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (queue == null)
                throw new ArgumentNullException("queue");

            return observable.SubscribeWithState(
                queue,
                (@event, x) => x.Enqueue(@event)
                );
        }

        /// <summary>
        /// Redirect events to an instance of IDictionary{,}.
        /// </summary>
        public static IDisposable SubscribeAndRedirectTo<T, TKey, TValue>(
            this IObservable<T> observable,
            IDictionary<TKey, TValue> dictionary,
            Func<T, TKey> keySelector,
            Func<T, TValue> valueSelector
            )
        {
            if (observable == null)
                throw new ArgumentNullException("observable");
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (valueSelector == null)
                throw new ArgumentNullException("valueSelector");

            return observable.SubscribeWithState3(
                dictionary,
                keySelector,
                valueSelector,
                (@event, x, y, z) => x.Add(y(@event), z(@event))
                );
        }
    }
}
