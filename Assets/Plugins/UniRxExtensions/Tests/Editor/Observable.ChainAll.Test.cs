using System;
using NUnit.Framework;
using UniRx;

namespace UniRxExtensions.Tests
{
#if USE_FIXED_COMPILER
    // ReSharper disable once InconsistentNaming
    [TestFixture]
    internal class Observable_ChainAll_Test
    {
        [Test]
        public void Test()
        {
            var lastEvent = 0;
            var completed = false;
            Exception error = null;

            new[]
            {
                AsyncAction(1),
                AsyncAction(2),
                AsyncAction(3),
            }.ChainAll()
                .Subscribe(x => lastEvent = x, x => error = x, () => completed = true);

            var t = DateTime.Now;

            while (!completed && error == null)
            {
                if (error != null)
                    throw error;

                if ((DateTime.Now - t).Seconds > 0.5)
                    throw new Exception();
            }

            Assert.True(completed);
            Assert.That(lastEvent, Is.EqualTo(3));
        }

        [Test]
        public void Test_CollectEvents()
        {
            int[] events = null;
            var completed = false;
            Exception error = null;

            new[]
            {
                AsyncAction(1),
                AsyncAction(2),
                AsyncAction(3),
            }.ChainAllAndCollectAllEvents()
                .Subscribe(x => events = x, x => error = x, () => completed = true);

            var t = DateTime.Now;

            while (!completed && error == null)
            {
                if (error != null)
                    throw error;

                if ((DateTime.Now - t).Seconds > 0.5)
                    throw new Exception();
            }

            Assert.True(completed);
            Assert.That(events, Is.EqualTo(new[] {1, 2, 3}));
        }

        [Test]
        public void Test_WithFunc()
        {
            var lastEvent = 0;
            var completed = false;
            Exception error = null;

            new Func<IObservable<int>>[]
            {
                () => AsyncAction(1),
                () => AsyncAction(2),
                () => AsyncAction(3),
            }.ChainAll()
                .Subscribe(x => lastEvent = x, x => error = x, () => completed = true);

            var t = DateTime.Now;

            while (!completed && error == null)
            {
                if (error != null)
                    throw error;

                if ((DateTime.Now - t).Seconds > 0.5)
                    throw new Exception();
            }

            Assert.True(completed);
            Assert.That(lastEvent, Is.EqualTo(3));
        }

        [Test]
        public void Test_WithFunc_CollectEvents()
        {
            int[] events = null;
            var completed = false;
            Exception error = null;

            new Func<IObservable<int>>[]
            {
                () => AsyncAction(1),
                () => AsyncAction(2),
                () => AsyncAction(3),
            }.ChainAllAndCollectAllEvents()
                .Subscribe(x => events = x, x => error = x, () => completed = true);

            var t = DateTime.Now;

            while (!completed && error == null)
            {
                if (error != null)
                    throw error;

                if ((DateTime.Now - t).Seconds > 0.5)
                    throw new Exception();
            }

            Assert.True(completed);
            Assert.That(events, Is.EqualTo(new[] {1, 2, 3}));
        }

        private static IObservable<T> AsyncAction<T>(T reservedResult)
        {
            return Observable.ExecuteAsync(() => reservedResult, notifyAtMainThread: false);
        }
    }
#endif
}
