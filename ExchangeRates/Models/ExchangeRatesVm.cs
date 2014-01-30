using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
            Currency = Currency.RUB;
            StartDate = DateTime.Today.AddDays(-10);
            EndDate = DateTime.Today;
            CurrencyList = Currency.RUB.ToSelectList();
        }

        public SelectList CurrencyList { get; set; }

        [Required(ErrorMessage = "Enter currency")]
        public Currency Currency { get; set; }

        [Required(ErrorMessage = "Enter start date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Enter end date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? EndDate { get; set; }

        public IList<Rate> Rates { get; set; }

        public void GetRates()
        {
            Rates = _exchangeRates.GetRates(Currency, StartDate.Value, EndDate.Value);
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
                    result += string.Format("['{0}',{1}],", rate.Stamp.ToString("yyyy-MM-dd"), rate.Value.ToString("F").Replace(',','.'));
                }

                return result.Substring(0, result.Length - 1);
            }
            return string.Empty;
        }
    }

    public static class EnumExtensions
    {
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj)
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = e, Name = e.ToString() };
            return new SelectList(values, "Id", "Name", enumObj);
        }
    }
}