using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace UniRxExtensions.AssetBundles
{
#if USE_FIXED_COMPILER
    public static class ObservableAssetBundle
    {
        public static IObservable<AssetBundleCreateRequestProgressEvent> LoadFromFileAsync(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            return Observable.Defer(
                () => AssetBundle.LoadFromFileAsync(path)
                    .AsObservable()
                    .ReplaceError(
                        (AssetBundleCreateRequestFailureException e) =>
                            new AssetBundleCreateRequestFailureException(
                                String.Format("Failed to load asset bundle from file. (path: \"{0}\")", path),
                                e
                                )
                    )
                );
        }

        public static IObservable<AssetBundleCreateRequestProgressEvent> LoadFromFileAsync(string path, uint crc)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            return Observable.Defer(
                () => AssetBundle.LoadFromFileAsync(path, crc)
                    .AsObservable()
                    .ReplaceError(
                        (AssetBundleCreateRequestFailureException e) =>
                            new AssetBundleCreateRequestFailureException(
                                String.Format("Failed to load asset bundle from file. (crc: {1}, path: \"{0}\")", path, crc),
                                e
                                )
                    )
                );
        }

        public static IObservable<AssetBundleCreateRequestProgressEvent> LoadFromFileAsync(
            string path,
            uint crc,
            ulong offset
            )
        {
            if (path == null)
                throw new ArgumentNullException("path");

            return Observable.Defer(
                () => AssetBundle.LoadFromFileAsync(path, crc, offset)
                    .AsObservable()
                    .ReplaceError(
                        (AssetBundleCreateRequestFailureException e) =>
                            new AssetBundleCreateRequestFailureException(
                                String.Format(
                                    "Failed to load asset bundle from file. (crc: {1}, offset: {2}, path: \"{0}\")",
                                    path,
                                    crc,
                                    offset
                                    ),
                                e
                                )
                    )
                );
        }

        public static IObservable<AssetBundleCreateRequestProgressEvent> LoadFromMemoryAsync(
            byte[] binary,
            bool copyBinary = false
            )
        {
            if (binary == null)
                throw new ArgumentNullException("binary");

            return LoadFromMemoryAsync_Core(copyBinary ? binary.ToArray() : binary);
        }

        private static IObservable<AssetBundleCreateRequestProgressEvent> LoadFromMemoryAsync_Core(byte[] binary)
        {
            return Observable.Defer(() => AssetBundle.LoadFromMemoryAsync(binary).AsObservable());
        }

        public static IObservable<AssetBundleCreateRequestProgressEvent> LoadFromMemoryAsync(
            byte[] binary,
            uint crc,
            bool copyBinary = false
            )
        {
            if (binary == null)
                throw new ArgumentNullException("binary");

            return LoadFromMemoryAsync_Core(copyBinary ? binary.ToArray() : binary, crc);
        }

        private static IObservable<AssetBundleCreateRequestProgressEvent> LoadFromMemoryAsync_Core(
            byte[] binary,
            uint crc
            )
        {
            return Observable.Defer(() => AssetBundle.LoadFromMemoryAsync(binary, crc).AsObservable());
        }
    }
#endif
}
