using System;
using System.Web.Mvc;
using ExchangeRates.BL.Interface;

namespace ExchangeRates.Models
{
    public class ExchangeRatesVm 
    {
        private readonly IExchangeRates _exchangeRates;

        public ExchangeRatesVm(IExchangeRates exchangeRates)
        {
            _exchangeRates = exchangeRates;
        }

        public SelectList CompanyList { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}