using System;
using NUnit.Framework;
using UniRx;
using UniRxExtensions.Tests.Utils;

namespace UniRxExtensions.Tests
{
    // ReSharper disable once InconsistentNaming
    [TestFixture]
    internal class IObservable_DoWithState_Test
    {
        private const int State1 = 123;
        private const string State2 = "abc";
        private const double State3 = 4.5;

        [TestFixture]
        internal class TestsFor1State
        {
            [Test]
            public void Test_OnNext()
            {
                var onNext = false;

                TestStream.Make(Unit.Default)
                    .DoWithState(
                        State1,
                        (_, s1) =>
                        {
                            Assert.False(onNext);
                            onNext = true;

                            AssertStates(s1);
                        }
                    )
                    .Subscribe();

                Assert.True(onNext);
            }

            [Test]
            public void Test_OnNext_And_OnError()
            {
                var onNext = false;
                var onError = false;

                TestStream.MakeWithError(() => new ExceptionForTest(), Unit.Default)
                    .DoWithState(
                        State1,
                        (_, s1) =>
                        {
                            Assert.False(onNext);
                            onNext = true;

                            AssertStates(s1);
                        },
                        (error, s1) =>
                        {
                            Assert.False(onError);
                            onError = true;

                            Assert.That(error, Is.TypeOf<ExceptionForTest>());
                            AssertStates(s1);
                        }
                    )
                    .CatchIgnore((ExceptionForTest _) => { })
                    .Subscribe();

                Assert.True(onNext);
                Assert.True(onError);
            }

            [Test]
            public void Test_OnNext_And_OnError_And_OnCompleted()
            {
                {
                    var onNext = false;
                    var onCompleted = false;

                    TestStream.Make(Unit.Default)
                        .DoWithState(
                            State1,
                            (_, s1) =>
                            {
                                Assert.False(onNext);
                                onNext = true;

                                AssertStates(s1);
                            },
                            (_, __) => Assert.Fail(),
                            s1 =>
                            {
                                Assert.False(onCompleted);
                                onCompleted = true;

                                AssertStates(s1);
                            }
                        )
                        .Subscribe();

                    Assert.True(onNext);
                    Assert.True(onCompleted);
                }

                {
                    var onNext = false;
                    var onError = false;

                    TestStream.MakeWithError(() => new ExceptionForTest(), Unit.Default)
                        .DoWithState(
                            State1,
                            (_, s1) =>
                            {
                                Assert.False(onNext);
                                onNext = true;

                                AssertStates(s1);
                            },
                            (error, s1) =>
                            {
                                Assert.False(onError);
                                onError = true;

                                Assert.That(error, Is.TypeOf<ExceptionForTest>());
                                AssertStates(s1);
                            },
                            _ => Assert.Fail()
                        )
                        .CatchIgnore((ExceptionForTest _) => { })
                        .Subscribe();

                    Assert.True(onNext);
                    Assert.True(onError);
                }
            }

            [Test]
            public void Test_OnNext_And_OnCompleted()
            {
                var onNext = false;
                var onCompleted = false;

                TestStream.Make(Unit.Default)
                    .DoWithState(
                        State1,
                        (_, s1) =>
                        {
                            Assert.False(onNext);
                            onNext = true;

                            AssertStates(s1);
                        },
                        s1 =>
                        {
                            Assert.False(onCompleted);
                            onCompleted = true;

                            AssertStates(s1);
                        }
                    )
                    .Subscribe();

                Assert.True(onNext);
                Assert.True(onCompleted);
            }
        }

        [TestFixture]
        internal class TestsFor2States
        {
            [Test]
            public void Test_OnNext()
            {
                var onNext = false;

                TestStream.Make(Unit.Default)
                    .DoWithState2(
                        State1,
                        State2,
                        (_, s1, s2) =>
                        {
                            Assert.False(onNext);
                            onNext = true;

                            AssertStates(s1, s2);
                        }
                    )
                    .Subscribe();

                Assert.True(onNext);
            }

            [Test]
            public void Test_OnNext_And_OnError()
            {
                var onNext = false;
                var onError = false;

                TestStream.MakeWithError(() => new ExceptionForTest(), Unit.Default)
                    .DoWithState2(
                        State1,
                        State2,
                        (_, s1, s2) =>
                        {
                            Assert.False(onNext);
                            onNext = true;

                            AssertStates(s1, s2);
                        },
                        (error, s1, s2) =>
                        {
                            Assert.False(onError);
                            onError = true;

                            Assert.That(error, Is.TypeOf<ExceptionForTest>());
                            AssertStates(s1, s2);
                        }
                    )
                    .CatchIgnore((ExceptionForTest _) => { })
                    .Subscribe();

                Assert.True(onNext);
                Assert.True(onError);
            }

            [Test]
            public void Test_OnNext_And_OnError_And_OnCompleted()
            {
                {
                    var onNext = false;
                    var onCompleted = false;

                    TestStream.Make(Unit.Default)
                        .DoWithState2(
                            State1,
                            State2,
                            (_, s1, s2) =>
                            {
                                Assert.False(onNext);
                                onNext = true;

                                AssertStates(s1, s2);
                            },
                            (_, __, ___) => Assert.Fail(),
                            (s1, s2) =>
                            {
                                Assert.False(onCompleted);
                                onCompleted = true;

                                AssertStates(s1, s2);
                            }
                        )
                        .Subscribe();

                    Assert.True(onNext);
                    Assert.True(onCompleted);
                }

                {
                    var onNext = false;
                    var onError = false;

                    TestStream.MakeWithError(() => new ExceptionForTest(), Unit.Default)
                        .DoWithState2(
                            State1,
                            State2,
                            (_, s1, s2) =>
                            {
                                Assert.False(onNext);
                                onNext = true;

                                AssertStates(s1, s2);
                            },
                            (error, s1, s2) =>
                            {
                                Assert.False(onError);
                                onError = true;

                                Assert.That(error, Is.TypeOf<ExceptionForTest>());
                                AssertStates(s1, s2);
                            },
                            (_, __) => Assert.Fail()
                        )
                        .CatchIgnore((ExceptionForTest _) => { })
                        .Subscribe();

                    Assert.True(onNext);
                    Assert.True(onError);
                }
            }

