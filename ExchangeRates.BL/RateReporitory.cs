using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRates.BL.Exceptions;
using ExchangeRates.BL.Interface;
using ExchangeRates.DA;
using ExchangeRetes.DM;

namespace ExchangeRates.BL
{
    /// <summary>
    /// Repository for caching rates in datasource 
    /// </summary>
    public class RateReporitory : IRateRepository
    {
        /// <summary>
        /// Return list of early cached rates <paramref name="currency"/> and date interval from <paramref name="startDate"/> to <paramref name="endDate"/> using <paramref name="session"/>
        /// </summary>
        /// <param name="session">Datasource session</param>
        /// <param name="currency">Selected currency</param>
        /// <param name="startDate">Start date of interval inclusively</param>
        /// <param name="endDate">End date of interval inclusively</param>
        /// <exception cref="GetCachedRepositoryException"/>
        /// <exception cref="ArgumentNullException"/>
        public IList<Rate> GetCached(ISession session, Currency currency, DateTime startDate, DateTime endDate)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            try
            {
                var start = startDate.Date.AddDays(-1);
                var end = endDate.Date.AddDays(1);
                return session.Query<Rate>()
                              .Where(t => t.Stamp > start)
                              .Where(t => t.Stamp < end)
                              .Where(t => t.Currency == currency)
                              .OrderBy(t => t.Stamp)
                              .ToList();
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new GetCachedRepositoryException(ex);
            }
        }

        /// <summary>
        /// Caches <paramref name="rates"/> using datasource <paramref name="session"/>
        /// </summary>
        /// <param name="session">New rates</param>
        /// <param name="rates">Datasource session</param>
        /// <exception cref="CacheRepositoryException"/>
        /// <exception cref="ArgumentNullException"/>
        public void Cache(ISession session, IEnumerable<Rate> rates)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            if (rates == null)
                throw new ArgumentNullException("rates");
            try
            {
                foreach (var rate in rates)
                {
                    session.Save(rate);
                }
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CacheRepositoryException(ex);
            }
        }
    }
}