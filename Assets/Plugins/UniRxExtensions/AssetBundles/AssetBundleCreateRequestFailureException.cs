using System;

namespace UniRxExtensions.AssetBundles
{
    public class AssetBundleCreateRequestFailureException :
        Exception
    {
        public AssetBundleCreateRequestFailureException()
        {
        }

        public AssetBundleCreateRequestFailureException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}
