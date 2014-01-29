using System.Data.Entity;
using System.Linq;
using ExchangeRetes.DM;

namespace ExchangeRates.DA
{
    internal class Session : DbContext, ISession
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rate>()
                .HasKey(t => new { t.Currency, t.Stamp });
        }

        public IQueryable<T> Query<T>() where T : class, IEntity
        {
            return Set<T>();
        }

        public T Save<T>(T entity) where T : class, IEntity
        {
           return Set<T>().Add(entity);
        }
    }
}