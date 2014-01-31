using System;

namespace ExchangeRates.BL.Exceptions
{
    public class GetCachedRepositoryException : ApplicationException
    {
        public GetCachedRepositoryException(Exception ex)
            : base("Error: fail to get cached rates (unexpected error on RateRepository.GetCached)", ex)
        {

        }
    }
}