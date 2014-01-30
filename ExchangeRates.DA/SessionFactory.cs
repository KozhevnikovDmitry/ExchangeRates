namespace ExchangeRates.DA
{
    internal class SessionFactory : ISessionFactory
    {
        public ISession New()
        {
            return new Session();
        }
    }
}