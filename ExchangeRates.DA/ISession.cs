using System;
using System.Linq;
using ExchangeRetes.DM;

namespace ExchangeRates.DA
{
    public interface ISession : IDisposable
    {
        IQueryable<T> Query<T>() where T : class, IEntity;

        T Save<T>(T entity) where T : class, IEntity;
    }
}