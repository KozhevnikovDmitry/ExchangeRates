using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ExchangeRates.BL.Exceptions;
using ExchangeRetes.DM;

namespace ExchangeRates.BL
{
    [DataContract]
    internal class RateResponce
    {
        [DataMember(Name = "disclaimer")]
        public string Disclaimer { get; set; }

        [DataMember(Name = "license")]
        public string License { get; set; }

        [DataMember(Name = "timestamp")]
        public int Stamp { get; set; }

        [DataMember(Name = "base")]
        public string Base { get; set; }

        [DataMember(Name = "rates")]
        public RateDetailResponce RateDetail { get; set; }

        public virtual Rate Parse(Currency currency)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return new Rate
            {
                Stamp = dtDateTime.AddSeconds(Stamp),
                Currency = currency,
                Value = Rates()[currency]
            };
        }

        private Dictionary<Currency, double> Rates()
        {
            if (RateDetail == null)
            {
                throw new RatesWereNotParsedException();
            }

            return new Dictionary<Currency, double>
                {
                    {Currency.RUB, RateDetail.RUB},
                    {Currency.GBP, RateDetail.GBP},
                    {Currency.JPY, RateDetail.JPY},
                    {Currency.EUR, RateDetail.EUR},
                    {Currency.USD, RateDetail.USD}
                };
        }
    }
}