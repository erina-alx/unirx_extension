using System;
using UniRx;
using Object = UnityEngine.Object;

namespace UniRxExtensions.Resources
{
    public static class ObservableResources
    {
        public static IObservable<ResourceRequestProgressEvent<TAsset>> LoadAsync<TAsset>(string resourcePath)
            where TAsset : Object
        {
            if (resourcePath == null)
                throw new ArgumentNullException("resourcePath");

            return Observable.Create<ResourceRequestProgressEvent<TAsset>>(
                observer =>
                    UnityEngine.Resources.LoadAsync<TAsset>(resourcePath)
                        .AsObservable()
                        .SubscribeWithState2(
                            observer,
                            resourcePath,
                            (x, o, _) => o.OnNext(
                                x.IsDone
                                    ? new ResourceRequestProgressEvent<TAsset>((TAsset) x.Asset)
                                    : new ResourceRequestProgressEvent<TAsset>(x.Progress)
                                ),
                            (error, o, rp) => o.OnError(
                                new ResourceRequestFailureException(
                                    String.Format("Failed to load resource \"{0}\". (type: {1})", rp, typeof(TAsset).Name),
                                    error
                                    )
                                ),
                            (o, _) => o.OnCompleted()
                        )
                );
        }

        public static IObservable<ResourceRequestProgressEvent> LoadAsync(string resourcePath, Type type)
        {
            if (resourcePath == null)
                throw new ArgumentNullException("resourcePath");
            if (type == null)
                throw new ArgumentNullException("type");

            return Observable.Create<ResourceRequestProgressEvent>(
                observer =>
                    UnityEngine.Resources.LoadAsync(resourcePath, type)
                        .AsObservable()
                        .SubscribeWithState3(
                            observer,
                            resourcePath,
                            type,
                            (x, o, _, __) => o.OnNext(x),
                            (error, o, rp, t) => o.OnError(
                                new ResourceRequestFailureException(
                                    String.Format("Failed to load resource \"{0}\". (type: {1})", rp, t.Name),
                                    error
                                    )
                                ),
                            (o, _, __) => o.OnCompleted()
                        )
                );
        }

        public static IObservable<ResourceRequestProgressEvent> LoadAsync(string resourcePath)
        {
            if (resourcePath == null)
                throw new ArgumentNullException("resourcePath");

            return Observable.Create<ResourceRequestProgressEvent>(
                observer =>
                    UnityEngine.Resources.LoadAsync(resourcePath)
                        .AsObservable()
                        .SubscribeWithState2(
                            observer,
                            resourcePath,
                            (x, o, _) => o.OnNext(x),
                            (error, o, rp) => o.OnError(
                                new ResourceRequestFailureException(
                                    String.Format("Failed to load resource \"{0}\".", rp),
                                    error
                                    )
                                ),
                            (o, _) => o.OnCompleted()
                        )
                );
        }

        public static IObservable<Unit> UnloadUnusedAssetsAsync(IProgress<float> progress = null)
        {
            return Observable.Defer(
                () => UnityEngine.Resources.UnloadUnusedAssets()
                    .AsAsyncOperationObservable(progress)
                    .AsUnitObservable()
                );
        }
    }
}
