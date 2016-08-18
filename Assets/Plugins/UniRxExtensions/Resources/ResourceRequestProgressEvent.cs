using System;
using Object = UnityEngine.Object;

namespace UniRxExtensions.Resources
{
    public struct ResourceRequestProgressEvent
    {
        private readonly float progress;
        private readonly bool isDone;
        private readonly Object asset;

        public ResourceRequestProgressEvent(float progress) :
            this()
        {
            this.progress = progress;
        }

        public ResourceRequestProgressEvent(Object asset) :
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

        public Object Asset
        {
            get { return asset; }
        }
    }
}
