using System;

namespace ExchangeRates.BL.Exceptions
{
    public class CacheRepositoryException : ApplicationException
    {
        public CacheRepositoryException(Exception ex)
            : base("Error: fail to cache rates (unexpected error on RateRepository.Cache).", ex)
        {

        }
    }
}