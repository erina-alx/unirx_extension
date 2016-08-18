using System;

namespace UniRxExtensions.AssetBundles
{
    public class AssetBundleRequestFailureException :
        Exception
    {
        public AssetBundleRequestFailureException()
        {
        }

        public AssetBundleRequestFailureException(string message) :
            base(message)
        {
        }

        public AssetBundleRequestFailureException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}
