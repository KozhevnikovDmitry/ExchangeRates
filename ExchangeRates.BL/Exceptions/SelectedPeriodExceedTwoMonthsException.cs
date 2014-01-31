using System;

namespace ExchangeRates.BL.Exceptions
{
    public class SelectedPeriodExceedTwoMonthsException : ApplicationException
    {
        public SelectedPeriodExceedTwoMonthsException()
            : base("Mistyping: Selected date interval exceeds two months.")
        {
            
        }
    }
}