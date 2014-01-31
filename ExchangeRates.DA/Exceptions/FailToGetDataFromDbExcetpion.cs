using System;

namespace ExchangeRates.DA.Exceptions
{
    internal class FailToGetDataFromDbExcetpion : DaException
    {
        public FailToGetDataFromDbExcetpion(Exception ex)
            : base("Error: Fail to get data from database", ex)
        {

        }
    }
}