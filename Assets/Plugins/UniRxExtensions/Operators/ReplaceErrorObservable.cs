using System;
using UniRx;
using UniRx.Operators;

namespace UniRxExtensions.Operators
{
    internal class ReplaceErrorObservable<T, TSourceException, TDestinationException> :
        OperatorObservableBase<T>
        where TSourceException : Exception
        where TDestinationException : Exception
    {
        private readonly IObservable<T> source;
        private readonly Func<TSourceException, TDestinationException> exceptionReplacer;
//        private readonly Action<T> onNext;
//        private readonly Action<Exception> onError;
//        private readonly Action onCompleted;

        public ReplaceErrorObservable(
            IObservable<T> source,
            Func<TSourceException, TDestinationException> exceptionReplacer/*,
            Action<T> onNext,
            Action<Exception> onError,
            Action onCompleted*/
            )
            : base(source.IsRequiredSubscribeOnCurrentThread())
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (exceptionReplacer == null)
                throw new ArgumentNullException("exceptionReplacer");
//            if (onNext == null)
//                throw new ArgumentNullException("onNext");
//            if (onError == null)
//                throw new ArgumentNullException("onError");
//            if (onCompleted == null)
//                throw new ArgumentNullException("onCompleted");

            this.source = source;
            this.exceptionReplacer = exceptionReplacer;
//            this.onNext = onNext;
//            this.onError = onError;
//            this.onCompleted = onCompleted;
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            return new ReplaceError(this, observer, cancel).Run();
        }

        private Exception ReplaceExceptionIfMatchType(Exception sourceException)
        {
            if (sourceException is TSourceException)
                return exceptionReplacer((TSourceException) sourceException);

            return sourceException;
        }

        private class ReplaceError :
            OperatorObserverBase<T, T>
        {
            private readonly ReplaceErrorObservable<T, TSourceException, TDestinationException> parent;

            public ReplaceError(
                ReplaceErrorObservable<T, TSourceException, TDestinationException> parent,
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

//            public override void OnNext(T value)
//            {
//                try
//                {
//                    parent.onNext(value);
//                }
//                catch (Exception ex)
//                {
//                    try
//                    {
//                        observer.OnError(parent.ReplaceExceptionIfMatchType(ex));
//                    }
//                    finally
//                    {
//                        Dispose();
//                    }
//
//                    return;
//                }
//
//                observer.OnNext(value);
//            }

            public override void OnNext(T value)
            {
                observer.OnNext(value);
            }

            public override void OnError(Exception error)
            {
//                try
//                {
//                    parent.onError(parent.ReplaceExceptionIfMatchType(error));
//                }
//                catch (Exception ex)
//                {
//                    try
//                    {
//                        observer.OnError(ex);
//                    }
//                    finally
//                    {
//                        Dispose();
//                    }
//
//                    return;
//                }

                try
                {
                    observer.OnError(parent.ReplaceExceptionIfMatchType(error));
                }
                finally
                {
                    Dispose();
                }
            }

//            public override void OnCompleted()
//            {
//                try
//                {
//                    parent.onCompleted();
//                }
//                catch (Exception ex)
//                {
//                    observer.OnError(parent.ReplaceExceptionIfMatchType(ex));
//                    Dispose();
//                    return;
//                }
//
//                try
//                {
//                    observer.OnCompleted();
//                }
//                finally
//                {
//                    Dispose();
//                }
//            }

            public override void OnCompleted()
            {
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
