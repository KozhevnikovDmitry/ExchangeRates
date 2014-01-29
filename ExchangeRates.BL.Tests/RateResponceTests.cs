using System;
using ExchangeRates.BL.Exceptions;
using ExchangeRetes.DM;
using NUnit.Framework;

namespace ExchangeRates.BL.Tests
{
    [TestFixture]
    public class RateResponceTests
    {
        [Test]
        public void Parse_UnixTime_Test()
        {
            // Arrange
            var rateResponce = new RateResponce
            {
                Stamp = 0,
                RateDetail = new RateDetailResponce()
            };

            // Act
            var rate = rateResponce.Parse(Currency.RUB);

            // Assert
            Assert.AreEqual(rate.Stamp.Date, new DateTime(1970, 1, 1, 0, 0, 0));
        }

        [TestCase(Currency.EUR, Result = 1)]
        [TestCase(Currency.GBP, Result = 2)]
        [TestCase(Currency.JPY, Result = 3)]
        [TestCase(Currency.RUB, Result = 4)]
        [TestCase(Currency.USD, Result = 5)]
        public double Parse_Currency_Test(Currency currency)
        {
            // Arrange
            var rateResponce = new RateResponce
            {
                RateDetail = new RateDetailResponce
                {
                    EUR = 1,
                    GBP = 2,
                    JPY = 3,
                    RUB = 4,
                    USD = 5,
                }
            };

            // Act
            var rate = rateResponce.Parse(currency);

            // Assert
            return rate.Value;
        }

        [Test]
        public void Parse_ThrowsRatesWereNotParsed_OnNullRateDetail_Test()
        {
            // Arrange
            var rateResponce = new RateResponce();

            // Assert
            Assert.Throws<RatesWereNotParsedException>(() => rateResponce.Parse(Currency.RUB));
        }
    }
}