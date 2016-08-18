using System;
using Object = UnityEngine.Object;

namespace UniRxExtensions.Resources
{
    public struct ResourceRequestProgressEvent<TAsset>
        where TAsset : Object
    {
        private readonly float progress;
        private readonly bool isDone;
        private readonly TAsset asset;

        public ResourceRequestProgressEvent(float progress) :
            this()
        {
            this.progress = progress;
        }

        public ResourceRequestProgressEvent(TAsset asset) :
            this()
        {
            if (ReferenceEquals(asset, null))
                throw new ArgumentNullException("asset");

            progress = 1;
            isDone = true;
            this.asset = asset;
        }

        public float Progress
        {
            get { return progress; }
        }

        public bool IsDone
        {
            get { return isDone; }
        }

        public TAsset Asset
        {
            get { return asset; }
        }
    }
}
