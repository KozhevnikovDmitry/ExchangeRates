using System.Runtime.Serialization;

namespace ExchangeRates.BL
{
    [DataContract]
    internal class RateDetailResponce
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