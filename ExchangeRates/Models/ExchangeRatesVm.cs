using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ExchangeRates.BL.Interface;
using ExchangeRetes.DM;

namespace ExchangeRates.Models
{
    public class ExchangeRatesVm 
    {
        private readonly IExchangeRates _exchangeRates;

        public ExchangeRatesVm(IExchangeRates exchangeRates)
        {
            _exchangeRates = exchangeRates;
        }

        public SelectList CurencyList { get; set; }

        public Currency Currency { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public IList<Rate> Rates { get; set; }

        public void GetRates()
        {
            Rates = _exchangeRates.GetRates(Currency, StartDate, EndDate);
        }
    }
}