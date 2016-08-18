using System;

namespace UniRxExtensions.Resources
{
    public class ResourceRequestFailureException :
        Exception
    {
        public ResourceRequestFailureException()
        {
        }

        public ResourceRequestFailureException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }
}
