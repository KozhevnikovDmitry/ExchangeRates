using System;

namespace ExchangeRates.BL.Exceptions
{
    public class GetRatesFromServiceException : ApplicationException
    {
        public GetRatesFromServiceException(Exception ex)
            : base("Error: fail to get rates from web service  (unexpected error on RateService.GetRate)", ex)
        {

        }
    }
}