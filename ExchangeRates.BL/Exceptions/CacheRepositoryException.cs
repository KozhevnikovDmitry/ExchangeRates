using System;

namespace ExchangeRates.BL.Exceptions
{
    public class CacheRepositoryException : ApplicationException
    {
        public CacheRepositoryException(Exception ex)
            : base("UnexpectedExcetion on RateRepository.Cache", ex)
        {

        }
    }
}