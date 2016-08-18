using System;
using UniRx;
using UniRx.Operators;

namespace UniRxExtensions.Operators
{
    internal class DoWithStateObservable<T, TState> :
        OperatorObservableBase<T>
    {
        private readonly IObservable<T> source;
        private readonly TState state;
        private readonly Action<T, TState> onNext;
        private readonly Action<Exception, TState> onError;
        private readonly Action<TState> onCompleted;

        public DoWithStateObservable(
            IObservable<T> source,
            TState state,
            Action<T, TState> onNext,
            Action<Exception, TState> onError,
            Action<TState> onCompleted
            )
            : base(source.IsRequiredSubscribeOnCurrentThread())
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            this.source = source;
            this.state = state;
            this.onNext = onNext;
            this.onError = onError;
            this.onCompleted = onCompleted;
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            return new Do(this, observer, cancel).Run();
        }

        public TState State
        {
            get { return state; }
        }

        private class Do :
            OperatorObserverBase<T, T>
        {
            private readonly DoWithStateObservable<T, TState> parent;

            public Do(
                DoWithStateObservable<T, TState> parent,
                IObserver<T> observer,
                IDisposable cancel
                ) : base(observer, cancel)
            {
                this.parent = parent;
            }

            public IDisposable Run()
            {
                return parent.source.Subscribe(this);
            }

            public override void OnNext(T value)
            {
                try
                {
                    parent.onNext(value, parent.state);
                }
                catch (Exception ex)
                {
                    try
                    {
                        observer.OnError(ex);
                    }
                    finally
                    {
                        Dispose();
                    }

                    return;
                }

                observer.OnNext(value);
            }

            public override void OnError(Exception error)
            {
                try
                {
                    parent.onError(error, parent.state);
                }
                catch (Exception ex)
                {
                    try
                    {
                        observer.OnError(ex);
                    }
                    finally
                    {
                        Dispose();
                    }

                    return;
                }

                try
                {
                    observer.OnError(error);
                }
                finally
                {
                    Dispose();
                }
            }

            public override void OnCompleted()
            {
                try
                {
                    parent.onCompleted(parent.state);
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                    Dispose();
                    return;
                }

                try
                {
                    observer.OnCompleted();
                }
                finally
                {
                    Dispose();
                }
            }
        }
    }
}
