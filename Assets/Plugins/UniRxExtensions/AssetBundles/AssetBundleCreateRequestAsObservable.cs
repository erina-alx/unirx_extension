using System;
using UniRx;
using UniRxExtensions.Core;
using UnityEngine;

namespace UniRxExtensions.AssetBundles
{
    public static class AssetBundleCreateRequestAsObservable
    {
        public static IObservable<AssetBundleCreateRequestProgressEvent> AsObservable(
            this AssetBundleCreateRequest request
            )
        {
            if (request == null)
                throw new ArgumentNullException("request");

            return Observable.Create<AssetBundleCreateRequestProgressEvent>(
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

        private static bool NotifyToObserverByRequest(
            IObserver<AssetBundleCreateRequestProgressEvent> observer,
            AssetBundleCreateRequest request
            )
        {
            if (request.isDone)
            {
                if (ReferenceEquals(request.assetBundle, null))
                {
                    observer.OnError(new AssetBundleCreateRequestFailureException());
                }
                else
                {
                    observer.OnNext(new AssetBundleCreateRequestProgressEvent(request.assetBundle));
                    observer.OnCompleted();
                }

                return true;
            }

            observer.OnNext(new AssetBundleCreateRequestProgressEvent(request.progress));

            return false;
        }
    }
}
