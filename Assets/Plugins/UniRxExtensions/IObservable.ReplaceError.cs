using System;
using UniRx;
using UniRxExtensions.Operators;

namespace UniRxExtensions
{
    // ReSharper disable once InconsistentNaming
    public static class _IObservable_ReplaceError
    {
        public static IObservable<T> ReplaceError<T, TSourceException, TDestinationException>(
            this IObservable<T> source,
            Func<TSourceException, TDestinationException> exceptionReplacer
            )
            where TSourceException : Exception
            where TDestinationException : Exception
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new ReplaceErrorObservable<T, TSourceException, TDestinationException>(source, exceptionReplacer);
        }
    }
}
