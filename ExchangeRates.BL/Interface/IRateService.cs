using System;
using System.Collections.Generic;
using ExchangeRetes.DM;

namespace ExchangeRates.BL.Interface
{
    internal interface IRateService
    {
        IList<Rate> GetRates(Currency currency, IEnumerable<DateTime> interval);
    }
}