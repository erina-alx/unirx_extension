using System;
using UnityEngine;

namespace UniRxExtensions.AssetBundles
{
    public struct AssetBundleCreateRequestProgressEvent
    {
        private readonly float progress;
        private readonly bool isDone;
        private readonly AssetBundle createdAssetBundle;

        public AssetBundleCreateRequestProgressEvent(float progress)
        {
            this.progress = progress;
            isDone = false;
            createdAssetBundle = null;
        }

        public AssetBundleCreateRequestProgressEvent(AssetBundle createdAssetBundle)
        {
            if (ReferenceEquals(createdAssetBundle, null))
                throw new ArgumentNullException("createdAssetBundle");

            progress = 1f;
            isDone = true;
            this.createdAssetBundle = createdAssetBundle;
        }

        public float Progress
        {
            get { return progress; }
        }

        public bool IsDone
        {
            get { return isDone; }
        }

        public AssetBundle CreatedAssetBundle
        {
            get { return createdAssetBundle; }
        }

        public override string ToString()
        {
            return String.Format("(Is Done: {0}, Progress: {1})", IsDone, Progress);
        }
    }
}
