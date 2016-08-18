using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRxExtensions.Files;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace UniRxExtensions.AssetBundles.Tests
{
#if UNITY_EDITOR
    internal class TestBehaviourForAssetBundle :
        MonoBehaviour
    {
#if USE_FIXED_COMPILER
        private Dictionary<string, Func<IObservable<object>>> contents;

        private Vector2 scrollPosition;

        private static string AssetBundleDirectoryPath
        {
            get { return Application.dataPath + "/Plugins/UniRxExtensions/AssetBundles/Tests/AssetBundles"; }
        }

        private static string TextAssetBundleFilePath
        {
            get { return AssetBundleDirectoryPath + "/ab_text"; }
        }

        private static string ImageAssetBundleFilePath
        {
            get { return AssetBundleDirectoryPath + "/ab_image"; }
        }

        private static string ImageMainAssetName
        {
            get { return "sample_image"; }
        }

        private static IObservable<AssetBundle> Test_LoadFromFile()
        {
            return ObservableAssetBundle.LoadFromFileAsync(TextAssetBundleFilePath)
                .SkipWhileInProgress()
                .UnloadFinally(unloadAllLoadedObjects: false)
                .Select(x => x.CreatedAssetBundle);
        }

        private static IObservable<Unit> Test_LoadFromFile_Error()
        {
            return ObservableAssetBundle.LoadFromFileAsync(TextAssetBundleFilePath + ".fake")
                .UnloadFinally(unloadAllLoadedObjects: false)
                .CatchIgnore((AssetBundleCreateRequestFailureException e) => Debug.Log("Caught expected error"))
                .AsUnitObservable();
        }

        private static IObservable<AssetBundle> Test_LoadFromMemory()
        {
            return ObservableFile.ReadAsync(TextAssetBundleFilePath)
                .SelectMany(bytes => ObservableAssetBundle.LoadFromMemoryAsync(bytes))
                .SkipWhileInProgress()
                .UnloadFinally(unloadAllLoadedObjects: false)
                .Select(x => x.CreatedAssetBundle);
        }

        private static IObservable<Unit> Test_LoadFromMemory_Error()
        {
            return Observable.Return(new byte[] {})
                .SelectMany(bytes => ObservableAssetBundle.LoadFromMemoryAsync(bytes))
                .UnloadFinally(unloadAllLoadedObjects: false)
                .CatchIgnore((AssetBundleCreateRequestFailureException e) => Debug.Log("Caught expected error"))
                .AsUnitObservable();
        }

        private static IObservable<AssetBundleRequestProgressEvent<Object>>
            Test_LoadAllAssetsAsyncAsObservable()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAllAssetsAsyncAsObservable()
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .SkipWhile(x => !x.IsDone)
                .Do(
                    x =>
                    {
                        Assert.IsNull(x.MainAsset);
                        Assert.AreEqual(0, x.NumberOfSubAssets);
                        Assert.AreEqual(5, x.AllAssets.Count);

                        Assert.AreEqual(typeof(Texture2D), x.AllAssets[0].GetType());
                        Assert.AreEqual(typeof(Sprite), x.AllAssets[1].GetType());
                        Assert.AreEqual(typeof(Sprite), x.AllAssets[2].GetType());
                        Assert.AreEqual(typeof(Sprite), x.AllAssets[3].GetType());
                        Assert.AreEqual(typeof(Sprite), x.AllAssets[4].GetType());
                    }
                );
        }

        private static IObservable<AssetBundleRequestProgressEvent<Object>>
            Test_LoadAllAssetsAsyncAsObservable_WithType_Texture2D()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAllAssetsAsyncAsObservable(typeof(Texture2D))
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .SkipWhileInProgress()
                .Do(
                    x =>
                    {
                        Assert.IsNull(x.MainAsset);
                        Assert.AreEqual(0, x.NumberOfSubAssets);
                        Assert.AreEqual(1, x.AllAssets.Count);

                        foreach (var asset in x.AllAssets)
                            Assert.AreEqual(typeof(Texture2D), asset.GetType());
                    }
                );
        }

        private static IObservable<AssetBundleRequestProgressEvent<Object>>
            Test_LoadAllAssetsAsyncAsObservable_WithType_Sprite()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAllAssetsAsyncAsObservable(typeof(Sprite))
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .SkipWhileInProgress()
                .Do(
                    x =>
                    {
                        Assert.IsNull(x.MainAsset);
                        Assert.AreEqual(0, x.NumberOfSubAssets);
                        Assert.AreEqual(4, x.AllAssets.Count);

                        foreach (var asset in x.AllAssets)
                            Assert.AreEqual(typeof(Sprite), asset.GetType());
                    }
                );
        }

        private static IObservable<AssetBundleRequestProgressEvent<Object>>
            Test_LoadAllAssetsAsyncAsObservable_WithType_None()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAllAssetsAsyncAsObservable(typeof(TextAsset))
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .SkipWhileInProgress()
                .Do(
                    x =>
                    {
                        Assert.AreEqual(0, x.NumberOfAssets);
                    }
                );
        }

