using System;

namespace ExchangeRates.BL.Exceptions
{
    public class GetRatesFromServiceException : ApplicationException
    {
        public GetRatesFromServiceException(Exception ex)
            : base("UnexpectedExcetion on RateService.GetRates", ex)
        {

        }
    }
}