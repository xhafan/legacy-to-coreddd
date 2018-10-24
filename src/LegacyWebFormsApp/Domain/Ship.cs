using System.Collections.Generic;
using CoreDdd.Domain;

namespace LegacyWebFormsApp.Domain
{
    public class Ship : Entity, IAggregateRoot
    {
        private readonly ICollection<ShipHistory> _shipHistories = new HashSet<ShipHistory>();

        protected Ship() { } // parameterless constructor needed by nhibernate 
                             // to be able to instantiate the entity when loaded from database

        public Ship(string name, decimal tonnage)
        {
            UpdateData(name, tonnage);
        }

        public virtual string Name { get; protected set; } // virtual modifier needed by nhibernate // - https://stackoverflow.com/a/848116/379279
        public virtual decimal Tonnage { get; protected set; } // protected modifier needed by nhibernate
        public virtual IEnumerable<ShipHistory> ShipHistories => _shipHistories;

        public virtual void UpdateData(string newShipName, decimal tonnage)
        {
            Name = newShipName;
            Tonnage = tonnage;

            _shipHistories.Add(new ShipHistory(this, newShipName, tonnage));
        }
    }
}