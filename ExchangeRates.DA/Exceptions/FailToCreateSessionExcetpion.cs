using System;

namespace ExchangeRates.DA.Exceptions
{
    internal class FailToCreateSessionExcetpion : DaException
    {
        public FailToCreateSessionExcetpion(Exception ex)
            :base("Error: Fail to establish conection to database", ex)
        {
            
        }
    }
}