using System;

namespace ExchangeRates.Models.Exceptions
{
    public class FailToResolveViewModelException : ApplicationException
    {
        public FailToResolveViewModelException(Exception ex)
            :base("Error: Fail to resolve viewmodel from dependency root", ex)
        {
            
        }
    }
}