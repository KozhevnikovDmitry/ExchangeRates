using System;
using ExchangeRates.DA.Exceptions;

namespace ExchangeRates.DA
{
    internal class SessionFactory : ISessionFactory
    {
        public ISession New()
        {
            try
            {
                return new Session();
            }
            catch (Exception ex)
            {
                throw new FailToCreateSessionExcetpion(ex);
            }
        }
    }
}