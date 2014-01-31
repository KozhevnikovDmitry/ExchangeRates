using System;

namespace ExchangeRates.Models.Exceptions
{
    public class UnexpectedViewModelException : ApplicationException
    {
        public UnexpectedViewModelException(Exception ex)
            : base("Error: unexpected viewmodel exception", ex)
        {

        }
    }
}