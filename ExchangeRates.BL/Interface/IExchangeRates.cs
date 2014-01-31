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
        /// Some error information about <see cref="GetRates"/> performing
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        /// Returns ordered rates list for <paramref name="currency"/> and date interval from <paramref name="startDate"/> to <paramref name="endDate"/>
        /// </summary>
        /// <param name="currency">Selected currency</param>
        /// <param name="startDate">Start date of interval inclusively</param>
        /// <param name="endDate">End date of interval inclusively</param>
        IList<Rate> GetRates(Currency currency, DateTime startDate, DateTime endDate);
    }
}