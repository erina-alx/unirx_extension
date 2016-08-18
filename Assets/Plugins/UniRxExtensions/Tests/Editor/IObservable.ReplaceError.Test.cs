using System;
using NUnit.Framework;
using UniRx;
using UniRxExtensions.Tests.Utils;

namespace UniRxExtensions.Tests
{
    // ReSharper disable once InconsistentNaming
    [TestFixture]
    internal class IObservable_ReplaceError_Test
    {
        [Test]
        public void Test_NoError()
        {
            var next = false;
            var error = false;
            var completed = false;

            TestStream.Make(Unit.Default)
                .ReplaceError((SourceException e) => new DestinationException())
                .Subscribe(_ => next = true, _ => error = true, () => completed = true);

            Assert.True(next);
            Assert.False(error);
            Assert.True(completed);
        }

        [Test]
        public void Test_ReplaceError()
        {
            var next = false;
            Exception error = null;
            var completed = false;

            TestStream.MakeWithError(() => new SourceException(), Unit.Default)
                .DoOnError(x => Assert.That(x, Is.TypeOf<SourceException>()))
                .ReplaceError((SourceException e) => new DestinationException())
                .DoOnError(x => Assert.That(x, Is.TypeOf<DestinationException>()))
                .Subscribe(_ => next = true, x => error = x, () => completed = true);

            Assert.True(next);
            Assert.That(error, Is.TypeOf<DestinationException>());
            Assert.False(completed);
        }

        [Test]
        public void Test_NotReplaceError()
        {
            var next = false;
            Exception error = null;
            var completed = false;

            TestStream.MakeWithError(() => new FakeException(), Unit.Default)
                .DoOnError(x => Assert.That(x, Is.TypeOf<FakeException>()))
                .ReplaceError((SourceException e) => new DestinationException())
                .DoOnError(x => Assert.That(x, Is.TypeOf<FakeException>()))
                .Subscribe(_ => next = true, x => error = x, () => completed = true);

            Assert.True(next);
            Assert.That(error, Is.Not.TypeOf<DestinationException>());
            Assert.That(error, Is.TypeOf<FakeException>());
            Assert.False(completed);
        }

        private class SourceException :
            Exception
        {
        }

        private class DestinationException :
            Exception
        {
        }

        private class FakeException :
            Exception
        {
        }
    }
}
