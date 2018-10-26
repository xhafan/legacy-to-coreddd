using System;
using CoreDdd.Domain;

namespace CoreDddShared.Domain
{
    public class ShipHistory : Entity
    {
        protected ShipHistory() {}

        public ShipHistory(string name, decimal tonnage)
        {
            Name = name;
            Tonnage = tonnage;
            CreatedOn = DateTime.Now;
        }

        public virtual string Name { get; protected set; }
        public virtual decimal Tonnage { get; protected set; }
        public virtual DateTime CreatedOn { get; protected set; }
    }
}