#if USE_FIXED_COMPILER
        private static IObservable<AssetBundleRequestProgressEvent<Texture2D>>
            Test_LoadAllAssetsAsyncAsObservable_Generic_Texture2D()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAllAssetsAsyncAsObservable<Texture2D>()
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .SkipWhileInProgress()
                .Do(
                    x =>
                    {
                        Assert.IsNull(x.MainAsset);
                        Assert.AreEqual(0, x.NumberOfSubAssets);
                        Assert.AreEqual(1, x.AllAssets.Count);
                    }
                );
        }

        private static IObservable<AssetBundleRequestProgressEvent<Sprite>>
            Test_LoadAllAssetsAsyncAsObservable_Generic_Sprite()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAllAssetsAsyncAsObservable<Sprite>()
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .SkipWhileInProgress()
                .Do(
                    x =>
                    {
                        Assert.IsNull(x.MainAsset);
                        Assert.AreEqual(0, x.NumberOfSubAssets);
                        Assert.AreEqual(4, x.AllAssets.Count);
                    }
                );
        }

        private static IObservable<AssetBundleRequestProgressEvent<TextAsset>>
            Test_LoadAllAssetsAsyncAsObservable_Generic_None()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAllAssetsAsyncAsObservable<TextAsset>()
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .SkipWhileInProgress()
                .Do(
                    x =>
                    {
                        Assert.AreEqual(0, x.NumberOfAssets);
                    }
                );
        }
#endif

        private static IObservable<AssetBundleRequestProgressEvent<Object>>
            Test_LoadAssetAsyncAsObservable()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetAsyncAsObservable(ImageMainAssetName)
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .SkipWhileInProgress()
                .Do(
                    x =>
                    {
                        Assert.IsNotNull(x.MainAsset);

                        if (x.MainAsset != null)
                            Assert.AreEqual(typeof(Texture2D), x.MainAsset.GetType());

                        Assert.AreEqual(4, x.NumberOfSubAssets);
                        Assert.AreEqual(5, x.NumberOfAssets);

                        foreach (var asset in x.SubAssets)
                            Assert.AreEqual(typeof(Sprite), asset.GetType());
                    }
                );
        }

        private static IObservable<AssetBundleRequestProgressEvent<Object>>
            Test_LoadAssetAsyncAsObservable_Error()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetAsyncAsObservable(ImageMainAssetName + "_fake")
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .DoOnCompleted(() => { throw new Exception(); })
                .CatchIgnore((AssetBundleRequestFailureException _) => Debug.Log("Caught expected error."));
        }

        private static IObservable<AssetBundleRequestProgressEvent<Object>>
            Test_LoadAssetAsyncAsObservable_WithType_Texture2D()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetAsyncAsObservable(ImageMainAssetName, typeof(Texture2D))
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .SkipWhileInProgress()
                .Do(
                    x =>
                    {
                        Assert.IsNotNull(x.MainAsset);

                        if (x.MainAsset != null)
                            Assert.AreEqual(typeof(Texture2D), x.MainAsset.GetType());

                        Assert.AreEqual(0, x.NumberOfSubAssets);
                        Assert.AreEqual(1, x.NumberOfAssets);
                    }
                );
        }

        private static IObservable<AssetBundleRequestProgressEvent<Object>>
            Test_LoadAssetAsyncAsObservable_WithType_Sprite()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetAsyncAsObservable(ImageMainAssetName, typeof(Sprite))
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .SkipWhileInProgress()
                .Do(
                    x =>
                    {
                        Assert.IsNotNull(x.MainAsset);

                        if (x.MainAsset != null)
                            Assert.AreEqual(typeof(Sprite), x.MainAsset.GetType());

                        Assert.AreEqual(3, x.NumberOfSubAssets);
                        Assert.AreEqual(4, x.NumberOfAssets);
                    }
                );
        }

        private static IObservable<AssetBundleRequestProgressEvent<Object>>
            Test_LoadAssetAsyncAsObservable_WithType_NameError()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetAsyncAsObservable(ImageMainAssetName + "_fake", typeof(TextAsset))
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .DoOnCompleted(() => { throw new Exception(); })
                .SkipWhileInProgress()
                .CatchIgnore((AssetBundleRequestFailureException _) => Debug.Log("Caught expected error."));
        }

        private static IObservable<AssetBundleRequestProgressEvent<Object>>
            Test_LoadAssetAsyncAsObservable_WithType_TypeError()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetAsyncAsObservable(ImageMainAssetName, typeof(TextAsset))
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .DoOnCompleted(() => { throw new Exception(); })
                .SkipWhileInProgress()
                .CatchIgnore((AssetBundleRequestFailureException _) => Debug.Log("Caught expected error."));
        }

