using System;
using NUnit.Framework;
using UniRx;
using UniRxExtensions.Tests.Utils;

namespace UniRxExtensions.Tests
{
    // ReSharper disable once InconsistentNaming
    [TestFixture]
    internal class IObservable_DoWithoutEvent_Test
    {
        [Test]
        public void Test_OnNext()
        {
            var onNext = false;

            TestStream.Make(Unit.Default)
                .DoWithoutEvent(
                    () =>
                    {
                        Assert.False(onNext);
                        onNext = true;
                    }
                )
                .Subscribe();

            Assert.True(onNext);
        }

        [Test]
        public void Test_OnNext_And_OnError()
        {
            var step = 0;
            var onNext = false;
            Exception error = null;

            TestStream.MakeWithError(() => new ExceptionForTest(), Unit.Default)
                .DoWithoutEvent(
                    () =>
                    {
                        Assert.That(++step, Is.EqualTo(1));
                        onNext = true;
                    },
                    x =>
                    {
                        Assert.That(++step, Is.EqualTo(2));
                        error = x;
                    }
                )
                .CatchIgnore((ExceptionForTest _) => {})
                .Subscribe();

            Assert.True(onNext);
            Assert.That(error, Is.TypeOf<ExceptionForTest>());
        }

        [Test]
        public void Test_OnNext_And_OnError_And_OnCompleted()
        {
            {
                var step = 0;
                var onNext = false;
                var onCompleted = false;

                TestStream.Make(Unit.Default)
                    .DoWithoutEvent(
                        () =>
                        {
                            Assert.That(++step, Is.EqualTo(1));
                            onNext = true;
                        },
                        _ => Assert.Fail(),
                        () =>
                        {
                            Assert.That(++step, Is.EqualTo(2));
                            onCompleted = true;
                        }
                    )
                    .Subscribe();

                Assert.True(onNext);
                Assert.True(onCompleted);
            }

            {
                var step = 0;
                var onNext = false;
                Exception error = null;

                TestStream.MakeWithError(() => new ExceptionForTest(), Unit.Default)
                    .DoWithoutEvent(
                        () =>
                        {
                            Assert.That(++step, Is.EqualTo(1));
                            onNext = true;
                        },
                        x =>
                        {
                            Assert.That(++step, Is.EqualTo(2));
                            error = x;
                        },
                        Assert.Fail
                    )
                    .CatchIgnore((ExceptionForTest _) => { })
                    .Subscribe();

                Assert.True(onNext);
                Assert.That(error, Is.TypeOf<ExceptionForTest>());
            }
        }

        [Test]
        public void Test_OnNext_And_OnCompleted()
        {
            var step = 0;
            var onNext = false;
            var onCompleted = false;

            TestStream.Make(Unit.Default)
                .DoWithoutEvent(
                    () =>
                    {
                        Assert.That(++step, Is.EqualTo(1));
                        onNext = true;
                    },
                    _ => Assert.Fail(),
                    () =>
                    {
                        Assert.That(++step, Is.EqualTo(2));
                        onCompleted = true;
                    }
                )
                .Subscribe();

            Assert.True(onNext);
            Assert.True(onCompleted);
        }

        private class ExceptionForTest :
            Exception
        {
        }
    }
}
