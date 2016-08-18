using System.IO;
using NUnit.Framework;
using UniRx;

namespace UniRxExtensions.Tests
{
    // ReSharper disable once InconsistentNaming
    [TestFixture]
    internal class Observable_ExecuteAsync_Test
    {
        [Test]
        public void Test_Action()
        {
            var next = false;
            var completed = false;

            Observable.ExecuteAsync(Task, false)
                .Subscribe(
                    _ => next = true,
                    () => completed = true
                );

            while (!completed)
            {
            }

            Assert.True(next);
            Assert.True(completed);
        }

        [Test]
        [TestCase(123)]
        public void Test_Func(int parameter)
        {
            var next = false;
            var completed = false;

            Observable.ExecuteAsync(() => TaskWithResult(parameter), false)
                .Subscribe(
                    x =>
                    {
                        Assert.That(x, Is.EqualTo(parameter));

                        Assert.False(next);
                        next = true;
                    },
                    () => completed = true
                );

            while (!completed)
            {
            }

            Assert.True(next);
            Assert.True(completed);
        }

        [Test]
        public void Test_StreamWriteAsync()
        {
            using (var stream = new MemoryStream())
            {
                var next = false;
                var completed = false;

                var bytes = new byte[] {0x12, 0x34, 0x56, 0x78};

                Observable.ExecuteAsync(
                    // ReSharper disable once AccessToDisposedClosure
                    (callback, state) => stream.BeginWrite(bytes, 0, bytes.Length, callback, state),
                    stream.EndWrite,
                    false
                    )
                    .SubscribeWithState(
                        stream,
                        (_, s) =>
                        {
                            Assert.That(s.Length, Is.EqualTo(4));
                            Assert.That(s.Position, Is.EqualTo(4));

                            var buffer = new byte[bytes.Length];
                            s.Position = 0;
                            s.Read(buffer, 0, buffer.Length);

                            Assert.That(buffer, Is.EqualTo(bytes));

                            Assert.False(next);
                            next = true;
                        },
                        _ => completed = true
                    );

                while (!completed)
                {
                }

                Assert.True(next);
                Assert.True(completed);
            }
        }

        [Test]
        public void Test_StreamReadAsync()
        {
            var bytes = new byte[] {0x12, 0x34, 0x56, 0x78};

            using (var stream = new MemoryStream(bytes))
            {
                var next = false;
                var completed = false;
                var buffer = new byte[bytes.Length];

                Observable.ExecuteAsync(
                    // ReSharper disable once AccessToDisposedClosure
                    (callback, state) => stream.BeginRead(buffer, 0, bytes.Length, callback, state),
                    ar =>
                    {
                        // ReSharper disable once AccessToDisposedClosure
                        // ReSharper disable once ConvertToLambdaExpression
                        return stream.EndRead(ar);
                    },
                    false
                    )
                    .Subscribe(
                        readSize =>
                        {
                            Assert.That(readSize, Is.EqualTo(bytes.Length));
                            Assert.That(buffer, Is.EqualTo(bytes));

                            Assert.False(next);
                            next = true;
                        },
                        () => completed = true
                    );

                while (!completed)
                {
                }

                Assert.True(next);
                Assert.True(completed);
            }
        }

        private static void Task()
        {
        }

        private static T TaskWithResult<T>(T result)
        {
            return result;
        }
    }
}