            [Test]
            public void Test_OnNext_And_OnCompleted()
            {
                var onNext = false;
                var onCompleted = false;

                TestStream.Make(Unit.Default)
                    .DoWithState2(
                        State1,
                        State2,
                        (_, s1, s2) =>
                        {
                            Assert.False(onNext);
                            onNext = true;

                            AssertStates(s1, s2);
                        },
                        (s1, s2) =>
                        {
                            Assert.False(onCompleted);
                            onCompleted = true;

                            AssertStates(s1, s2);
                        }
                    )
                    .Subscribe();

                Assert.True(onNext);
                Assert.True(onCompleted);
            }
        }

        [TestFixture]
        internal class TestsFor3States
        {
            [Test]
            public void Test_OnNext()
            {
                var onNext = false;

                TestStream.Make(Unit.Default)
                    .DoWithState3(
                        State1,
                        State2,
                        State3,
                        (_, s1, s2, s3) =>
                        {
                            Assert.False(onNext);
                            onNext = true;

                            AssertStates(s1, s2, s3);
                        }
                    )
                    .Subscribe();

                Assert.True(onNext);
            }

            [Test]
            public void Test_OnNext_And_OnError()
            {
                var onNext = false;
                var onError = false;

                TestStream.MakeWithError(() => new ExceptionForTest(), Unit.Default)
                    .DoWithState3(
                        State1,
                        State2,
                        State3,
                        (_, s1, s2, s3) =>
                        {
                            Assert.False(onNext);
                            onNext = true;

                            AssertStates(s1, s2, s3);
                        },
                        (error, s1, s2, s3) =>
                        {
                            Assert.False(onError);
                            onError = true;

                            Assert.That(error, Is.TypeOf<ExceptionForTest>());
                            AssertStates(s1, s2, s3);
                        }
                    )
                    .CatchIgnore((ExceptionForTest _) => { })
                    .Subscribe();

                Assert.True(onNext);
                Assert.True(onError);
            }

            [Test]
            public void Test_OnNext_And_OnError_And_OnCompleted()
            {
                {
                    var onNext = false;
                    var onCompleted = false;

                    TestStream.Make(Unit.Default)
                        .DoWithState3(
                            State1,
                            State2,
                            State3,
                            (_, s1, s2, s3) =>
                            {
                                Assert.False(onNext);
                                onNext = true;

                                AssertStates(s1, s2, s3);
                            },
                            (_, __, ___, ____) => Assert.Fail(),
                            (s1, s2, s3) =>
                            {
                                Assert.False(onCompleted);
                                onCompleted = true;

                                AssertStates(s1, s2, s3);
                            }
                        )
                        .Subscribe();

                    Assert.True(onNext);
                    Assert.True(onCompleted);
                }

                {
                    var onNext = false;
                    var onError = false;

                    TestStream.MakeWithError(() => new ExceptionForTest(), Unit.Default)
                        .DoWithState3(
                            State1,
                            State2,
                            State3,
                            (_, s1, s2, s3) =>
                            {
                                Assert.False(onNext);
                                onNext = true;

                                AssertStates(s1, s2, s3);
                            },
                            (error, s1, s2, s3) =>
                            {
                                Assert.False(onError);
                                onError = true;

                                Assert.That(error, Is.TypeOf<ExceptionForTest>());
                                AssertStates(s1, s2, s3);
                            },
                            (_, __, ___) => Assert.Fail()
                        )
                        .CatchIgnore((ExceptionForTest _) => { })
                        .Subscribe();

                    Assert.True(onNext);
                    Assert.True(onError);
                }
            }

            [Test]
            public void Test_OnNext_And_OnCompleted()
            {
                var onNext = false;
                var onCompleted = false;

                TestStream.Make(Unit.Default)
                    .DoWithState3(
                        State1,
                        State2,
                        State3,
                        (_, s1, s2, s3) =>
                        {
                            Assert.False(onNext);
                            onNext = true;

                            AssertStates(s1, s2, s3);
                        },
                        (s1, s2, s3) =>
                        {
                            Assert.False(onCompleted);
                            onCompleted = true;

                            AssertStates(s1, s2, s3);
                        }
                    )
                    .Subscribe();

                Assert.True(onNext);
                Assert.True(onCompleted);
            }
        }

        private static void AssertStates<T1>(T1 state1)
        {
            Assert.That(state1, Is.EqualTo(State1));
        }

        private static void AssertStates<T1, T2>(T1 state1, T2 state2)
        {
            AssertStates(state1);
            Assert.That(state2, Is.EqualTo(State2));
        }

        private static void AssertStates<T1, T2, T3>(T1 state1, T2 state2, T3 state3)
        {
            AssertStates(state1, state2);
            Assert.That(state3, Is.EqualTo(State3));
        }

        private class ExceptionForTest :
            Exception
        {
        }
    }
}
