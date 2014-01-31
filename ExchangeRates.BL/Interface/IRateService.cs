using System;
using System.Collections.Generic;
using ExchangeRetes.DM;

namespace ExchangeRates.BL.Interface
{
    /// <summary>
    /// Interface for providing remote access to rates data
    /// </summary>
    internal interface IRateService
    {
        /// <summary>
        /// Returns rates data for <paramref name="currency"/> by every date in <paramref name="dates"/>
        /// </summary>
        /// <param name="currency">Selected currency</param>
        /// <param name="dates">Rates dates</param>
        IList<Rate> GetRates(Currency currency, IEnumerable<DateTime> dates);
    }
}