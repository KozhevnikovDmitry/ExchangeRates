using System;

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
                return new FailngSession(ex);
            }
        }
    }
}