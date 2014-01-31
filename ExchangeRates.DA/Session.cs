using System;
using System.Data.Entity;
using System.Linq;
using ExchangeRates.DA.Exceptions;
using ExchangeRetes.DM;

namespace ExchangeRates.DA
{
    /// <summary>
    /// EF-dbcontext for ExcnangeRates database
    /// </summary>
    internal class Session : DbContext, ISession
    {
        public Session()
            : base("ExcnangeRates")
        {
            Database.CreateIfNotExists();
        }

        /// <summary>
        /// Register mappings 
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rate>()
                .HasKey(t => new { t.Currency, t.Stamp });
        }

        /// <summary>
        /// Returns query of <see cref="T"/> entities
        /// </summary>
        /// <exception cref="FailToGetDataFromDbExcetpion"/>
        public IQueryable<T> Query<T>() where T : class, IEntity
        {
            try
            {
                return Set<T>();
            }
            catch (Exception ex)
            {
                throw new FailToGetDataFromDbExcetpion(ex);
            }
        }

        /// <summary>
        /// Saves <paramref name="entity"/> data to database, returns saved entity.
        /// </summary>
        /// <exception cref="FailToSetupDataToDbExcetpion"/>
        public T Save<T>(T entity) where T : class, IEntity
        {
            try
            {
                Set<T>().Add(entity);
                SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw new FailToSetupDataToDbExcetpion(ex);
            }
        }
    }
}