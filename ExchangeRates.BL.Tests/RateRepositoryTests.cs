using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRates.BL.Exceptions;
using ExchangeRates.DA;
using ExchangeRetes.DM;
using Moq;
using NUnit.Framework;

namespace ExchangeRates.BL.Tests
{
    [TestFixture]
    public class RateRepositoryTests
    {
        [Test]
        public void GetCached_ByDates_Test()
        {
            // Arrange
            var rate = Mock.Of<Rate>(r => r.Stamp == DateTime.Today && r.Currency == Currency.RUB);
            var session = Mock.Of<ISession>(t => t.Query<Rate>() == new List<Rate>
            {
                Mock.Of<Rate>(r => r.Stamp == DateTime.Today.AddDays(-10) && r.Currency == Currency.RUB),
                rate,
                Mock.Of<Rate>(r => r.Stamp == DateTime.Today.AddDays(10) && r.Currency == Currency.RUB)
            }.AsQueryable());

            var repository = new RateReporitory();

            // Act
            var rates = repository.GetCached(session, Currency.RUB, DateTime.Today.AddDays(-5), DateTime.Today.AddDays(5));

            // Assert
            Assert.AreEqual(rates.Single(), rate);
        }

        [Test]
        public void GetCached_ByDates_UseOnluDates_Test()
        {
            // Arrange
            var now = DateTime.Now;
            var rate = Mock.Of<Rate>(r => r.Stamp == now && r.Currency == Currency.RUB);
            var session = Mock.Of<ISession>(t => t.Query<Rate>() == new List<Rate>
            {
                Mock.Of<Rate>(r => r.Stamp == now.AddDays(-10) && r.Currency == Currency.RUB),
                rate,
                Mock.Of<Rate>(r => r.Stamp == now.AddDays(10) && r.Currency == Currency.RUB)
            }.AsQueryable());

            var repository = new RateReporitory();

            // Act
            var rates = repository.GetCached(session, Currency.RUB, now.AddDays(-5), now.AddDays(5));

            // Assert
            Assert.AreEqual(rates.Single(), rate);
        }

        [Test]
        public void GetCached_ByCurrency_Test()
        {
            // Arrange
            var rate = Mock.Of<Rate>(r => r.Stamp == DateTime.Today && r.Currency == Currency.RUB);
            var session = Mock.Of<ISession>(t => t.Query<Rate>() == new List<Rate>
            {
                Mock.Of<Rate>(r => r.Stamp == DateTime.Today && r.Currency == Currency.EUR),
                rate,
                Mock.Of<Rate>(r => r.Stamp == DateTime.Today && r.Currency == Currency.GBP)
            }.AsQueryable());

            var repository = new RateReporitory();

            // Act
            var rates = repository.GetCached(session, Currency.RUB, DateTime.Today, DateTime.Today);

            // Assert
            Assert.AreEqual(rates.Single(), rate);
        }

        [Test]
        public void GetCached_OrderByStamp_Test()
        {
            // Arrange
            var rate2 = Mock.Of<Rate>(r => r.Stamp == DateTime.Today && r.Currency == Currency.RUB);
            var rate1 = Mock.Of<Rate>(r => r.Stamp == DateTime.Today.AddDays(1) && r.Currency == Currency.RUB);
            var session = Mock.Of<ISession>(t => t.Query<Rate>() == new List<Rate>
            {
                rate1,
                rate2
            }.AsQueryable());

            var repository = new RateReporitory();

            // Act
            var rates = repository.GetCached(session, Currency.RUB, DateTime.Today, DateTime.Today.AddDays(1));

            // Assert
            Assert.AreEqual(rates[0], rate2);
            Assert.AreEqual(rates[1], rate1);
        }

        [Test]
        public void GetCached_ThrowsOnNullSession_Test()
        {
            // Arrange
            var repository = new RateReporitory();

            // Assert
            Assert.Throws<ArgumentNullException>(() => repository.GetCached(null, Currency.RUB, DateTime.Today.AddDays(-5), DateTime.Today.AddDays(5)));
        }

        [Test]
        public void GetCached_ApplicationException_Test()
        {
            // Arrange
            var exception = new ApplicationException();
            var session = new Mock<ISession>();
            session.Setup(t => t.Query<Rate>()).Throws(exception);
            var repository = new RateReporitory();

            // Assert
            var ex = Assert.Throws<ApplicationException>(() => repository.GetCached(session.Object, Currency.RUB, DateTime.Today, DateTime.Today));
            Assert.AreEqual(ex, exception);
        }

        [Test]
        public void GetCached_UnexpectedException_Test()
        {
            // Arrange
            var exception = new Exception();
            var session = new Mock<ISession>();
            session.Setup(t => t.Query<Rate>()).Throws(exception);
            var repository = new RateReporitory();

            // Assert
            var ex = Assert.Throws<GetCachedRepositoryException>(() => repository.GetCached(session.Object, Currency.RUB, DateTime.Today, DateTime.Today));
            Assert.AreEqual(ex.InnerException, exception);
        }

        [Test]
        public void Cache_Test()
        {
            // Arrange
            var rate = Mock.Of<Rate>();
            var session = new Mock<ISession>();
            var repository = new RateReporitory();

            // Act
            repository.Cache(session.Object, new[] { rate, rate });

            // Assert
            session.Verify(t => t.Save(rate), Times.Exactly(2));
        }

        [Test]
        public void Cache_ThrowsOnNullSession_Test()
        {
            // Arrange
            var repository = new RateReporitory();

            // Assert
            Assert.Throws<ArgumentNullException>(() => repository.Cache(null, Mock.Of<IEnumerable<Rate>>()));
        }

        [Test]
        public void Cache_ThrowsOnNullRates_Test()
        {
            // Arrange
            var repository = new RateReporitory();

            // Assert
            Assert.Throws<ArgumentNullException>(() => repository.Cache(Mock.Of<ISession>(), null));
        }

        [Test]
        public void Cache_ApplicationException_Test()
        {
            // Arrange
            var exception = new ApplicationException();
            var session = new Mock<ISession>();
            session.Setup(t => t.Save<Rate>(It.IsAny<Rate>())).Throws(exception);
            var repository = new RateReporitory();

            // Assert
            var ex = Assert.Throws<ApplicationException>(() => repository.Cache(session.Object, new[] { Mock.Of<Rate>() }));
            Assert.AreEqual(ex, exception);
        }

        [Test]
        public void Cache_UnexpectedException_Test()
        {
            // Arrange
            var exception = new Exception();
            var session = new Mock<ISession>();
            session.Setup(t => t.Save<Rate>(It.IsAny<Rate>())).Throws(exception);
            var repository = new RateReporitory();

            // Assert
            var ex = Assert.Throws<CacheRepositoryException>(() => repository.Cache(session.Object, new[] { Mock.Of<Rate>() }));
            Assert.AreEqual(ex.InnerException, exception);
        }
    }
}
