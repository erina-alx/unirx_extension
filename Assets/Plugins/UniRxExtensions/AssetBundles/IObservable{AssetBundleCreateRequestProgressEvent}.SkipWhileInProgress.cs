using System;
using UniRx;

namespace UniRxExtensions.AssetBundles
{
    // ReSharper disable once InconsistentNaming
    public static class _IObservable_AssetBundleCreateRequestProgressEvent_SkipWhileInProgress
    {
        public static IObservable<AssetBundleCreateRequestProgressEvent> SkipWhileInProgress(
            this IObservable<AssetBundleCreateRequestProgressEvent> source
            )
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.SkipWhile(x => !x.IsDone);
        }
    }
}
