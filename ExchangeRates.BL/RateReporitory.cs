using System;
using System.Collections.Generic;
using System.Linq;
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

            return session.Query<Rate>()
                          .Where(t => t.Stamp.Date >= startDate.Date)
                          .Where(t => t.Stamp.Date <= endDate.Date)
                          .Where(t => t.Currency == currency)
                          .OrderBy(t => t.Stamp)
                          .ToList();
        }

        public void Cache(ISession session, IEnumerable<Rate> rates)
        {
            if (session == null) 
                throw new ArgumentNullException("session");

            if (rates == null) 
                throw new ArgumentNullException("rates");

            foreach (var rate in rates)
            {
                session.Save(rate);
            }
        }
    }
}