#if USE_FIXED_COMPILER
        private static IObservable<AssetBundleRequestProgressEvent<Texture2D>>
            Test_LoadAssetAsyncAsObservable_Generic_Texture2D()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetAsyncAsObservable<Texture2D>(ImageMainAssetName)
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .SkipWhileInProgress()
                .Do(
                    x =>
                    {
                        Assert.IsNotNull(x.MainAsset);
                        Assert.AreEqual(0, x.NumberOfSubAssets);
                        Assert.AreEqual(1, x.NumberOfAssets);
                    }
                );
        }

        private static IObservable<AssetBundleRequestProgressEvent<Sprite>>
            Test_LoadAssetAsyncAsObservable_Generic_Sprite()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetAsyncAsObservable<Sprite>(ImageMainAssetName)
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .SkipWhileInProgress()
                .Do(
                    x =>
                    {
                        Assert.IsNotNull(x.MainAsset);
                        Assert.AreEqual(3, x.NumberOfSubAssets);
                        Assert.AreEqual(4, x.NumberOfAssets);
                    }
                );
        }

        private static IObservable<AssetBundleRequestProgressEvent<Texture2D>>
            Test_LoadAssetAsyncAsObservable_Generic_NameError()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetAsyncAsObservable<Texture2D>(ImageMainAssetName + "_fake")
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .DoOnCompleted(() => { throw new Exception(); })
                .CatchIgnore((AssetBundleRequestFailureException _) => Debug.Log("Caught expected error."));
        }

        private static IObservable<AssetBundleRequestProgressEvent<TextAsset>>
            Test_LoadAssetAsyncAsObservable_Generic_TypeError()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetAsyncAsObservable<TextAsset>(ImageMainAssetName)
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .DoOnCompleted(() => { throw new Exception(); })
                .CatchIgnore((AssetBundleRequestFailureException _) => Debug.Log("Caught expected error."));
        }
