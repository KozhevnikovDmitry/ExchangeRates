using Autofac;
using ExchangeRates.BL;
using ExchangeRates.DA;
using ExchangeRates.Models;

namespace ExchangeRates.Tests.Acceptance
{
    public class AcceptanceRoot
    {
        public IContainer Root { get; private set; }
        
        public void Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<BlModule>();
            builder.RegisterModule<DaModule>();
            builder.RegisterModule<UiModule>();
            Root = builder.Build();
        }

        public ExchangeRatesVm Resolve()
        {
            return Root.Resolve<ExchangeRatesVm>();
        }

        public ISessionFactory GetSessionFactory()
        {
            return Root.Resolve<ISessionFactory>();
        }

        public void Release()
        {
            Root.Dispose();
        }
    }
}
