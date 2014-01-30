using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using ExchangeRates.DA;
using ExchangeRetes.DM;

namespace ExchangeRates.Tests
{
    public class IntegrationSession : DbContext, ISession
    {
        public IntegrationSession(DbConnection connection)
            : base(connection, true)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rate>()
                .HasKey(t => new { t.Currency, t.Stamp });
        }

        public IQueryable<T> Query<T>() where T : class, IEntity
        {
            return Set<T>().Local.AsQueryable();
        }

                
        public T Save<T>(T entity) where T : class, IEntity
        {
            Set<T>().Add(entity);
            SaveChanges();
            return entity;
        }

        public void Dispose()
        {
            
        }

        public void DoDispose()
        {
            base.Dispose();
        }
    }
}