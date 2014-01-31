using System;

namespace ExchangeRates.BL.Exceptions
{
    public class EndDateIsLaterThanTodayException : ApplicationException
    {
        public EndDateIsLaterThanTodayException()
            : base("Mistyping: End date is later than today.")
        {

        }
    }
}