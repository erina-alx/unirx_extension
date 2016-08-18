using System;
using UniRx;
using UnityEngine.Events;

namespace UniRxExtensions
{
    // ReSharper disable once InconsistentNaming
    public static class UnityEvent_InvokeByTuple
    {
        public static void Invoke<T0>(
            this UnityEvent<T0> unityEvent,
            Tuple<T0> tuple
            )
        {
            if (unityEvent == null)
                throw new ArgumentNullException("unityEvent");

            unityEvent.Invoke(tuple.Item1);
        }

        public static void Invoke<T0, T1>(
            this UnityEvent<T0, T1> unityEvent,
            Tuple<T0, T1> tuple
            )
        {
            if (unityEvent == null)
                throw new ArgumentNullException("unityEvent");

            unityEvent.Invoke(tuple.Item1, tuple.Item2);
        }

        public static void Invoke<T0, T1, T2>(
            this UnityEvent<T0, T1, T2> unityEvent,
            Tuple<T0, T1, T2> tuple
            )
        {
            if (unityEvent == null)
                throw new ArgumentNullException("unityEvent");

            unityEvent.Invoke(tuple.Item1, tuple.Item2, tuple.Item3);
        }

        public static void Invoke<T0, T1, T2, T3>(
            this UnityEvent<T0, T1, T2, T3> unityEvent,
            Tuple<T0, T1, T2, T3> tuple
            )
        {
            if (unityEvent == null)
                throw new ArgumentNullException("unityEvent");

            unityEvent.Invoke(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
        }
    }
}
