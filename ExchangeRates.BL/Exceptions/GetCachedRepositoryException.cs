using System;

namespace ExchangeRates.BL.Exceptions
{
    public class GetCachedRepositoryException : ApplicationException
    {
        public GetCachedRepositoryException(Exception ex)
            : base("UnexpectedExcetion on RateRepository.GetChached", ex)
        {

        }
    }
}