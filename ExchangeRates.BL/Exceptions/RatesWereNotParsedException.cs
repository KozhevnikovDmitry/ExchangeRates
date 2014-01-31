using System;

namespace ExchangeRates.BL.Exceptions
{
    internal class RatesWereNotParsedException : Exception
    {
        public RatesWereNotParsedException()
            : base("Error: fail to parse rate from service responce (unexpected error on RateResponce.Parse)")
        {
            
        }
    }
}