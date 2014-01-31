using System;

namespace ExchangeRates.DA.Exceptions
{
    internal class FailToSetupDataToDbExcetpion : DaException
    {
        public FailToSetupDataToDbExcetpion(Exception ex)
            : base("Error: Fail to setup data to database", ex)
        {

        }
    }
}