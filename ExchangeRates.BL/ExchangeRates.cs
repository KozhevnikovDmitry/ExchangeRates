using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRates.BL.Exceptions;
using ExchangeRates.BL.Interface;
using ExchangeRates.DA;
using ExchangeRetes.DM;

namespace ExchangeRates.BL
{
    /// <summary>
    /// Provider of exchange rates data
    /// </summary>
    internal class ExchangeRates : IExchangeRates
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly IRateRepository _rateRepository;
        private readonly IRateService _rateService;

        /// <summary>
        /// Counstruts example of <see cref="ExchangeRates"/>
        /// </summary>
        /// <param name="sessionFactory">Factory of datasource-sessions</param>
        /// <param name="rateRepository">Repository of cached rates</param>
        /// <param name="rateService">Rate remote service</param>
        /// <exception cref="ArgumentNullException"/>
        public ExchangeRates(ISessionFactory sessionFactory, IRateRepository rateRepository, IRateService rateService)
        {
            if (sessionFactory == null)
                throw new ArgumentNullException("sessionFactory");
            if (rateRepository == null)
                throw new ArgumentNullException("rateRepository");
            if (rateService == null)
                throw new ArgumentNullException("rateService");

            ErrorMessage = string.Empty;
            _sessionFactory = sessionFactory;
            _rateRepository = rateRepository;
            _rateService = rateService;
        }

        /// <summary>
        /// Some error information about <see cref="GetRates"/> performing
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Returns ordered rates list for <paramref name="currency"/> and date interval from <paramref name="startDate"/> to <paramref name="endDate"/>
        /// </summary>
        /// <param name="currency">Selected currency</param>
        /// <param name="startDate">Start date of interval inclusively</param>
        /// <param name="endDate">End date of interval inclusively</param>
        /// <exception cref="GetRatesException"/>
        /// <exception cref="EndDateIsEarilerThanStartDateException"/>
        /// <exception cref="SelectedPeriodExceedTwoMonthsException"/>
        public IList<Rate> GetRates(Currency currency, DateTime startDate, DateTime endDate)
        {
            if (endDate.Date > DateTime.Today)
            {
                throw new EndDateIsLaterThanTodayException();
            }

            if (startDate.Date > endDate.Date)
            {
                throw new EndDateIsEarilerThanStartDateException();
            }

            if (MonthDifference(startDate, endDate) > 2)
            {
                throw new SelectedPeriodExceedTwoMonthsException();
            }

            try
            {
                var interval = GetInterval(startDate, endDate);

                using (var session = _sessionFactory.New())
                {
                    var cache = GetCached(currency, startDate, endDate, session);
                    var cachedDays = cache.Select(t => t.Stamp.Date)
                                          .OrderBy(t => t).ToList();
                    if (!interval.SequenceEqual(cachedDays))
                    {
                        var external = interval.Except(cachedDays);
                        var rates = _rateService.GetRates(currency, external);
                        cache = cache.Concat(rates).ToList();
                        CacheRates(session, rates);
                    }
                    return cache.OrderBy(t => t.Stamp).ToList();
                }
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new GetRatesException(ex);
            }
        }

        /// <summary>
        /// Returns list of cached rates
        /// </summary>
        private IList<Rate> GetCached(Currency currency, DateTime startDate, DateTime endDate, ISession session)
        {
            try
            {
                return _rateRepository.GetCached(session, currency, startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return new List<Rate>();
            }
        }

        /// <summary>
        /// Caches list of new rates
        /// </summary>
        private void CacheRates(ISession session, IList<Rate> rates)
        {
            try
            {
                _rateRepository.Cache(session, rates);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        /// <summary>
        /// Returns date array, that represents days of interval from <paramref name="startDate"/> to <paramref name="endDate"/>
        /// </summary>
        private DateTime[] GetInterval(DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, 1 + endDate.Date.Subtract(startDate.Date).Days)
                .Select(offset => startDate.Date.AddDays(offset))
                .ToArray();
        }

        /// <summary>
        /// Return differenc between <paramref name="startDate"/> and <paramref name="endDate"/>
        /// </summary>
        private double MonthDifference(DateTime startDate, DateTime endDate)
        {
            int compMonth = (endDate.Month + endDate.Year * 12) - (startDate.Month + startDate.Year * 12);
            double daysInEndMonth = (endDate - endDate.AddMonths(1)).Days;
            return compMonth + (startDate.Day - endDate.Day) / daysInEndMonth;
        }
    }
}