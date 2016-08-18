using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Object = UnityEngine.Object;

namespace UniRxExtensions.AssetBundles
{
    public struct AssetBundleRequestProgressEvent<T>
        where T : Object
    {
        private static readonly ReadOnlyCollection<T> EmptyCollection =
            new ReadOnlyCollection<T>(new T[] {});

        private readonly float progress;
        private readonly bool isDone;
        private readonly T mainAsset;
        private readonly int numberOfAssets;

        private ReadOnlyCollection<T> subAssets;
        private ReadOnlyCollection<T> allAssets;

        public AssetBundleRequestProgressEvent(float progress)
        {
            this.progress = progress;
            isDone = false;
            mainAsset = null;
            subAssets = EmptyCollection;
            allAssets = EmptyCollection;
            numberOfAssets = 0;
        }

        public AssetBundleRequestProgressEvent(T mainAsset)
        {
            if (ReferenceEquals(mainAsset, null))
                throw new ArgumentNullException("mainAsset");

            progress = 1f;
            isDone = true;
            this.mainAsset = mainAsset;
            subAssets = EmptyCollection;
            allAssets = null;
            numberOfAssets = 1;
        }

        public AssetBundleRequestProgressEvent(ReadOnlyCollection<T> allAssets)
        {
            if (allAssets == null)
                throw new ArgumentNullException("allAssets");
            if (allAssets.Any(x => ReferenceEquals(x, null)))
                throw new ArgumentException("Asset must not be null.", "allAssets");

            progress = 1f;
            isDone = true;
            mainAsset = null;
            subAssets = EmptyCollection;
            this.allAssets = allAssets;
            numberOfAssets = allAssets.Count;
        }

        public AssetBundleRequestProgressEvent(T mainAsset, ReadOnlyCollection<T> subAssets)
        {
            if (ReferenceEquals(mainAsset, null))
                throw new ArgumentNullException("mainAsset");
            if (subAssets == null)
                throw new ArgumentNullException("subAssets");
            if (subAssets.Any(x => ReferenceEquals(x, null)))
                throw new ArgumentException("Asset must not be null.", "subAssets");

            progress = 1f;
            isDone = true;
            this.mainAsset = mainAsset;
            this.subAssets = subAssets;
            allAssets = null;
            numberOfAssets = subAssets.Count + 1;
        }

        public float Progress
        {
            get { return progress; }
        }

        public bool IsDone
        {
            get { return isDone; }
        }

        public T MainAsset
        {
            get { return mainAsset; }
        }

        public ReadOnlyCollection<T> SubAssets
        {
            get { return subAssets ?? (subAssets = EmptyCollection); }
        }

        public ReadOnlyCollection<T> AllAssets
        {
            get
            {
                if (allAssets != null)
                    return allAssets;

                if (!IsDone)
                    return allAssets = EmptyCollection;

                if (!ReferenceEquals(MainAsset, null))
                {
                    if (SubAssets.Count > 0)
                    {
                        var array = new T[SubAssets.Count + 1];
                        array[0] = MainAsset;
                        SubAssets.CopyTo(array, 1);

                        return allAssets = new ReadOnlyCollection<T>(array);
                    }

                    return allAssets = new ReadOnlyCollection<T>(new[] {MainAsset});
                }

                if (SubAssets.Count == 0)
                    return allAssets = EmptyCollection;

                return allAssets = SubAssets;
            }
        }

        public int NumberOfAssets
        {
            get { return numberOfAssets; }
        }

        public int NumberOfSubAssets
        {
            get { return SubAssets.Count; }
        }

        public bool ContainsAsset
        {
            get { return NumberOfAssets > 0; }
        }

        public bool ContainsMainAsset
        {
            get { return !ReferenceEquals(MainAsset, null); }
        }

        public bool ContainsSubAsset
        {
            get { return NumberOfSubAssets > 0; }
        }

#if USE_FIXED_COMPILER
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("(Is Done: {0}, Progress: {1}", IsDone, Progress);

            if (IsDone)
            {
                sb.AppendFormat(", Number of Assets: {0}", NumberOfAssets);

                if (ContainsMainAsset)
                {
                    if (MainAsset != null)
                        sb.AppendFormat(", Main Asset: <{1}> \"{0}\"", MainAsset.name, MainAsset.GetType().Name);
                    else
                        sb.Append(", Main Asset: (destroyed)");
                }

                if (ContainsSubAsset)
                {
                    sb.Append(", Sub Assets: (");
                    BuildAssetsString(sb, SubAssets);
                    sb.Append(")");
                }

                if (!ContainsMainAsset && !ContainsSubAsset)
                {
                    sb.Append(", Assets: (");
                    BuildAssetsString(sb, AllAssets);
                    sb.Append(")");
                }
            }

            sb.Append(")");

            return sb.ToString();
        }

        private static void BuildAssetsString(StringBuilder sb, IList<T> assets)
        {
            for (var i = 0; i < 3 && i < assets.Count; i++)
            {
                if (i > 0)
                    sb.Append(", ");

                if (assets[i] != null)
                    sb.AppendFormat("\"{0}\"", assets[i].name);
                else
                    sb.Append("(destroyed)");
            }

            if (assets.Count > 3)
                sb.Append(", ...");
        }
#else
        public override string ToString()
        {
            return String.Format("(Is Done: {0}, Progress: {1})", IsDone, Progress);
        }
#endif
    }
}
