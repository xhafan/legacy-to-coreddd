﻿using System;
using System.Collections.Generic;
using CoreDdd.Domain;
using CoreDdd.Domain.Events;
using CoreDddShared.Domain.Events;
#if !NET40
using System.Threading.Tasks;
#endif

namespace CoreDddShared.Domain
{
    public class Ship : Entity, IAggregateRoot
    {
        private readonly ICollection<ShipHistory> _shipHistories = new HashSet<ShipHistory>();

        protected Ship() { } // parameterless constructor needed by nhibernate 
                             // to be able to instantiate the entity when loaded from database

        public Ship(string name, decimal tonnage, string imoNumber)
        {
            UpdateData(name, tonnage);
            ImoNumber = imoNumber;
        }

        public virtual string Name { get; protected set; } // virtual modifier needed by nhibernate // - https://stackoverflow.com/a/848116/379279
        public virtual decimal Tonnage { get; protected set; } // protected modifier needed by nhibernate
        public virtual string ImoNumber { get; protected set; }
        public virtual bool IsImoNumberVerified { get; protected set; }
        public virtual IEnumerable<ShipHistory> ShipHistories => _shipHistories;

        public virtual void UpdateData(string newShipName, decimal tonnage)
        {
            Name = newShipName;
            Tonnage = tonnage;

            _shipHistories.Add(new ShipHistory(newShipName, tonnage));
        }

        public virtual void OnCreationCompleted()
        {
            if (Id == default(int)) throw new Exception("Id has not been assigned yet - entity creation has not been completed yet");

            DomainEvents.RaiseEvent(new ShipCreatedDomainEvent { ShipId = Id });
        }

        public virtual void CheckImoNumber()
        {
        }

#if !NET40
        public virtual async Task VerifyImoNumber(IInternationalMaritimeOrganizationVerifier internationalMaritimeOrganizationVerifier)
        {
            IsImoNumberVerified = await internationalMaritimeOrganizationVerifier.IsImoNumberValid(ImoNumber);
        }
#endif
    }
}