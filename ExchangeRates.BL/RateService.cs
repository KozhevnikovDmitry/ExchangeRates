using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRates.BL.Exceptions;
using ExchangeRates.BL.Interface;
using ExchangeRetes.DM;

namespace ExchangeRates.BL
{
    /// <summary>
    /// Service, that provide remote access to rates data
    /// </summary>
    internal class RateService : IRateService
    {
        private readonly IRateClient _rateClient;

        /// <summary>
        /// Constructs example of <see cref="RateService"/>
        /// </summary>
        /// <param name="rateClient">Http adapter for getting rates sources</param>
        /// <exception cref="ArgumentNullException"/>
        public RateService(IRateClient rateClient)
        {
            if (rateClient == null) 
                throw new ArgumentNullException("rateClient");

            _rateClient = rateClient;
        }

        /// <summary>
        /// Returns rates data for <paramref name="currency"/> by every date in <paramref name="dates"/>
        /// </summary>
        /// <param name="currency">Selected currency</param>
        /// <param name="dates">Rates dates</param>
        /// <exception cref="GetRatesFromServiceException"/>
        public IList<Rate> GetRates(Currency currency, IEnumerable<DateTime> dates)
        {
            try
            {
                return dates.Select(t => _rateClient.GetRate(t).Parse(currency))
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