namespace ExchangeRates.DA
{
    public interface ISessionFactory
    {
        ISession New();
    }

    internal class SessionFactory : ISessionFactory
    {
        public ISession New()
        {
            return new Session();
        }
    }
}
