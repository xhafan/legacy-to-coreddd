using System.Threading.Tasks;
using CoreDddShared.Domain.Events;
using FakeItEasy;
using NUnit.Framework;
using ServiceApp.WebServiceNotifications;

namespace ServiceApp.Tests
{
    [TestFixture]
    public class when_handling_ship_created_event_message
    {
        [Test]
        public async Task web_service_is_notified()
        {
            var webServiceNotification = A.Fake<IShipCreatedWebServiceNotification>();
            var eventMessageHandler = new ShipCreatedDomainEventMessageHandler(webServiceNotification);

            await eventMessageHandler.Handle(new ShipCreatedDomainEventMessage {ShipId = 23});

            A.CallTo(() => webServiceNotification.ShipCreatedNotification(23)).MustHaveHappened();
        }
    }
}
