using System;
using UniRx;
using Object = UnityEngine.Object;

namespace UniRxExtensions.Resources
{
    // ReSharper disable once InconsistentNaming
    public static class _IObservable_ResourceRequestProgressEvent_SkipWhileInProgress
    {
        public static IObservable<ResourceRequestProgressEvent> SkipWhileInProgress(
            this IObservable<ResourceRequestProgressEvent> source
            )
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.SkipWhile(x => !x.IsDone);
        }

        public static IObservable<ResourceRequestProgressEvent<T>> SkipWhileInProgress<T>(
            this IObservable<ResourceRequestProgressEvent<T>> source
            )
            where T : Object
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.SkipWhile(x => !x.IsDone);
        }
    }
}
