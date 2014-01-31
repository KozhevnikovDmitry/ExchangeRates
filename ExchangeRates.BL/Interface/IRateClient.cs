using System;

namespace ExchangeRates.BL.Interface
{
    /// <summary>
    /// Interface for providing deserialized rates sources from remote service
    /// </summary>
    internal interface IRateClient
    {
        /// <summary>
        /// Provides rates sources by <paramref name="date"/>
        /// </summary>
        RateSourceData GetRate(DateTime date);
    }
}