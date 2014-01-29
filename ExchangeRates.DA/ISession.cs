using System;
using System.Linq;

namespace ExchangeRates.DA
{
    public interface ISession : IDisposable
    {
        IQueryable<T> Query<T>();

        T Save<T>(T entity);
    }
}