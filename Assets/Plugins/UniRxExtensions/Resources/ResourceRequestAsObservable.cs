using System;
using UniRx;
using UniRxExtensions.Core;
using UnityEngine;

namespace UniRxExtensions.Resources
{
    public static class ResourceRequestAsObservable
    {
        public static IObservable<ResourceRequestProgressEvent> AsObservable(this ResourceRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            return Observable.Create<ResourceRequestProgressEvent>(
                observer =>
                {
                    if (request.isDone)
                    {
                        if (ReferenceEquals(request.asset, null))
                        {
                            observer.OnError(new ResourceRequestFailureException());
                        }
                        else
                        {
                            observer.OnNext(new ResourceRequestProgressEvent(request.asset));
                            observer.OnCompleted();
                        }

                        return DummyDisposable.Instance;
                    }

                    var compositeDisposable = new CompositeDisposable();

                    Observable.EveryEndOfFrame()
                        .SubscribeWithState3(
                            request,
                            observer,
                            compositeDisposable,
                            (
                                _,
                                // ReSharper disable once InconsistentNaming
                                _request,
                                // ReSharper disable once InconsistentNaming
                                _observer,
                                // ReSharper disable once InconsistentNaming
                                _disposable
                                ) =>
                            {
                                if (_request.isDone)
                                {
                                    if (ReferenceEquals(_request.asset, null))
                                    {
                                        if (!_disposable.IsDisposed)
                                            _observer.OnError(new ResourceRequestFailureException());
                                    }
                                    else
                                    {
                                        if (!_disposable.IsDisposed)
                                        {
                                            _observer.OnNext(new ResourceRequestProgressEvent(_request.asset));
                                            _observer.OnCompleted();
                                        }
                                    }

                                    _disposable.Dispose();
                                }
                                else
                                {
                                    if (!_disposable.IsDisposed)
                                        _observer.OnNext(new ResourceRequestProgressEvent(_request.progress));
                                }
                            }
                        )
                        .AddTo(compositeDisposable);

                    return compositeDisposable;
                }
                );
        }
    }
}
