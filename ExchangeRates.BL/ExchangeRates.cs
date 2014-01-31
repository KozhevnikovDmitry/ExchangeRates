using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRates.BL.Exceptions;
using ExchangeRates.BL.Interface;
using ExchangeRates.DA;
using ExchangeRetes.DM;

namespace ExchangeRates.BL
{
    internal class ExchangeRates : IExchangeRates
    {
        private readonly ISessionFactory _sessionFacotry;
        private readonly IRateRepository _repository;
        private readonly IRateService _service;

        public ExchangeRates(ISessionFactory sessionFacotry, IRateRepository repository, IRateService service)
        {
            if (sessionFacotry == null)
                throw new ArgumentNullException("sessionFacotry");
            if (repository == null)
                throw new ArgumentNullException("repository");
            if (service == null)
                throw new ArgumentNullException("service");

            ErrorMessage = string.Empty;
            _sessionFacotry = sessionFacotry;
            _repository = repository;
            _service = service;
        }

        public string ErrorMessage { get; private set; }

        public IList<Rate> GetRates(Currency currency, DateTime startDate, DateTime endDate)
        {
            if (startDate.Date > endDate.Date)
            {
                throw new EndDateIsEarilerThanStartDateException();
            }

            if (MonthDifference(startDate, endDate) > 2)
            {
                throw new SelectedPeriodExceedTwoMonthsException();
            }

            var interval = GetInterval(startDate, endDate);

            try
            {
                using (var session = _sessionFacotry.New())
                {
                    var cache = GetCached(currency, startDate, endDate, session);
                    var cachedDays = cache.Select(t => t.Stamp.Date)
                                          .OrderBy(t => t).ToList();
                    if (!interval.SequenceEqual(cachedDays))
                    {
                        var external = interval.Except(cachedDays);
                        var rates = _service.GetRates(currency, external);
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

        private IList<Rate> GetCached(Currency currency, DateTime startDate, DateTime endDate, ISession session)
        {
            try
            {
                return _repository.GetCached(session, currency, startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return new List<Rate>();
            }
        }

        private void CacheRates(ISession session, IList<Rate> rates)
        {
            try
            {
                _repository.Cache(session, rates);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private DateTime[] GetInterval(DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, 1 + endDate.Date.Subtract(startDate.Date).Days)
                .Select(offset => startDate.Date.AddDays(offset))
                .ToArray();
        }

        public double MonthDifference(DateTime start, DateTime end)
        {
            int compMonth = (end.Month + end.Year * 12) - (start.Month + start.Year * 12);
            double daysInEndMonth = (end - end.AddMonths(1)).Days;
            return compMonth + (start.Day - end.Day) / daysInEndMonth;
        }
    }
}