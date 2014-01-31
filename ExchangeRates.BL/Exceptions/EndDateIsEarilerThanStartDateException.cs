using System;

namespace ExchangeRates.BL.Exceptions
{
    public class EndDateIsEarilerThanStartDateException : ApplicationException
    {
        public EndDateIsEarilerThanStartDateException()
            : base("Mistyping: End date is earlier than start date.")
        {
            
        }
    }
}