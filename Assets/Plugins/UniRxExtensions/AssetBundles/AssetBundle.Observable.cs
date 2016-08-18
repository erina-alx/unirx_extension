using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniRxExtensions.AssetBundles
{
#if USE_FIXED_COMPILER
    // ReSharper disable once InconsistentNaming
    public static class AssetBundle_Observable
    {
        public static IObservable<AssetBundleRequestProgressEvent<Object>> LoadAllAssetsAsyncAsObservable(
            this AssetBundle assetBundle
            )
        {
            if (ReferenceEquals(assetBundle, null))
                throw new ArgumentNullException("assetBundle");

            return Observable.Defer(
                () => assetBundle.LoadAllAssetsAsync().AsObservable()
                    .Select(x => ConvertEventForAllAssetLoading(x))
                );
        }

        public static IObservable<AssetBundleRequestProgressEvent<Object>> LoadAllAssetsAsyncAsObservable(
            this AssetBundle assetBundle,
            Type type
            )
        {
            if (ReferenceEquals(assetBundle, null))
                throw new ArgumentNullException("assetBundle");

            return Observable.Defer(
                () => assetBundle.LoadAllAssetsAsync(type).AsObservable()
                    .ReplaceError(
                        (AssetBundleRequestFailureException e) =>
                            new AssetBundleRequestFailureException(
                                String.Format("Failed to laod assets. (type: {0})", type.Name),
                                e
                                )
                    )
                    .Select(x => ConvertEventForAllAssetLoading(x))
                );
        }

#if USE_FIXED_COMPILER
        public static IObservable<AssetBundleRequestProgressEvent<T>> LoadAllAssetsAsyncAsObservable<T>(
            this AssetBundle assetBundle
            )
            where T : Object
        {
            if (ReferenceEquals(assetBundle, null))
                throw new ArgumentNullException("assetBundle");

            return Observable.Defer(
                () => assetBundle.LoadAllAssetsAsync<T>().AsObservable<T>()
                    .ReplaceError(
                        (AssetBundleRequestFailureException e) =>
                            new AssetBundleRequestFailureException(
                                String.Format("Failed to laod assets. (type: {0})", typeof(T).Name),
                                e
                                )
                    )
                    .Select(x => ConvertEventForAllAssetLoading(x))
                );
        }
#endif

        private static AssetBundleRequestProgressEvent<T> ConvertEventForAllAssetLoading<T>(
            AssetBundleRequestProgressEvent<T> @event
            )
            where T : Object
        {
            if (!@event.IsDone)
                return @event;

            if (!@event.ContainsAsset)
                return @event;

            var assets = new List<T>(@event.NumberOfAssets);

            if (@event.ContainsMainAsset && @event.ContainsSubAsset)
                if (!ReferenceEquals(@event.MainAsset, @event.SubAssets[0]))
                    assets.Add(@event.MainAsset);

            assets.AddRange(@event.SubAssets);

            return new AssetBundleRequestProgressEvent<T>(new ReadOnlyCollection<T>(assets));
        }

        public static IObservable<AssetBundleRequestProgressEvent<Object>> LoadAssetAsyncAsObservable(
            this AssetBundle assetBundle,
            string name
            )
        {
            if (ReferenceEquals(assetBundle, null))
                throw new ArgumentNullException("assetBundle");
            if (name == null)
                throw new ArgumentNullException("name");

            return Observable.Defer(
                () => assetBundle.LoadAssetAsync(name)
                    .AsObservable()
                    .ReplaceError(
                        (
                            AssetBundleRequestFailureException e) =>
                            new AssetBundleRequestFailureException(
                                String.Format("Failed to load asset. (name: \"{0}\")", name),
                                e
                                )
                    )
                    .Do(
                        x =>
                        {
                            if (x.IsDone && !x.ContainsAsset)
                                throw new AssetBundleRequestFailureException(
                                    String.Format("Failed to load asset. (name: \"{0}\")", name)
                                    );
                        }
                    )
                    .Select(x => ConvertEventForSingleAssetLoading(x))
                );
        }

        public static IObservable<AssetBundleRequestProgressEvent<Object>> LoadAssetAsyncAsObservable(
            this AssetBundle assetBundle,
            string name,
            Type type
            )
        {
            if (ReferenceEquals(assetBundle, null))
                throw new ArgumentNullException("assetBundle");
            if (name == null)
                throw new ArgumentNullException("name");
            if (type == null)
                throw new ArgumentNullException("type");

            return Observable.Defer(
                () => assetBundle.LoadAssetAsync(name, type)
                    .AsObservable()
                    .ReplaceError(
                        (
                            AssetBundleRequestFailureException e) =>
                            new AssetBundleRequestFailureException(
                                String.Format("Failed to load asset. (type: {1}, name: \"{0}\")", name, type.Name),
                                e
                                )
                    )
                    .Do(
                        x =>
                        {
                            if (x.IsDone && !x.ContainsAsset)
                                throw new AssetBundleRequestFailureException(
                                    String.Format("Failed to load asset. (type: {1}, name: \"{0}\")", name, type.Name)
                                    );
                        }
                    )
                    .Select(x => ConvertEventForSingleAssetLoading(x))
                );
        }

#if USE_FIXED_COMPILER
        public static IObservable<AssetBundleRequestProgressEvent<T>> LoadAssetAsyncAsObservable<T>(
            this AssetBundle assetBundle,
            string name
            )
            where T : Object
        {
            if (ReferenceEquals(assetBundle, null))
                throw new ArgumentNullException("assetBundle");
            if (name == null)
                throw new ArgumentNullException("name");

            return Observable.Defer(
                () => assetBundle.LoadAssetAsync<T>(name)
                    .AsObservable<T>()
                    .ReplaceError(
                        (
                            AssetBundleRequestFailureException e) =>
                            new AssetBundleRequestFailureException(
                                String.Format("Failed to load asset. (type: {1}, name: \"{0}\")", name, typeof(T).Name),
                                e
                                )
                    )
                    .Do(
                        x =>
                        {
                            if (x.IsDone && !x.ContainsAsset)
                                throw new AssetBundleRequestFailureException(
                                    String.Format("Failed to load asset. (type: {1}, name: \"{0}\")", name, typeof(T).Name)
                                    );
                        }
                    )
                    .Select(x => ConvertEventForSingleAssetLoading(x))
                );
        }
#endif

        private static AssetBundleRequestProgressEvent<T> ConvertEventForSingleAssetLoading<T>(
            AssetBundleRequestProgressEvent<T> @event
            )
            where T : Object
        {
            if (!@event.IsDone)
                return @event;

            if (!@event.ContainsMainAsset)
                throw new ArgumentException();

            var subAssets = new List<T>(@event.NumberOfSubAssets);

            for (var i = 0; i < @event.SubAssets.Count; i++)
                if (!ReferenceEquals(@event.SubAssets[i], @event.MainAsset))
                    subAssets.Add(@event.SubAssets[i]);

            return new AssetBundleRequestProgressEvent<T>(
                @event.MainAsset,
                new ReadOnlyCollection<T>(subAssets)
                );
        }

        public static IObservable<AssetBundleRequestProgressEvent<Object>> LoadAssetWithSubAssetsAsyncAsObservable(
            this AssetBundle assetBundle,
            string name
            )
        {
            if (ReferenceEquals(assetBundle, null))
                throw new ArgumentNullException("assetBundle");
            if (name == null)
                throw new ArgumentNullException("name");

            return Observable.Defer(
                () => assetBundle.LoadAssetWithSubAssetsAsync(name)
                    .AsObservable()
                    .ReplaceError(
                        (
                            AssetBundleRequestFailureException e) =>
                            new AssetBundleRequestFailureException(
                                String.Format("Failed to load asset with sub assets. (name: \"{0}\")", name),
                                e
                                )
                    )
                    .Do(
                        x =>
                        {
                            if (x.IsDone && !x.ContainsAsset)
                                throw new AssetBundleRequestFailureException(
                                    String.Format("Failed to load asset with sub assets. (name: \"{0}\")", name)
                                    );
                        }
                    )
                    .Select(x => ConvertEventForSingleAssetLoading(x))
                );
        }

        public static IObservable<AssetBundleRequestProgressEvent<Object>> LoadAssetWithSubAssetsAsyncAsObservable(
            this AssetBundle assetBundle,
            string name,
            Type type
            )
        {
            if (ReferenceEquals(assetBundle, null))
                throw new ArgumentNullException("assetBundle");
            if (name == null)
                throw new ArgumentNullException("name");
            if (type == null)
                throw new ArgumentNullException("type");

            return Observable.Defer(
                () => assetBundle.LoadAssetWithSubAssetsAsync(name, type)
                    .AsObservable()
                    .ReplaceError(
                        (
                            AssetBundleRequestFailureException e) =>
                            new AssetBundleRequestFailureException(
                                String.Format("Failed to load asset with sub assets. (type: {1}, name: \"{0}\")", name, type.Name),
                                e
                                )
                    )
                    .Do(
                        x =>
                        {
                            if (x.IsDone && !x.ContainsAsset)
                                throw new AssetBundleRequestFailureException(
                                    String.Format("Failed to load asset with sub assets. (type: {1}, name: \"{0}\")", name, type.Name)
                                    );
                        }
                    )
                    .Select(x => ConvertEventForSingleAssetLoading(x))
                );
        }

#if USE_FIXED_COMPILER
        public static IObservable<AssetBundleRequestProgressEvent<T>> LoadAssetWithSubAssetsAsyncAsObservable<T>(
            this AssetBundle assetBundle,
            string name
            )
            where T : Object
        {
            if (ReferenceEquals(assetBundle, null))
                throw new ArgumentNullException("assetBundle");
            if (name == null)
                throw new ArgumentNullException("name");

            return Observable.Defer(
                () => assetBundle.LoadAssetWithSubAssetsAsync<T>(name)
                    .AsObservable<T>()
                    .ReplaceError(
                        (
                            AssetBundleRequestFailureException e) =>
                            new AssetBundleRequestFailureException(
                                String.Format(
                                    "Failed to load asset with sub assets. (type: {1}, name: \"{0}\")",
                                    name,
                                    typeof(T).Name
                                    ),
                                e
                                )
                    )
                    .Do(
                        x =>
                        {
                            if (x.IsDone && !x.ContainsAsset)
                                throw new AssetBundleRequestFailureException(
                                    String.Format("Failed to load asset with sub assets. (type: {1}, name: \"{0}\")", name, typeof(T).Name)
                                    );
                        }
                    )
                    .Select(x => ConvertEventForSingleAssetLoading(x))
                );
        }
#endif
    }
#endif
}
