using System.Threading.Tasks;

namespace ServiceApp.WebServiceNotifications
{
    public class ShipCreatedWebServiceNotification : IShipCreatedWebServiceNotification
    {
        public async Task ShipCreatedNotification(int shipId)
        {
            // implement a web service notification informing that a ship was created
        }
    }
}