#endif

        private static IObservable<AssetBundleRequestProgressEvent<Object>>
            Test_LoadAssetWithSubAssetsAsyncAsObservable()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetWithSubAssetsAsyncAsObservable(ImageMainAssetName)
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .SkipWhileInProgress()
                .Do(
                    x =>
                    {
                        Assert.IsNotNull(x.MainAsset);

                        if (x.MainAsset != null)
                            Assert.AreEqual(typeof(Texture2D), x.MainAsset.GetType());

                        Assert.AreEqual(4, x.NumberOfSubAssets);
                        Assert.AreEqual(5, x.NumberOfAssets);

                        foreach (var asset in x.SubAssets)
                            Assert.AreEqual(typeof(Sprite), asset.GetType());
                    }
                );
        }

        private static IObservable<AssetBundleRequestProgressEvent<Object>>
            Test_LoadAssetWithSubAssetsAsyncAsObservable_Error()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetWithSubAssetsAsyncAsObservable(ImageMainAssetName + "_fake")
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .DoOnCompleted(() => { throw new Exception(); })
                .CatchIgnore((AssetBundleRequestFailureException _) => Debug.Log("Caught expected error."));
        }

        private static IObservable<AssetBundleRequestProgressEvent<Object>>
            Test_LoadAssetWithSubAssetsAsyncAsObservable_WithType_Texture2D()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetWithSubAssetsAsyncAsObservable(
                        ImageMainAssetName,
                        typeof(Texture2D)
                        )
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .SkipWhileInProgress()
                .Do(
                    x =>
                    {
                        Assert.IsNotNull(x.MainAsset);

                        if (x.MainAsset != null)
                            Assert.AreEqual(typeof(Texture2D), x.MainAsset.GetType());

                        Assert.AreEqual(0, x.NumberOfSubAssets);
                        Assert.AreEqual(1, x.NumberOfAssets);
                    }
                );
        }

        private static IObservable<AssetBundleRequestProgressEvent<Object>>
            Test_LoadAssetWithSubAssetsAsyncAsObservable_WithType_Sprite()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetWithSubAssetsAsyncAsObservable(ImageMainAssetName, typeof(Sprite))
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .SkipWhileInProgress()
                .Do(
                    x =>
                    {
                        Assert.IsNotNull(x.MainAsset);

                        if (x.MainAsset != null)
                            Assert.AreEqual(typeof(Sprite), x.MainAsset.GetType());

                        Assert.AreEqual(3, x.NumberOfSubAssets);
                        Assert.AreEqual(4, x.NumberOfAssets);
                    }
                );
        }

        private static IObservable<AssetBundleRequestProgressEvent<Object>>
            Test_LoadAssetWithSubAssetsAsyncAsObservable_WithType_NameError()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetWithSubAssetsAsyncAsObservable(
                        ImageMainAssetName + "_fake",
                        typeof(Texture2D)
                        )
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .DoOnCompleted(() => { throw new Exception(); })
                .SkipWhileInProgress()
                .CatchIgnore((AssetBundleRequestFailureException _) => Debug.Log("Caught expected error."));
        }

        private static IObservable<AssetBundleRequestProgressEvent<Object>>
            Test_LoadAssetWithSubAssetsAsyncAsObservable_WithType_TypeError()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetWithSubAssetsAsyncAsObservable(
                        ImageMainAssetName,
                        typeof(TextAsset)
                        )
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .DoOnCompleted(() => { throw new Exception(); })
                .SkipWhileInProgress()
                .CatchIgnore((AssetBundleRequestFailureException _) => Debug.Log("Caught expected error."));
        }

#if USE_FIXED_COMPILER
        private static IObservable<AssetBundleRequestProgressEvent<Texture2D>>
            Test_LoadAssetWithSubAssetsAsyncAsObservable_Generic_Texture2D()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetWithSubAssetsAsyncAsObservable<Texture2D>(ImageMainAssetName)
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .SkipWhileInProgress()
                .Do(
                    x =>
                    {
                        Assert.IsNotNull(x.MainAsset);
                        Assert.AreEqual(0, x.NumberOfSubAssets);
                        Assert.AreEqual(1, x.NumberOfAssets);
                    }
                );
        }

        private static IObservable<AssetBundleRequestProgressEvent<Sprite>>
            Test_LoadAssetWithSubAssetsAsyncAsObservable_Generic_Sprite()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetWithSubAssetsAsyncAsObservable<Sprite>(ImageMainAssetName)
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .SkipWhileInProgress()
                .Do(
                    x =>
                    {
                        Assert.IsNotNull(x.MainAsset);
                        Assert.AreEqual(3, x.NumberOfSubAssets);
                        Assert.AreEqual(4, x.NumberOfAssets);
                    }
                );
        }

        private static IObservable<AssetBundleRequestProgressEvent<Texture2D>>
            Test_LoadAssetWithSubAssetsAsyncAsObservable_Generic_NameError()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetWithSubAssetsAsyncAsObservable<Texture2D>(ImageMainAssetName + "_fake")
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .DoOnCompleted(() => { throw new Exception(); })
                .CatchIgnore((AssetBundleRequestFailureException _) => Debug.Log("Caught expected error."));
        }

        private static IObservable<AssetBundleRequestProgressEvent<TextAsset>>
            Test_LoadAssetWithSubAssetsAsyncAsObservable_Generic_TypeError()
        {
            return ObservableAssetBundle.LoadFromFileAsync(ImageAssetBundleFilePath)
                .ChainTo(
                    x => x.CreatedAssetBundle.LoadAssetWithSubAssetsAsyncAsObservable<TextAsset>(ImageMainAssetName)
                        .Finally(() => x.CreatedAssetBundle.Unload(unloadAllLoadedObjects: true))
                )
                .DoOnCompleted(() => { throw new Exception(); })
                .CatchIgnore((AssetBundleRequestFailureException _) => Debug.Log("Caught expected error."));
        }
