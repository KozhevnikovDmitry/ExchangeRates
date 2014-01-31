using System;

namespace ExchangeRates.DA.Exceptions
{
    public class DaException : ApplicationException
    {
        public DaException()
        {
            
        }

        public DaException(string message, Exception ex)
            :base(message, ex)
        {
            
        }
    }
}