using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ExchangeRates.BL.Exceptions;
using ExchangeRetes.DM;

namespace ExchangeRates.BL
{
    /// <summary>
    /// Data contract for rates web-service, that provides rates for a date <see cref="Stamp"/>
    /// </summary>
    /// <remarks>
    /// Also provides <see cref="Rate"/> entity by deserialized data
    /// </remarks>
    [DataContract]
    internal class RateSourceData
    {
        /// <summary>
        /// Unix-timestamp in seconds
        /// </summary>
        [DataMember(Name = "timestamp")]
        public int Stamp { get; set; }

        [DataMember(Name = "rates")]
        public RateValuesData RatesValues { get; set; }

        /// <summary>
        /// Returns rate for <paramref name="currency"/> 
        /// </summary>
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
            if (RatesValues == null)
            {
                throw new RatesWereNotParsedException();
            }

            return new Dictionary<Currency, double>
                {
                    {Currency.RUB, RatesValues.RUB},
                    {Currency.GBP, RatesValues.GBP},
                    {Currency.JPY, RatesValues.JPY},
                    {Currency.EUR, RatesValues.EUR},
                    {Currency.USD, RatesValues.USD}
                };
        }
    }

    /// <summary>
    /// Data contract for rates web-service, that provides rates values
    /// </summary>
    [DataContract]
    internal class RateValuesData
    {
        [DataMember]
        public double RUB { get; set; }

        [DataMember]
        public double GBP { get; set; }

        [DataMember]
        public double JPY { get; set; }

        [DataMember]
        public double USD { get; set; }

        [DataMember]
        public double EUR { get; set; }
    }
}