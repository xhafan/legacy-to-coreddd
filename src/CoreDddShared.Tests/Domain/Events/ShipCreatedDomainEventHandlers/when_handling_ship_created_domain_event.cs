#if NETCOREAPP // ShipCreatedDomainEventHandler is only in AspNetCoreMvcApp and not in LegacyWebFormsApp
using CoreDddShared.Domain.Events;
using FakeItEasy;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using Rebus.Bus.Advanced;

namespace CoreDddShared.Tests.Domain.Events.ShipCreatedDomainEventHandlers
{
    [TestFixture]
    public class when_handling_ship_created_domain_event
    {
        [Test]
        public void message_is_published_to_the_bus()
        {
            var bus = A.Fake<ISyncBus>();
            var handler = new ShipCreatedDomainEventHandler(bus);

            handler.Handle(new ShipCreatedDomainEvent { ShipId = 23 });

            A.CallTo(() => bus.Publish(A<object>.That.Matches(p => _MatchingEventMessage(p)), A<Dictionary<string, string>>._)).MustHaveHappened();
        }

        private bool _MatchingEventMessage(object eventMessage)
        {
            eventMessage.ShouldBeOfType<ShipCreatedDomainEventMessage>();
            var shipCreatedDomainEventMessage = (ShipCreatedDomainEventMessage)eventMessage;
            shipCreatedDomainEventMessage.ShipId.ShouldBe(23);
            return true;
        }
    }
}
#endif