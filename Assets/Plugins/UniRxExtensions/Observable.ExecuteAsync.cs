using System;
using UniRxExtensions.Core;

namespace UniRx
{
    public static partial class Observable
    {
        public static IObservable<Unit> ExecuteAsync(
            Action action,
            bool notifyAtMainThread = true
            )
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return ExecuteAsync(
                action.BeginInvoke,
                action.EndInvoke,
                notifyAtMainThread
                );
        }

        public static IObservable<TResult> ExecuteAsync<TResult>(
            Func<TResult> action,
            bool notifyAtMainThread = true
            )
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return ExecuteAsync(
                (x, y) =>
                {
                    // ReSharper disable once ConvertClosureToMethodGroup
                    return action.BeginInvoke(x, y);
                },
                x =>
                {
                    // ReSharper disable once ConvertClosureToMethodGroup
                    // ReSharper disable once ConvertToLambdaExpression
                    return action.EndInvoke(x);
                },
                notifyAtMainThread
                );
        }

        public static IObservable<Unit> ExecuteAsync(
            Func<AsyncCallback, object, IAsyncResult> methodToBegin,
            Action<IAsyncResult> methodToEnd,
            bool notifyAtMainThread = true
            )
        {
            if (methodToBegin == null)
                throw new ArgumentNullException("methodToBegin");
            if (methodToEnd == null)
                throw new ArgumentNullException("methodToEnd");

            return ExecuteAsync(
                methodToBegin,
                asyncResult =>
                {
                    methodToEnd(asyncResult);

                    return Unit.Default;
                },
                notifyAtMainThread
                );
        }

        public static IObservable<TResult> ExecuteAsync<TResult>(
            Func<AsyncCallback, object, IAsyncResult> methodToBegin,
            Func<IAsyncResult, TResult> methodToEnd,
            bool notifyAtMainThread = true
            )
        {
            if (methodToBegin == null)
                throw new ArgumentNullException("methodToBegin");
            if (methodToEnd == null)
                throw new ArgumentNullException("methodToEnd");

            return ExecuteAsync_Core(
                methodToBegin,
                methodToEnd,
                notifyAtMainThread ? (IWaiter) new MainThreadWaiter() : FakeWaiter.Instance
                );
        }

        private static IObservable<TResult> ExecuteAsync_Core<TResult>(
            Func<AsyncCallback, object, IAsyncResult> methodToBegin,
            Func<IAsyncResult, TResult> methodToEnd,
            IWaiter waiter
            )
        {
            if (waiter == null)
                throw new ArgumentNullException("waiter");

            MainThreadDispatcher.Initialize();

            return Create<TResult>(
                observer =>
                {
                    var token = new DisposableToken();

                    methodToBegin(
                        ar =>
                        {
                            var result = default(TResult);
                            Exception error = null;

                            try
                            {
                                result = methodToEnd(ar);
                            }
                            catch (Exception e)
                            {
                                error = e;
                            }

                            waiter.Execute(
                                () =>
                                {
                                    if (token.IsDisposed)
                                        return;

                                    if (error == null)
                                    {
                                        observer.OnNext(result);
                                        observer.OnCompleted();
                                    }
                                    else
                                    {
                                        observer.OnError(error);
                                    }
                                }
                                );
                        },
                        null
                        );

                    return token;
                }
                );
        }

        private interface IWaiter
        {
            void Execute(Action action);
        }

        private class FakeWaiter :
            IWaiter
        {
            public static readonly FakeWaiter Instance = new FakeWaiter();

            public void Execute(Action action)
            {
                action();
            }
        }

        private class MainThreadWaiter :
            IWaiter
        {
            public void Execute(Action action)
            {
                MainThreadDispatcher.Initialize();

                NextFrame(FrameCountType.EndOfFrame)
                    .SubscribeWithState(action, (_, a) => a());
            }
        }
    }
}
