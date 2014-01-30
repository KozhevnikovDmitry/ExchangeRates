using System;
using System.Linq;
using ExchangeRates.BL.Exceptions;
using ExchangeRates.BL.Interface;
using ExchangeRates.Models;
using ExchangeRetes.DM;
using Moq;
using NUnit.Framework;

namespace ExchangeRates.Tests.Models
{
    [TestFixture]
    public class ExchangeRatesVmTests
    {
        [Test]
        public void Ctor_ThrowsOnNullExchangeRates_Test()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ExchangeRatesVm(null));
        }

        [Test]
        public void Ctor_DefaultData_Test()
        {
            // Act
            var vm = new ExchangeRatesVm(Mock.Of<IExchangeRates>());

            // Assert
            Assert.True(vm.IsSuccesfull);
            Assert.IsEmpty(vm.ErrorMessage);
            Assert.AreEqual(vm.StartDate, DateTime.Today.AddDays(-10));
            Assert.AreEqual(vm.EndDate, DateTime.Today);
            Assert.AreEqual(vm.Currency, Currency.RUB);
        }

        [Test]
        public void GetRates_Test()
        {
            // Arrange
            var rate1 = Mock.Of<Rate>();
            var rate2 = Mock.Of<Rate>();
            var exchangeRates =
                Mock.Of<IExchangeRates>(t =>
                    t.GetRates(Currency.GBP, new DateTime(2000, 1, 1), new DateTime(2000, 1, 2)) == new[] { rate1, rate2 });
            var vm = new ExchangeRatesVm(exchangeRates)
            {
                Currency = Currency.GBP,
                StartDate = new DateTime(2000, 1, 1),
                EndDate = new DateTime(2000, 1, 2)
            };

            // Act
            vm.GetRates();

            // Assert
            Assert.True(vm.IsSuccesfull);
            Assert.IsEmpty(vm.ErrorMessage);
            Assert.AreEqual(vm.Rates.First(), rate1);
            Assert.AreEqual(vm.Rates.Last(), rate2);
        }

        [Test]
        public void GetRates_EndDateIsEarilerThanStartDate_Test()
        {
            // Arrange
            var exchangeRates = new Mock<IExchangeRates>();
            exchangeRates.Setup(t => t.GetRates(It.IsAny<Currency>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws<EndDateIsEarilerThanStartDateException>();
            var vm = new ExchangeRatesVm(exchangeRates.Object);

            // Act
            vm.GetRates();

            // Assert
            Assert.False(vm.IsSuccesfull);
            Assert.AreEqual(vm.ErrorMessage, "Mistyping: End date is earlier than start date.");
        }

        [Test]
        public void GetRates_SelectedPeriodExceedTwoMonths_Test()
        {
            // Arrange
            var exchangeRates = new Mock<IExchangeRates>();
            exchangeRates.Setup(t => t.GetRates(It.IsAny<Currency>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws<SelectedPeriodExceedTwoMonthsException>();
            var vm = new ExchangeRatesVm(exchangeRates.Object);

            // Act
            vm.GetRates();

            // Assert
            Assert.False(vm.IsSuccesfull);
            Assert.AreEqual(vm.ErrorMessage, "Mistyping: Selected date interval exceeds two months.");
        }

        [Test]
        public void GetRates_ApplicationException_Test()
        {
            // Arrange
            var exception = Mock.Of<ApplicationException>(t => t.Message == "Application Exception");
            var exchangeRates = new Mock<IExchangeRates>();
            exchangeRates.Setup(t => t.GetRates(It.IsAny<Currency>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws(exception);
            var vm = new ExchangeRatesVm(exchangeRates.Object);

            // Act
            vm.GetRates();

            // Assert
            Assert.False(vm.IsSuccesfull);
            Assert.AreEqual(vm.ErrorMessage, "Error: Application Exception");
        }

        [Test]
        public void GetRates_UnexpectedException_Test()
        {
            // Arrange
            var exception = Mock.Of<Exception>(t => t.ToString() == "Unexpected Exception");
            var exchangeRates = new Mock<IExchangeRates>();
            exchangeRates.Setup(t => t.GetRates(It.IsAny<Currency>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws(exception);
            var vm = new ExchangeRatesVm(exchangeRates.Object);

            // Act
            vm.GetRates();

            // Assert
            Assert.False(vm.IsSuccesfull);
            Assert.AreEqual(vm.ErrorMessage, "Unexpected error: Unexpected Exception");
        }

        [Test]
        public void GetRates_StartDateHasNotValue_Test()
        {
            // Arrange
            var exchangeRates = Mock.Of<IExchangeRates>();
            var vm = new ExchangeRatesVm(exchangeRates)
            {
                StartDate = null
            };

            // Act
            vm.GetRates();

            // Assert
            Assert.False(vm.IsSuccesfull);
            Assert.AreEqual(vm.ErrorMessage, "Error: Start date must have value");
        }

        [Test]
        public void GetRates_EndDateHasNotValue_Test()
        {
            // Arrange
            var exchangeRates = Mock.Of<IExchangeRates>();
            var vm = new ExchangeRatesVm(exchangeRates)
            {
                EndDate = null
            };

            // Act
            vm.GetRates();

            // Assert
            Assert.False(vm.IsSuccesfull);
            Assert.AreEqual(vm.ErrorMessage, "Error: End date must have value");
        }

        [Test]
        public void GetDays_EmptyWhenRatesIsNull_Test()
        {
            // Arrange
            var exchangeRates = Mock.Of<IExchangeRates>();
            var vm = new ExchangeRatesVm(exchangeRates);

            // Act
            var days = vm.GetDays();

            // Assert
            Assert.IsEmpty(days);
        }

        [Test]
        public void GetDays_Test()
        {
            // Arrange
            var rate1 = Mock.Of<Rate>(t => t.Stamp == new DateTime(2001, 1, 1));
            var rate2 = Mock.Of<Rate>(t => t.Stamp == new DateTime(2002, 2, 2));
            var vm = new ExchangeRatesVm(Mock.Of<IExchangeRates>())
            {
                Rates = new[] { rate1, rate2 }
            };

            // Act
            var days = vm.GetDays();

            // Assert
            Assert.AreEqual(days, "'2001-01-01','2002-02-02'");
        }

        [Test]
        public void GetRatesData_EmptyWhenRatesIsNull_Test()
        {
            // Arrange
            var exchangeRates = Mock.Of<IExchangeRates>();
            var vm = new ExchangeRatesVm(exchangeRates);

            // Act
            var ratesData = vm.GetRatesData();

            // Assert
            Assert.IsEmpty(ratesData);
        }

        [Test]
        public void GetRatesData_Test()
        {
            // Arrange
            var rate1 = Mock.Of<Rate>(t => t.Stamp == new DateTime(2001, 1, 1) && t.Value == 123.456789);
            var rate2 = Mock.Of<Rate>(t => t.Stamp == new DateTime(2002, 2, 2) && t.Value == 987.654321);
            var vm = new ExchangeRatesVm(Mock.Of<IExchangeRates>())
            {
                Rates = new[] { rate1, rate2 }
            };

            // Act
            var ratesData = vm.GetRatesData();

            // Assert
            Assert.AreEqual(ratesData, "['2001-01-01',123.456789],['2002-02-02',987.654321]");
        }
    }
}
