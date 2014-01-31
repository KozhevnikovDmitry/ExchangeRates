using System;
using System.Collections.Generic;
using ExchangeRates.DA;
using ExchangeRetes.DM;

namespace ExchangeRates.BL.Interface
{
    /// <summary>
    /// Interface for caching rates in datasource 
    /// </summary>
    internal interface IRateRepository
    {
        /// <summary>
        /// Return list of early cached rates <paramref name="currency"/> and date interval from <paramref name="startDate"/> to <paramref name="endDate"/> using <paramref name="session"/>
        /// </summary>
        /// <param name="session">Datasource session</param>
        /// <param name="currency">Selected currency</param>
        /// <param name="startDate">Start date of interval inclusively</param>
        /// <param name="endDate">End date of interval inclusively</param>
        IList<Rate> GetCached(ISession session, Currency currency, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Caches <paramref name="rates"/> using datasource <paramref name="session"/>
        /// </summary>
        /// <param name="session">New rates</param>
        /// <param name="rates">Datasource session</param>
        void Cache(ISession session, IEnumerable<Rate> rates);
    }
}
