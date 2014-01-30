using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRates.BL.Exceptions;
using ExchangeRates.BL.Interface;
using ExchangeRates.DA;
using ExchangeRetes.DM;

namespace ExchangeRates.BL
{
    public class RateReporitory : IRateRepository
    {
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