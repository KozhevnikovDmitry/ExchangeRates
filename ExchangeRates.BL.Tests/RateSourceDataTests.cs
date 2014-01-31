using System;
using ExchangeRates.BL.Exceptions;
using ExchangeRetes.DM;
using NUnit.Framework;

namespace ExchangeRates.BL.Tests
{
    [TestFixture]
    public class RateSourceDataTests
    {
        [Test]
        public void Parse_UnixTime_Test()
        {
            // Arrange
            var rateSourceData = new RateSourceData
            {
                Stamp = 0,
                RatesValues = new RateValuesData()
            };

            // Act
            var rate = rateSourceData.Parse(Currency.RUB);

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
            var rateSourceData = new RateSourceData
            {
                RatesValues = new RateValuesData
                {
                    EUR = 1,
                    GBP = 2,
                    JPY = 3,
                    RUB = 4,
                    USD = 5,
                }
            };

            // Act
            var rate = rateSourceData.Parse(currency);

            // Assert
            return rate.Value;
        }

        [Test]
        public void Parse_ThrowsRatesWereNotParsed_OnNullRateDetail_Test()
        {
            // Arrange
            var rateSourceData = new RateSourceData();

            // Assert
            Assert.Throws<RatesWereNotParsedException>(() => rateSourceData.Parse(Currency.RUB));
        }
    }
}