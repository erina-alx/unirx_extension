using System;
using System.Collections.ObjectModel;
using UniRx;
using UniRxExtensions.Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniRxExtensions.AssetBundles
{
#if USE_FIXED_COMPILER
    public static class AssetBundleRequestAsObservable
    {
        public static IObservable<AssetBundleRequestProgressEvent<Object>> AsObservable(
            this AssetBundleRequest request
            )
        {
            if (request == null)
                throw new ArgumentNullException("request");

            return request.AsObservable<Object>();
        }

        public static IObservable<AssetBundleRequestProgressEvent<TObject>> AsObservable<TObject>(
            this AssetBundleRequest request
            )
            where TObject : Object
        {
            if (request == null)
                throw new ArgumentNullException("request");

            return Observable.Create<AssetBundleRequestProgressEvent<TObject>>(
                observer =>
                {
                    if (NotifyToObserverByRequest(observer, request))
                        return DummyDisposable.Instance;

                    var compositeDisposable = new CompositeDisposable();

                    Observable.EveryEndOfFrame()
                        .SubscribeWithState3(
                            observer,
                            request,
                            compositeDisposable,
                            (_, o, r, d) =>
                            {
                                if (!d.IsDisposed)
                                    if (NotifyToObserverByRequest(o, r))
                                        d.Dispose();
                            }
                        )
                        .AddTo(compositeDisposable);

                    return compositeDisposable;
                }
                );
        }

        private static bool NotifyToObserverByRequest<TObject>(
            IObserver<AssetBundleRequestProgressEvent<TObject>> observer,
            AssetBundleRequest request
            )
            where TObject : Object
        {
            if (request.isDone)
            {
                if (ReferenceEquals(request.asset, null) && request.allAssets == null)
                {
                    observer.OnError(new AssetBundleRequestFailureException());
                }
                else
                {
                    if (!ReferenceEquals(request.asset, null) && request.allAssets != null)
                        observer.OnNext(
                            new AssetBundleRequestProgressEvent<TObject>(
                                (TObject) request.asset,
                                ConvertToReadOnlyCollection<TObject>(request.allAssets)
                                )
                            );
                    else if (!ReferenceEquals(request.asset, null))
                        observer.OnNext(
                            new AssetBundleRequestProgressEvent<TObject>(
                                (TObject) request.asset
                                )
                            );
                    else
                        observer.OnNext(
                            new AssetBundleRequestProgressEvent<TObject>(
                                ConvertToReadOnlyCollection<TObject>(request.allAssets)
                                )
                            );

                    observer.OnCompleted();
                }

                return true;
            }

            observer.OnNext(new AssetBundleRequestProgressEvent<TObject>(request.progress));

            return false;
        }

        private static ReadOnlyCollection<TObject> ConvertToReadOnlyCollection<TObject>(Object[] sourceArray)
        {
            var array = new TObject[sourceArray.Length];
            sourceArray.CopyTo(array, 0);
            return new ReadOnlyCollection<TObject>(array);
        }
    }
#endif
}
