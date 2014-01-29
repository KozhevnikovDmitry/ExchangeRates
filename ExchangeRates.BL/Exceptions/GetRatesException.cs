using System;

namespace ExchangeRates.BL.Exceptions
{
    public class GetRatesException : ApplicationException
    {
        public GetRatesException(Exception ex)
            : base("UnexpectedExcetion on EchangeRates.GetRates", ex)
        {

        }
    }
}