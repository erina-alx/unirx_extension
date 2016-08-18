using System;
using UniRx;
using UnityEngine;

namespace UniRxExtensions.AssetBundles
{
#if USE_FIXED_COMPILER
    // ReSharper disable once InconsistentNaming
    public static class _IObservable_AssetBundleCreateRequestProgressEvent_UnloadFinally
    {
        public static IObservable<AssetBundleCreateRequestProgressEvent> UnloadFinally(
            this IObservable<AssetBundleCreateRequestProgressEvent> source,
            bool unloadAllLoadedObjects
            )
        {
            if (source == null)
                throw new ArgumentNullException("source");

            AssetBundle createdAssetBundle = null;

            return source
                .Do(
                    x =>
                    {
                        if (x.IsDone)
                            createdAssetBundle = x.CreatedAssetBundle;
                    }
                )
                .Finally(
                    () =>
                    {
                        if (createdAssetBundle != null)
                            createdAssetBundle.Unload(unloadAllLoadedObjects: unloadAllLoadedObjects);
                    }
                );
        }
    }
#endif
}
