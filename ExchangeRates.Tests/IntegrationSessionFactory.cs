using ExchangeRates.DA;

namespace ExchangeRates.Tests
{
    public class IntegrationSessionFactory : ISessionFactory
    {
        public ISession New()
        {
            if (IntegrationSession == null)
            {
                var connection = Effort.DbConnectionFactory.CreateTransient();
                IntegrationSession = new IntegrationSession(connection);
            }
            return IntegrationSession;
        }

        public IntegrationSession IntegrationSession { get; set; }
    }
}