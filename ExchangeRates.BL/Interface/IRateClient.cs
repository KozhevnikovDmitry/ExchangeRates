using System;

namespace ExchangeRates.BL.Interface
{
    internal interface IRateClient
    {
        RateResponce GetRate(DateTime date);
    }
}