using System;
using System.IO;
using System.Linq;
using UniRx;
using UniRxExtensions.Core;

namespace UniRxExtensions.Files
{
#if USE_FIXED_COMPILER
    public static class ObservableFile
    {
        public static IObservable<byte[]> ReadAsync(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException("filePath");

            return Observable.Create<byte[]>(
                observer =>
                {
                    var token = new DisposableToken();

                    try
                    {
                        var fs = new FileStream(filePath, FileMode.Open);

                        try
                        {
                            fs.ReadAsyncBySize((int) fs.Length, disposeFinnaly: true)
                                .SubscribeWithState3(
                                    observer,
                                    token,
                                    fs,
                                    (bytes, o, d, f) =>
                                    {
                                        if (!d.IsDisposed)
                                        {
                                            o.OnNext(bytes);
                                            o.OnCompleted();
                                        }

                                        f.Dispose();
                                    },
                                    (error, o, d, f) =>
                                    {
                                        if (!d.IsDisposed)
                                            o.OnError(error);

                                        f.Dispose();
                                    },
                                    (_, __, f) => f.Dispose()
                                );
                        }
                        catch (Exception e)
                        {
                            fs.Dispose();
                            observer.OnError(e);
                        }
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                    }

                    return token;
                }
                );
        }

        public static IObservable<Unit> WriteAsync(
            string filePath,
            FileMode fileMode,
            byte[] bytes,
            bool copyBytes = false
            )
        {
            if (filePath == null)
                throw new ArgumentNullException("filePath");
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            return WriteAsync_Core(filePath, fileMode, copyBytes ? bytes.ToArray() : bytes);
        }

        private static IObservable<Unit> WriteAsync_Core(
            string filePath,
            FileMode fileMode,
            byte[] bytes
            )
        {
            return Observable.Create<Unit>(
                observer =>
                {
                    var token = new DisposableToken();

                    try
                    {
                        var fs = new FileStream(filePath, fileMode);

                        try
                        {
                            fs.WriteAsync(bytes, disposeFinnaly: true)
                                .SubscribeWithState3(
                                    observer,
                                    token,
                                    fs,
                                    (_, o, d, f) =>
                                    {
                                        if (!d.IsDisposed)
                                        {
                                            o.OnNext(Unit.Default);
                                            o.OnCompleted();
                                        }

                                        f.Dispose();
                                    },
                                    (error, o, d, f) =>
                                    {
                                        if (!d.IsDisposed)
                                            o.OnError(error);

                                        f.Dispose();
                                    },
                                    (_, __, f) => f.Dispose()
                                );
                        }
                        catch (Exception e)
                        {
                            fs.Dispose();
                            observer.OnError(e);
                        }
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                    }

                    return token;
                }
                );
        }
    }
#endif
}
