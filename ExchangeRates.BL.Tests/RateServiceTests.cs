using System;
using System.Linq;
using ExchangeRates.BL.Exceptions;
using ExchangeRates.BL.Interface;
using ExchangeRetes.DM;
using Moq;
using NUnit.Framework;

namespace ExchangeRates.BL.Tests
{
    [TestFixture]
    public class RateServiceTests
    {
        [Test]
        public void Ctor_ThrowsOnNullRateClient_Test()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new RateService(null));
        }

        [Test]
        public void GetRates_Test()
        {
            // Arrange
            var rate = Mock.Of<Rate>();
            var rateResponce = Mock.Of<RateSourceData>(t => t.Parse(Currency.RUB) == rate);
            var client = Mock.Of<IRateClient>(t => t.GetRate(DateTime.Today) == rateResponce);
            var rateService = new RateService(client);

            // Act
            var result = rateService.GetRates(Currency.RUB, new[] { DateTime.Today });

            // Assert
            Assert.AreEqual(result.Single(), rate);
        }

        [Test]
        public void GetRates_ApplicationException_Test()
        {
            // Arrange
            var exception = new ApplicationException();
            var client = new Mock<IRateClient>();
            client.Setup(t => t.GetRate(DateTime.Today)).Throws(exception);
            var rateService = new RateService(client.Object);

            // Assert
            var ex = Assert.Throws<ApplicationException>(() => rateService.GetRates(Currency.RUB, new[] { DateTime.Today }));
            Assert.AreEqual(ex, exception);
        }

        [Test]
        public void GetRates_UnexpectedException_Test()
        {
            // Arrange
            var exception = new Exception();
            var client = new Mock<IRateClient>();
            client.Setup(t => t.GetRate(DateTime.Today)).Throws(exception);
            var rateService = new RateService(client.Object);

            // Assert
            var ex =
                Assert.Throws<GetRatesFromServiceException>(
                    () => rateService.GetRates(Currency.RUB, new[] { DateTime.Today }));
            Assert.AreEqual(ex.InnerException, exception);
        }
    }
}
