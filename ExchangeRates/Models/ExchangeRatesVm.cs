using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            if (exchangeRates == null) 
                throw new ArgumentNullException("exchangeRates");
            _exchangeRates = exchangeRates;
            IsSuccesfull = true;
            ErrorMessage = string.Empty;
            Currency = Currency.RUB;
            StartDate = DateTime.Today.AddDays(-10);
            EndDate = DateTime.Today;
            CurrencyList = Currency.RUB.ToSelectList();
        }

        public SelectList CurrencyList { get; set; }

        [Display(Name = "Start date")]
        [Required(ErrorMessage = "Enter currency")]
        public Currency Currency { get; set; }

        [Display(Name = "End date")]
        [Required(ErrorMessage = "Enter start date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Currency")]
        [Required(ErrorMessage = "Enter end date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? EndDate { get; set; }

        public IList<Rate> Rates { get; set; }
        public bool IsSuccesfull { get; set; }
        public string ErrorMessage { get; set; }

        public void GetRates()
        {
            try
            {
                if (!StartDate.HasValue)
                {
                    throw new ApplicationException("Error: Start date must have value");
                }

                if (!EndDate.HasValue)
                {
                    throw new ApplicationException("Error: End date must have value");
                }

                Rates = _exchangeRates.GetRates(Currency, StartDate.Value, EndDate.Value);
                IsSuccesfull = true;
                ErrorMessage = string.Empty;
            }
            catch (ApplicationException ex)
            {
                IsSuccesfull = false;
                ErrorMessage = string.Format(ex.Message);

            }
            catch (Exception ex)
            {
                IsSuccesfull = false;
                ErrorMessage = string.Format("Unexpected error: {0}", ex);

            }
        }

        public string GetDays()
        {
            if (Rates != null)
            {
                string result = string.Empty;
                foreach (var rate in Rates)
                {
                    result += string.Format("'{0}',", rate.Stamp.ToString("yyyy-MM-dd"));
                }
                return result.Substring(0, result.Length - 1);
            }
            return string.Empty;
        }
        
        public string GetRatesData()
        {
            if (Rates != null)
            {
                string result = string.Empty;
                foreach (var rate in Rates)
                {
                    result += string.Format("['{0}',{1}],", rate.Stamp.ToString("yyyy-MM-dd"), rate.Value.ToString("F6").Replace(',','.'));
                }

                return result.Substring(0, result.Length - 1);
            }
            return string.Empty;
        }
    }
}