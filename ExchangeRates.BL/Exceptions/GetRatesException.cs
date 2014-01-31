using System;

namespace ExchangeRates.BL.Exceptions
{
    public class GetRatesException : ApplicationException
    {
        public GetRatesException(Exception ex)
            : base("Error: fail to get rates (unexpected error on ExchangeRates.GetRates)", ex)
        {

        }
    }
}