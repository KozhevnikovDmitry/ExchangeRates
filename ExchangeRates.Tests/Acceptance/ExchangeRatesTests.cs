using System;
using System.IO;
using System.Linq;
using Autofac;
using ExchangeRates.DA;
using ExchangeRetes.DM;
using Moq;
using NUnit.Framework;

namespace ExchangeRates.Tests.Acceptance
{
    /// <summary>
    /// Acceptance tests for ExchangeRates application logic
    /// </summary>
    [TestFixture]
    public class ExchangeRatesTests
    {
        private AcceptanceRoot _root;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _root = new AcceptanceRoot();
            _root.Register();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _root.Release();
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete("ExcnangeRates.sdf");
        }

        [Test]
        public void GetRatesToVm_CacheIsEmpty_Test()
        {
            // Arrange
            var exchangeRatesVm = _root.Resolve();
            exchangeRatesVm.Currency = Currency.GBP;
            exchangeRatesVm.StartDate = DateTime.Today.AddMonths(-1);
            exchangeRatesVm.EndDate = DateTime.Today;

            // Act
            exchangeRatesVm.GetRates();

            // Assert
            for (int i = 0; i < exchangeRatesVm.Rates.Count; i++)
            {
                var rate = exchangeRatesVm.Rates[i];
                Assert.AreEqual(rate.Currency, Currency.GBP, "Rate currency is GBP");
                Assert.AreEqual(rate.Stamp.Date, DateTime.Today.AddMonths(-1).AddDays(i), "Rate stamp is in last month");
                Assert.AreNotEqual(rate.Value, 0, "Rate value is not equal zero");
            }
            using (var session = _root.GetSessionFactory().New())
            {
                var cache = session.Query<Rate>().ToList();

                for (int i = 0; i < cache.Count; i++)
                {
                    var rate = exchangeRatesVm.Rates[i];
                    var cachedRate = cache[i];
                    Assert.AreEqual(rate.Currency, cachedRate.Currency, "Rate currency is equal cached rate currency");
                    Assert.AreEqual(rate.Stamp.Date, cachedRate.Stamp.Date, "Rate stamp is equal cached rate stamp");
                    Assert.AreEqual(rate.Value, cachedRate.Value, "Rate value is equal cached rate value");
                }
            }
        }

        [Test]
        public void GetRatesToVm_CacheIsEnough_Test()
        {
            // Arrange
            var exchangeRatesVm = _root.Resolve();
            exchangeRatesVm.Currency = Currency.RUB;
            exchangeRatesVm.StartDate = new DateTime(2000, 01, 01);
            exchangeRatesVm.EndDate = new DateTime(2000, 02, 01);

            using (var session = _root.GetSessionFactory().New())
            {
                var date = exchangeRatesVm.StartDate.Value;
                while (date <= exchangeRatesVm.EndDate)
                {
                    session.Save(new Rate
                    {
                        Currency = Currency.RUB,
                        Stamp = date,
                        Value = 100500
                    });

                    date = date.AddDays(1);
                }
            }

            // Act
            exchangeRatesVm.GetRates();

            // Assert
            for (int i = 0; i < exchangeRatesVm.Rates.Count; i++)
            {
                var rate = exchangeRatesVm.Rates[i];
                Assert.AreEqual(rate.Currency, Currency.RUB, "Rate currency is RUB");
                Assert.AreEqual(rate.Stamp.Date, new DateTime(2000, 01, 01).AddDays(i), "Rate stamp is between 01.01.2000 and 01.02.2000");
                Assert.AreEqual(rate.Value, 100500, "Rate value is 100500");
            }
        }

        [Test]
        public void GetRatesToVm_CacheIsNotEnough_Test()
        {
            // Arrange
            var exchangeRatesVm = _root.Resolve();
            exchangeRatesVm.Currency = Currency.EUR;
            exchangeRatesVm.StartDate = new DateTime(2000, 01, 01);
            exchangeRatesVm.EndDate = new DateTime(2000, 02, 01);

            using (var session = _root.GetSessionFactory().New())
            {
                var date = exchangeRatesVm.StartDate.Value;
                while (date <= new DateTime(2000, 01, 15))
                {
                    session.Save(new Rate
                    {
                        Currency = Currency.EUR,
                        Stamp = date,
                        Value = 100500
                    });

                    date = date.AddDays(1);
                }
            }

            // Act
            exchangeRatesVm.GetRates();

            // Assert
            for (int i = 0; i < 15; i++)
            {
                var rate = exchangeRatesVm.Rates[i];
                Assert.AreEqual(rate.Currency, Currency.EUR, "Rate currency is EUR");
                Assert.AreEqual(rate.Stamp.Date, new DateTime(2000, 01, 01).AddDays(i), "Rate stamp is between 01.01.2000 and 01.02.2000");
                Assert.AreEqual(rate.Value, 100500, "Rate value is 100500");
            }

            for (int i = 16; i < 31; i++)
            {
                var rate = exchangeRatesVm.Rates[i];
                Assert.AreEqual(rate.Currency, Currency.EUR, "Rate currency is EUR");
                Assert.AreEqual(rate.Stamp.Date, new DateTime(2000, 01, 01).AddDays(i), "Rate stamp is between 01.01.2000 and 01.02.2000");
                Assert.AreNotEqual(rate.Value, 100500, "Rate value is not equal 100500");
            }
        }

        [Test]
        public void GetRatesToVm_WithoutDb_Test()
        {
            // Arrange
            
            // Corrupt db-connection
            var contanerBuilder = new ContainerBuilder();
            contanerBuilder.RegisterInstance(Mock.Of<ISessionFactory>()).AsSelf().AsImplementedInterfaces();
            contanerBuilder.Update(_root.Root);

            var exchangeRatesVm = _root.Resolve();
            exchangeRatesVm.Currency = Currency.GBP;
            exchangeRatesVm.StartDate = DateTime.Today.AddMonths(-1);
            exchangeRatesVm.EndDate = DateTime.Today;

            // Act
            exchangeRatesVm.GetRates();

            // Assert
            for (int i = 0; i < exchangeRatesVm.Rates.Count; i++)
            {
                var rate = exchangeRatesVm.Rates[i];
                Assert.AreEqual(rate.Currency, Currency.GBP, "Rate currency is GBP");
                Assert.AreEqual(rate.Stamp.Date, DateTime.Today.AddMonths(-1).AddDays(i), "Rate stamp is in last month");
                Assert.AreNotEqual(rate.Value, 0, "Rate value is not equal zero");
            }

            Assert.False(exchangeRatesVm.IsSuccesfull, "Getting rates was performed with errors");
            Assert.IsNotEmpty(exchangeRatesVm.ErrorMessage, "Error message is provided");
        }
    }
}
