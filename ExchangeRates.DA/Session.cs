using System;
using System.Data.Entity;
using System.Linq;
using ExchangeRates.DA.Exceptions;
using ExchangeRetes.DM;

namespace ExchangeRates.DA
{
    internal class Session : DbContext, ISession
    {
        public Session()
            : base("ExcnangeRates")
        {
            Database.CreateIfNotExists();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rate>()
                .HasKey(t => new { t.Currency, t.Stamp });
        }

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