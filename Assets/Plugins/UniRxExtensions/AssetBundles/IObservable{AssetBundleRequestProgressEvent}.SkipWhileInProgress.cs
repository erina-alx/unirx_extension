using System;
using UniRx;
using Object = UnityEngine.Object;

namespace UniRxExtensions.AssetBundles
{
#if USE_FIXED_COMPILER
    // ReSharper disable once InconsistentNaming
    public static class _IObservable_AssetBundleRequestProgressEvent_SkipWhileInProgress
    {
        public static IObservable<AssetBundleRequestProgressEvent<T>>
            SkipWhileInProgress<T>(this IObservable<AssetBundleRequestProgressEvent<T>> source)
            where T : Object
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.SkipWhile(x => !x.IsDone);
        }
    }
#endif
}
