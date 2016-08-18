using System;
using System.IO;
using UniRx;
using UnityEngine.Assertions;

namespace UniRxExtensions
{
    // ReSharper disable once InconsistentNaming
    public static class Stream_Observable
    {
        public static IObservable<byte[]> ReadAsyncBySize(
            this Stream stream,
            int size,
            bool notifyAtMainThread = true,
            bool disposeFinnaly = false
            )
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (size < 0)
                throw new ArgumentOutOfRangeException("size");

            var buffer = new byte[size];

            return stream.ReadAsync(
                buffer,
                0,
                buffer.Length,
                notifyAtMainThread: notifyAtMainThread,
                disposeFinnaly: disposeFinnaly
                )
                .Do(readSize => Assert.AreEqual(size, readSize))
                .Select(_ => buffer);
        }

        public static IObservable<int> ReadAsync(
            this Stream stream,
            byte[] buffer,
            int offset,
            int count,
            bool notifyAtMainThread = true,
            bool disposeFinnaly = false
            )
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            var s = Observable.ExecuteAsync(
                (x, y) => stream.BeginRead(buffer, offset, count, x, y),
                ar =>
                {
                    // ReSharper disable once ConvertToLambdaExpression
                    return stream.EndRead(ar);
                },
                notifyAtMainThread: notifyAtMainThread
                );

            if (disposeFinnaly)
                s = s.Finally(stream.Dispose);

            return s;
        }

        public static IObservable<Unit> WriteAsync(
            this Stream stream,
            byte[] buffer,
            bool notifyAtMainThread = true,
            bool disposeFinnaly = false
            )
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            return stream.WriteAsync(
                buffer,
                0,
                buffer.Length,
                notifyAtMainThread: notifyAtMainThread,
                disposeFinnaly: disposeFinnaly
                );
        }

        public static IObservable<Unit> WriteAsync(
            this Stream stream,
            byte[] buffer,
            int offset,
            int count,
            bool notifyAtMainThread = true,
            bool disposeFinnaly = false
            )
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            var s = Observable.ExecuteAsync(
                (x, y) => stream.BeginWrite(buffer, offset, count, x, y),
                stream.EndWrite,
                notifyAtMainThread: notifyAtMainThread
                );

            if (disposeFinnaly)
                s = s.Finally(stream.Dispose);

            return s;
        }
    }
}