#endif

        // ReSharper disable once UnusedMember.Local
        private void Awake()
        {
            contents = new Dictionary<string, Func<IObservable<object>>>
            {
                {
                    "LoadFromFile",
                    WrapTestMethod(Test_LoadFromFile)
                },
                {
                    "LoadFromFile_Error",
                    WrapTestMethod(Test_LoadFromFile_Error)
                },
                {
                    "LoadFromMemory",
                    WrapTestMethod(Test_LoadFromMemory)
                },
                {
                    "LoadFromMemory_Error",
                    WrapTestMethod(Test_LoadFromMemory_Error)
                },
                {
                    "LoadAllAssetsAsyncAsObservable",
                    WrapTestMethod(Test_LoadAllAssetsAsyncAsObservable)
                },
                {
                    "LoadAllAssetsAsyncAsObservable_WithType_Texture2D",
                    WrapTestMethod(Test_LoadAllAssetsAsyncAsObservable_WithType_Texture2D)
                },
                {
                    "LoadAllAssetsAsyncAsObservable_WithType_Sprite",
                    WrapTestMethod(Test_LoadAllAssetsAsyncAsObservable_WithType_Sprite)
                },
                {
                    "LoadAllAssetsAsyncAsObservable_WithType_None",
                    WrapTestMethod(Test_LoadAllAssetsAsyncAsObservable_WithType_None)
                },
#if USE_FIXED_COMPILER
                {
                    "LoadAllAssetsAsyncAsObservable_Generic_Texture2D",
                    WrapTestMethod(Test_LoadAllAssetsAsyncAsObservable_Generic_Texture2D)
                },
                {
                    "LoadAllAssetsAsyncAsObservable_Generic_Sprite",
                    WrapTestMethod(Test_LoadAllAssetsAsyncAsObservable_Generic_Sprite)
                },
                {
                    "LoadAllAssetsAsyncAsObservable_Generic_None",
                    WrapTestMethod(Test_LoadAllAssetsAsyncAsObservable_Generic_None)
                },
#endif
                {
                    "LoadAssetAsyncAsObservable",
                    WrapTestMethod(Test_LoadAssetAsyncAsObservable)
                },
                {
                    "LoadAssetAsyncAsObservable_Error",
                    WrapTestMethod(Test_LoadAssetAsyncAsObservable_Error)
                },
                {
                    "LoadAssetAsyncAsObservable_WithType_Texture2D",
                    WrapTestMethod(Test_LoadAssetAsyncAsObservable_WithType_Texture2D)
                },
                {
                    "LoadAssetAsyncAsObservable_WithType_Sprite",
                    WrapTestMethod(Test_LoadAssetAsyncAsObservable_WithType_Sprite)
                },
                {
                    "LoadAssetAsyncAsObservable_WithType_NameError",
                    WrapTestMethod(Test_LoadAssetAsyncAsObservable_WithType_NameError)
                },
                {
                    "LoadAssetAsyncAsObservable_WithType_TypeError",
                    WrapTestMethod(Test_LoadAssetAsyncAsObservable_WithType_TypeError)
                },
#if USE_FIXED_COMPILER
                {
                    "LoadAssetAsyncAsObservable_Generic_Texture2D",
                    WrapTestMethod(Test_LoadAssetAsyncAsObservable_Generic_Texture2D)
                },
                {
                    "LoadAssetAsyncAsObservable_Generic_Sprite",
                    WrapTestMethod(Test_LoadAssetAsyncAsObservable_Generic_Sprite)
                },
                {
                    "LoadAssetAsyncAsObservable_Generic_NameError",
                    WrapTestMethod(Test_LoadAssetAsyncAsObservable_Generic_NameError)
                },
                {
                    "LoadAssetAsyncAsObservable_Generic_TypeError",
                    WrapTestMethod(Test_LoadAssetAsyncAsObservable_Generic_TypeError)
                },
#endif
                {
                    "LoadAssetWithSubAssetsAsyncAsObservable",
                    WrapTestMethod(Test_LoadAssetWithSubAssetsAsyncAsObservable)
                },
                {
                    "LoadAssetWithSubAssetsAsyncAsObservable_Error",
                    WrapTestMethod(Test_LoadAssetWithSubAssetsAsyncAsObservable_Error)
                },
                {
                    "LoadAssetWithSubAssetsAsyncAsObservable_WithType_Texture2D",
                    WrapTestMethod(Test_LoadAssetWithSubAssetsAsyncAsObservable_WithType_Texture2D)
                },
                {
                    "LoadAssetWithSubAssetsAsyncAsObservable_WithType_Sprite",
                    WrapTestMethod(Test_LoadAssetWithSubAssetsAsyncAsObservable_WithType_Sprite)
                },
                {
                    "LoadAssetWithSubAssetsAsyncAsObservable_WithType_NameError",
                    WrapTestMethod(Test_LoadAssetWithSubAssetsAsyncAsObservable_WithType_NameError)
                },
                {
                    "LoadAssetWithSubAssetsAsyncAsObservable_WithType_TypeError",
                    WrapTestMethod(Test_LoadAssetWithSubAssetsAsyncAsObservable_WithType_TypeError)
                },
#if USE_FIXED_COMPILER
                {
                    "LoadAssetWithSubAssetsAsyncAsObservable_Generic_Texture2D",
                    WrapTestMethod(Test_LoadAssetWithSubAssetsAsyncAsObservable_Generic_Texture2D)
                },
                {
                    "LoadAssetWithSubAssetsAsyncAsObservable_Generic_Sprite",
                    WrapTestMethod(Test_LoadAssetWithSubAssetsAsyncAsObservable_Generic_Sprite)
                },
                {
                    "LoadAssetWithSubAssetsAsyncAsObservable_Generic_NameError",
                    WrapTestMethod(Test_LoadAssetWithSubAssetsAsyncAsObservable_Generic_NameError)
                },
                {
                    "LoadAssetWithSubAssetsAsyncAsObservable_Generic_TypeError",
                    WrapTestMethod(Test_LoadAssetWithSubAssetsAsyncAsObservable_Generic_TypeError)
                },
#endif
            };
        }

        private static Func<IObservable<object>> WrapTestMethod<T>(Func<IObservable<T>> testMethod)
        {
            return () => testMethod().Select(x => (object) x);
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once InconsistentNaming
        private void OnGUI()
        {
            using (var scrollView = new GUILayout.ScrollViewScope(scrollPosition))
            {
                scrollPosition = scrollView.scrollPosition;

                const int buttonWidth = 420;

                foreach (var x in contents)
                    if (GUILayout.Button(x.Key, GUILayout.Width(buttonWidth)))
                        RunTest(x.Key, x.Value).Subscribe().AddTo(this);

                GUILayout.Space(20);

                if (GUILayout.Button("Run ALL", GUILayout.Width(buttonWidth)))
                    contents
                        .Select(x => RunTest(x.Key, x.Value))
                        .ChainAll()
                        .DoOnCompleted(() => Debug.Log("All tests were completed."))
                        .Subscribe(
                            _ => { },
                            error => Debug.LogError(error.Message + "\n\n-----\n" + error.StackTrace + "\n-----\n\n")
                        )
                        .AddTo(this);
            }
        }

        private static IObservable<T> RunTest<T>(string testName, Func<IObservable<T>> testMethod)
        {
            return testMethod()
                .Do(
                    x =>
                    {
                        Debug.Log(String.Format("Received: <{0}> {1}", x.GetType().Name, x));
                    },
                    error =>
                    {
                        Debug.LogWarning(
                            String.Format(
                                "Error in `{2}`\n[{0}] {1}",
                                error.GetType().Name,
                                error.Message,
                                testName
                                )
                            );
                    },
                    () =>
                    {
                        Debug.Log("Completed: " + testName);
                    }
                );
        }
#endif
    }
#endif
    }
