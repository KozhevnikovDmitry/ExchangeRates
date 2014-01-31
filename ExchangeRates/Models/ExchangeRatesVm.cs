using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ExchangeRates.BL.Interface;
using ExchangeRetes.DM;

namespace ExchangeRates.Models
{
    /// <summary>
    /// ViewModel, that provides exchange rates reports data by date interval and currency
    /// </summary>
    public class ExchangeRatesVm
    {
        /// <summary>
        /// Provider of exchange rates data
        /// </summary>
        private readonly IExchangeRates _exchangeRates;

        /// <summary>
        /// Constructs example of <see cref="ExchangeRatesVm"/>
        /// </summary>
        /// <param name="exchangeRates">Provider of exchange rates data</param>
        /// <exception cref="ArgumentNullException"/>
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
        

        #region Input

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
        
        #endregion


        #region Output

        /// <summary>
        /// Result rate list
        /// </summary>
        public IList<Rate> Rates { get; set; }

        /// <summary>
        /// Returns true if <see cref="GetRates"/> was performed without error
        /// </summary>
        public bool IsSuccesfull { get; set; }

        /// <summary>
        /// Some error information about <see cref="GetRates"/> performing
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Provides rate list to the <see cref="Rates"/> using input <see cref="StartDate"/>, <see cref="EndDate"/> and <see cref="Currency"/>
        /// </summary>
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

                // Getting rates here
                Rates = _exchangeRates.GetRates(Currency, StartDate.Value, EndDate.Value);

                if (string.IsNullOrEmpty(_exchangeRates.ErrorMessage))
                {
                    IsSuccesfull = true;
                    ErrorMessage = string.Empty;
                }
                else
                {
                    IsSuccesfull = false;
                    ErrorMessage = _exchangeRates.ErrorMessage;
                }
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

        /// <summary>
        /// Returns strigified view of input date interval days.
        /// </summary>
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

        /// <summary>
        /// Returns strigified view of result rates.
        /// </summary>
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

        #endregion
    }
}