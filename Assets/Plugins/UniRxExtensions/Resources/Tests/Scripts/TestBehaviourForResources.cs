using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace UniRxExtensions.Resources.Tests
{
#if UNITY_EDITOR
    internal class TestBehaviourForResources : MonoBehaviour
    {
#if USE_FIXED_COMPILER
        private const string Path = "unirx_ex_test_asset";

        private readonly Dictionary<string, ITest> contents =
            new Dictionary<string, ITest>
            {
                {"Load (Generic)", new Test_LoadGeneric()},
                {"Load", new Test_LoadNotGeneric()},
                {"Load (Without Type)", new Test_LoadWithoutType()},
                {"Unload Unused Assets", new Test_UnloadUnusedAssets()},
            };

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once InconsistentNaming
        private void OnGUI()
        {
            foreach (var p in contents)
            {
                if (GUILayout.Button(p.Key, GUILayout.Width(160)))
                    p.Value.RunAsync().Subscribe().AddTo(this);
            }

            GUILayout.Space(20);

            if (GUILayout.Button("Run ALL"))
            {
                contents.Values
                    .Select(x => x.RunAsync())
                    .ChainAll()
                    .DoOnCompleted(() => Debug.Log("all tests were completed."))
                    .Subscribe()
                    .AddTo(this);
            }
        }

        private interface ITest
        {
            IObservable<Unit> RunAsync();
        }

        // ReSharper disable once InconsistentNaming
        private class Test_LoadGeneric :
            ITest
        {
            public IObservable<Unit> RunAsync()
            {
                TextAsset loadedAsset = null;

                return ObservableResources.LoadAsync<TextAsset>(Path)
                    .SkipWhileInProgress()
                    .Do(
                        x => loadedAsset = x.Asset,
                        () =>
                        {
                            Assert.IsNotNull(loadedAsset);
                            Debug.Log("completed: " + GetType().Name);
                        }
                    )
                    .AsUnitObservable();
            }
        }

        // ReSharper disable once InconsistentNaming
        private class Test_LoadNotGeneric :
            ITest
        {
            public IObservable<Unit> RunAsync()
            {
                Object loadedAsset = null;

                return ObservableResources.LoadAsync(Path, typeof(TextAsset))
                    .SkipWhileInProgress()
                    .Do(
                        x => loadedAsset = x.Asset,
                        () =>
                        {
                            Assert.IsNotNull(loadedAsset);
                            Assert.IsTrue(loadedAsset is TextAsset);
                            Debug.Log("completed: " + GetType().Name);
                        }
                    )
                    .AsUnitObservable();
            }
        }

        // ReSharper disable once InconsistentNaming
        private class Test_LoadWithoutType :
            ITest
        {
            public IObservable<Unit> RunAsync()
            {
                Object loadedAsset = null;

                return ObservableResources.LoadAsync(Path)
                    .SkipWhileInProgress()
                    .Do(
                        x => loadedAsset = x.Asset,
                        () =>
                        {
                            Assert.IsNotNull(loadedAsset);
                            Assert.IsTrue(loadedAsset is TextAsset);
                            Debug.Log("completed: " + GetType().Name);
                        }
                    )
                    .AsUnitObservable();
            }
        }

        // ReSharper disable once InconsistentNaming
        private class Test_UnloadUnusedAssets :
            ITest
        {
            public IObservable<Unit> RunAsync()
            {
                return ObservableResources.UnloadUnusedAssetsAsync()
                    .DoOnCompleted(
                        () =>
                        {
                            Debug.Log("completed: " + GetType().Name);
                        }
                    );
            }
        }
#endif
    }
#endif
    }
