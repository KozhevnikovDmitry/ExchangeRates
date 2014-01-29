using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRates.BL.Exceptions;
using ExchangeRates.BL.Interface;
using ExchangeRetes.DM;

namespace ExchangeRates.BL
{
    internal class RateService : IRateService
    {
        private readonly IRateClient _rateClient;

        public RateService(IRateClient rateClient)
        {
            if (rateClient == null) 
                throw new ArgumentNullException("rateClient");

            _rateClient = rateClient;
        }

        public IList<Rate> GetRates(Currency currency, IEnumerable<DateTime> interval)
        {
            try
            {
                return interval.Select(t => _rateClient.GetRate(t).Parse(currency))
                    .ToList();
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new GetRatesFromServiceException(ex);
            }
        }
    }
}