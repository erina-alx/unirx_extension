using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UniRx;

namespace UniRxExtensions.Tests
{
    // ReSharper disable once InconsistentNaming
    [TestFixture]
    internal class Stream_Observable_Test
    {
        public static readonly byte[] Bytes = {0x12, 0x45, 0x78};

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Test_ReadAsyncBySize(int size)
        {
            var stream = new MemoryStream(Bytes);
            byte[] readBytes = null;
            var completed = false;

            stream.ReadAsyncBySize(size, notifyAtMainThread: false, disposeFinnaly: true)
                .Subscribe(x => readBytes = x, () => completed = true);

            while (!completed)
            {
            }

            Assert.True(completed);
            Assert.That(readBytes.Length, Is.EqualTo(size));
            Assert.That(readBytes, Is.EqualTo(Bytes.Take(size).ToArray()));
        }

        [Test]
        [TestCase(0, 0, null)]
        [TestCase(0, 3, null)]
        [TestCase(0, 4, typeof(ArgumentException))]
        [TestCase(1, 2, null)]
        [TestCase(1, 3, typeof(ArgumentException))]
        [TestCase(2, 1, null)]
        [TestCase(2, 2, typeof(ArgumentException))]
        [TestCase(3, 0, null)]
        [TestCase(3, 1, typeof(ArgumentException))]
        [TestCase(4, 0, typeof(ArgumentException))]
        public void Test_ReadAsync(int offset, int count, Type typeOfExpectedException)
        {
            var stream = new MemoryStream(Bytes);
            var buffer = new byte[Bytes.Length];
            var readSize = 0;
            var completed = false;
            Exception error = null;

            var t = DateTime.Now;

            stream.ReadAsync(buffer, offset, count, notifyAtMainThread: false, disposeFinnaly: true)
                .Subscribe(x => readSize = x, x => error = x, () => completed = true);

            while (!completed && error == null)
            {
                if ((DateTime.Now - t).TotalSeconds > 0.5)
                    throw new Exception("timeout");
            }

            if (error != null)
            {
                if (typeOfExpectedException == null)
                    throw error;

                Assert.That(error, Is.TypeOf(typeOfExpectedException));
            }
            else
            {
                Assert.That(readSize, Is.EqualTo(count));
                Assert.True(completed);
                Assert.That(
                    buffer.Skip(offset).Take(count).ToArray(),
                    Is.EqualTo(Bytes.Take(count).ToArray())
                    );
            }
        }

        [Test]
        public void Test_WriteAsync_BufferOnly()
        {
            using (var stream = new MemoryStream())
            {
                var header = new byte[] {0xFF, 0xFE};

                var completed = false;
                Exception error = null;

                stream.Write(header, 0, header.Length);

                Assert.That(stream.Length, Is.EqualTo(header.Length));

                stream.Position = stream.Length;

                var t = DateTime.Now;

                stream.WriteAsync(Bytes, notifyAtMainThread: false)
                    .Subscribe(_ => { }, x => error = x, () => completed = true);

                while (!completed && error == null)
                {
                    if ((DateTime.Now - t).TotalSeconds > 0.5)
                        throw new Exception("timeout");
                }

                if (error != null)
                    throw error;

                Assert.True(completed);
                Assert.That(stream.Length, Is.EqualTo(header.Length + Bytes.Length));
                Assert.That(stream.Position, Is.EqualTo(stream.Length));

                stream.Position = 0;
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                Assert.That(buffer, Is.EqualTo(header.Concat(Bytes).ToArray()));
            }
        }

        [Test]
        [TestCase(0, 3)]
        [TestCase(1, 2)]
        public void Test_WriteAsync(int offset, int count)
        {
            using (var stream = new MemoryStream())
            {
                var header = new byte[] {0xFF, 0xFE};

                var completed = false;
                Exception error = null;

                stream.Write(header, 0, header.Length);

                Assert.That(stream.Length, Is.EqualTo(header.Length));

                stream.Position = stream.Length;

                var t = DateTime.Now;

                stream.WriteAsync(Bytes, offset, count, notifyAtMainThread: false)
                    .Subscribe(_ => { }, x => error = x, () => completed = true);

                while (!completed && error == null)
                {
                    if ((DateTime.Now - t).TotalSeconds > 0.5)
                        throw new Exception("timeout");
                }

                if (error != null)
                    throw error;

                Assert.True(completed);
                Assert.That(stream.Length, Is.EqualTo(header.Length + count));
                Assert.That(stream.Position, Is.EqualTo(stream.Length));

                stream.Position = 0;
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                Assert.That(buffer, Is.EqualTo(header.Concat(Bytes.Skip(offset).Take(count)).ToArray()));
            }
        }
    }
}
