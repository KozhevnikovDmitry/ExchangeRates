using System;
using System.Collections.Generic;
using ExchangeRates.DA;
using ExchangeRetes.DM;

namespace ExchangeRates.BL.Interface
{
    internal interface IRateRepository
    {
        IList<Rate> GetCached(ISession session, Currency currency, DateTime startDate, DateTime endDate);

        void Cache(ISession session, IEnumerable<Rate> rates);
    }
}
