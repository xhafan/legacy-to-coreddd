using System;
using CoreDdd.Domain;

namespace AspNetCoreMvcApp.Domain
{
    public class ShipHistory : Entity
    {
        protected ShipHistory() {}

        public ShipHistory(Ship ship, string name, decimal tonnage)
        {
            Ship = ship;
            Name = name;
            Tonnage = tonnage;
            CreatedOn = DateTime.Now;
        }

        public virtual Ship Ship { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual decimal Tonnage { get; protected set; }
        public virtual DateTime CreatedOn { get; protected set; }
    }
}