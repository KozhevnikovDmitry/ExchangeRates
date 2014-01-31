using System;
using System.Collections.Generic;
using ExchangeRetes.DM;

namespace ExchangeRates.BL.Interface
{
    /// <summary>
    /// An interface for providing exchange rates data
    /// </summary>
    public interface IExchangeRates
    {
        /// <summary>
        /// Returns ordered rates list for currency and date interval
        /// </summary>
        /// <param name="currency">Selected currency</param>
        /// <param name="startDate">Start date of interval inclusively</param>
        /// <param name="endDate">End date of interval inclusively</param>
        /// <returns></returns>
        IList<Rate> GetRates(Currency currency, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Some error information about <see cref="GetRates"/> performing
        /// </summary>
        string ErrorMessage { get; }
    }
}