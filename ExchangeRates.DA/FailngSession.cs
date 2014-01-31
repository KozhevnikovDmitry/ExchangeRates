using System;
using System.Linq;
using ExchangeRates.DA.Exceptions;
using ExchangeRetes.DM;

namespace ExchangeRates.DA
{
    /// <summary>
    /// Session, that represents unaccessible datasource 
    /// </summary>
    internal class FailngSession : ISession
    {
        private readonly Exception _ex;

        /// <summary>
        /// Constructs example of <see cref="FailngSession"/>
        /// </summary>
        /// <param name="ex">Access error exception</param>
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