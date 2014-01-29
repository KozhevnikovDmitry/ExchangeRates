using System;
using System.Collections.Generic;
using ExchangeRetes.DM;

namespace ExchangeRates.BL.Interface
{
    public interface IExchangeRates
    {
        IList<Rate> GetRates(Currency currency, DateTime startDate, DateTime endDate);
    }
}