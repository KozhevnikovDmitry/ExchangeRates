using System;

namespace ExchangeRetes.DM
{
    public class Rate : IEntity
    {
        public virtual Guid Id { get; set; }

        public virtual Currency Currency { get; set; }

        public virtual DateTime Stamp { get; set; }

        public virtual double Value { get; set; }
    }
}