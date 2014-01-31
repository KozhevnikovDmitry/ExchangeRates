using System;
using System.Linq;
using ExchangeRates.DA.Exceptions;
using ExchangeRetes.DM;

namespace ExchangeRates.DA
{
    internal class FailngSession : ISession
    {
        private readonly Exception _ex;

        public FailngSession(Exception ex)
        {
            _ex = ex;
        }

        public void Dispose()
        {
            
        }

        public IQueryable<T> Query<T>() where T : class, IEntity
        {
            throw new FailToCreateSessionExcetpion(_ex);
        }

        public T Save<T>(T entity) where T : class, IEntity
        {
            throw new FailToCreateSessionExcetpion(_ex);
        }
    }
}