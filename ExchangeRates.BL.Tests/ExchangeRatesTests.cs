using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRates.BL.Exceptions;
using ExchangeRates.BL.Interface;
using ExchangeRates.DA;
using ExchangeRetes.DM;
using Moq;
using NUnit.Framework;

namespace ExchangeRates.BL.Tests
{
    [TestFixture]
    public class ExchangeRatesTests
    {
        [Test]
        public void Ctor_ThrowsOnNullSessionFactory_Test()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ExchangeRates(null, Mock.Of<IRateRepository>(), Mock.Of<IRateService>()));
        }

        [Test]
        public void Ctor_ThrowsOnNullRateReapository_Test()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ExchangeRates(Mock.Of<ISessionFactory>(), null, Mock.Of<IRateService>()));
        }

        [Test]
        public void Ctor_ThrowsOnNullRateService_Test()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ExchangeRates(Mock.Of<ISessionFactory>(), Mock.Of<IRateRepository>(), null));
        }

        [Test]
        public void GetRates_EndDateIsEarilerThanStartDate_Test()
        {
            // Arrange
            var sessionFacotry = Mock.Of<ISessionFactory>();
            var repository = Mock.Of<IRateRepository>();
            var service = Mock.Of<IRateService>();
            var exchangeRates = new ExchangeRates(sessionFacotry, repository, service);

            // Assert
            Assert.Throws<EndDateIsEarilerThanStartDateException>(() => exchangeRates.GetRates(Currency.RUB, DateTime.Today.AddDays(1), DateTime.Today));
        }

        [Test]
        public void GetRates_SelectedPeriodExceedTwoMonths_Test()
        {
            // Arrange
            var sessionFacotry = Mock.Of<ISessionFactory>();
            var repository = Mock.Of<IRateRepository>();
            var service = Mock.Of<IRateService>();
            var exchangeRates = new ExchangeRates(sessionFacotry, repository, service);

            // Assert
            Assert.Throws<SelectedPeriodExceedTwoMonthsException>(() => exchangeRates.GetRates(Currency.RUB, DateTime.Today, DateTime.Today.AddDays(100)));
        }

        [Test]
        public void GetRates_ApplicationException_Test()
        {
            // Arrange
            var exception = new ApplicationException();
            var sessionFacotry = new Mock<ISessionFactory>();
            sessionFacotry.Setup(t => t.New()).Throws(exception);
            var repository = Mock.Of<IRateRepository>();
            var service = Mock.Of<IRateService>();
            var exchangeRates = new ExchangeRates(sessionFacotry.Object, repository, service);

            // Assert
            var ex = Assert.Throws<ApplicationException>(() => exchangeRates.GetRates(Currency.RUB, DateTime.Today, DateTime.Today.AddDays(1)));
            Assert.AreEqual(ex, exception);
        }

        [Test]
        public void GetRates_UnexpectedException_Test()
        {
            // Arrange
            var exception =new Exception();
            var sessionFacotry = new Mock<ISessionFactory>();
            sessionFacotry.Setup(t => t.New()).Throws(exception);
            var repository = Mock.Of<IRateRepository>();
            var service = Mock.Of<IRateService>();
            var exchangeRates = new ExchangeRates(sessionFacotry.Object, repository, service);

            // Assert
            var ex =
                Assert.Throws<GetRatesException>(
                    () => exchangeRates.GetRates(Currency.RUB, DateTime.Today, DateTime.Today.AddDays(1)));
            Assert.AreEqual(ex.InnerException, exception);
        }

        [Test]
        public void GetRates_EmprtyCache_Test()
        {
            // Arrange
            var session = Mock.Of<ISession>();
            var sessionFacotry = Mock.Of<ISessionFactory>(t => t.New() == session);
            var repository =
                Mock.Of<IRateRepository>(
                    t =>
                        t.GetCached(session, It.IsAny<Currency>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()) == new Rate[0]);
            var rate = Mock.Of<Rate>();
            var service =
                Mock.Of<IRateService>(
                    t => t.GetRates(Currency.RUB, It.Is<IEnumerable<DateTime>>(e => e.Contains(DateTime.Today) && e.Contains(DateTime.Today.AddDays(1)))) == new[] { rate });
            var exchangeRates = new ExchangeRates(sessionFacotry, repository, service);

            // Act
            var rates = exchangeRates.GetRates(Currency.RUB, DateTime.Today, DateTime.Today.AddDays(1));

            // Assert
            Assert.AreEqual(rates.Single(), rate);
        }

        [Test]
        public void GetRates_EmprtyCache_CacheRates_Test()
        {
            // Arrange
            var session = Mock.Of<ISession>();
            var sessionFacotry = Mock.Of<ISessionFactory>(t => t.New() == session);
            var repository =
                new Mock<IRateRepository>();
            repository.Setup(t => t.GetCached(session, It.IsAny<Currency>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new Rate[0]);
            var rate = Mock.Of<Rate>();
            var service =
                Mock.Of<IRateService>(
                    t => t.GetRates(Currency.RUB, It.Is<IEnumerable<DateTime>>(e => e.Contains(DateTime.Today) && e.Contains(DateTime.Today.AddDays(1)))) == new[] { rate });
            var exchangeRates = new ExchangeRates(sessionFacotry, repository.Object, service);

            // Act
            exchangeRates.GetRates(Currency.RUB, DateTime.Today, DateTime.Today.AddDays(1));

            // Assert
            repository.Verify(t => t.Cache(session, It.Is<IEnumerable<Rate>>(e => e.Single() == rate)));
        }

        [Test]
        public void GetRates_CacheIsEnough_Test()
        {
            // Arrange
            var session = Mock.Of<ISession>();
            var sessionFacotry = Mock.Of<ISessionFactory>(t => t.New() == session);
            var rate1 = Mock.Of<Rate>(t => t.Stamp == DateTime.Today);
            var rate2 = Mock.Of<Rate>(t => t.Stamp == DateTime.Today.AddDays(1));
            var repository =
                Mock.Of<IRateRepository>(
                    t =>
                        t.GetCached(session, It.IsAny<Currency>(), DateTime.Today, DateTime.Today.AddDays(1)) == new[] { rate2, rate1 });
            var service = new Mock<IRateService>();
            var exchangeRates = new ExchangeRates(sessionFacotry, repository, service.Object);

            // Act
            var rates = exchangeRates.GetRates(Currency.RUB, DateTime.Today, DateTime.Today.AddDays(1));

            // Assert
            Assert.AreEqual(rates.First(), rate1);
            Assert.AreEqual(rates.Last(), rate2);
            service.Verify(t => t.GetRates(It.IsAny<Currency>(), It.IsAny<IEnumerable<DateTime>>()), Times.Never);
        }

        [Test]
        public void GetRates_CacheIsEnough_UseDateOnly_Test()
        {
            // Arrange
            var now = DateTime.Now;
            var session = Mock.Of<ISession>();
            var sessionFacotry = Mock.Of<ISessionFactory>(t => t.New() == session);
            var rate1 = Mock.Of<Rate>(t => t.Stamp == now);
            var rate2 = Mock.Of<Rate>(t => t.Stamp == now.AddDays(1));
            var repository =
                Mock.Of<IRateRepository>(
                    t =>
                        t.GetCached(session, It.IsAny<Currency>(), now, now.AddDays(1)) == new[] { rate2, rate1 });
            var service = new Mock<IRateService>();
            var exchangeRates = new ExchangeRates(sessionFacotry, repository, service.Object);

            // Act
            var rates = exchangeRates.GetRates(Currency.RUB, now, now.AddDays(1));

            // Assert
            Assert.AreEqual(rates.First(), rate1);
            Assert.AreEqual(rates.Last(), rate2);
            service.Verify(t => t.GetRates(It.IsAny<Currency>(), It.IsAny<IEnumerable<DateTime>>()), Times.Never);
        }

        [Test]
        public void GetRates_CacheNotEnough_Test()
        {
            // Arrange
            var session = Mock.Of<ISession>();
            var sessionFacotry = Mock.Of<ISessionFactory>(t => t.New() == session);
            var rate1 = Mock.Of<Rate>(t => t.Stamp == DateTime.Today);
            var rate2 = Mock.Of<Rate>(t => t.Stamp == DateTime.Today.AddDays(1));
            var rate3 = Mock.Of<Rate>(t => t.Stamp == DateTime.Today.AddDays(2));
            var repository =
                Mock.Of<IRateRepository>(
                    t =>
                        t.GetCached(session, It.IsAny<Currency>(), DateTime.Today, DateTime.Today.AddDays(2)) == new[] { rate1, rate3 });
            var service = Mock.Of<IRateService>(t => t.GetRates(Currency.RUB, It.Is<IEnumerable<DateTime>>(e => e.Single() == DateTime.Today.AddDays(1))) == new[] { rate2});
            var exchangeRates = new ExchangeRates(sessionFacotry, repository, service);

            // Act
            var rates = exchangeRates.GetRates(Currency.RUB, DateTime.Today, DateTime.Today.AddDays(2));

            // Assert
            Assert.AreEqual(rates[0], rate1);
            Assert.AreEqual(rates[1], rate2);
            Assert.AreEqual(rates[2], rate3);
        }

        [Test]
        public void DisposeSessionTest()
        {
            // Arrange
            var session = new Mock<ISession>();
            var sessionFacotry = Mock.Of<ISessionFactory>(t => t.New() == session.Object);
            var repository =
                Mock.Of<IRateRepository>(
                    t =>
                        t.GetCached(session.Object, It.IsAny<Currency>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()) == new Rate[0]);
            var service =
                Mock.Of<IRateService>(
                    t => t.GetRates(Currency.RUB, It.IsAny<IEnumerable<DateTime>>()) == new Rate[0]);
            var exchangeRates = new ExchangeRates(sessionFacotry, repository, service);

            // Act
            exchangeRates.GetRates(Currency.RUB, DateTime.Today, DateTime.Today);

            // Assert
            session.Verify(t => t.Dispose(), Times.Once);
        }
    }
}

