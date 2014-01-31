using System;
using System.Linq;
using ExchangeRetes.DM;

namespace ExchangeRates.DA
{
    /// <summary>
    /// Session of datasource
    /// </summary>
    public interface ISession : IDisposable
    {
        IQueryable<T> Query<T>() where T : class, IEntity;

        T Save<T>(T entity) where T : class, IEntity;
    }
}