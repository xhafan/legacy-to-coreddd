using System.Threading.Tasks;

namespace ServiceApp.WebServiceNotifications
{
    public interface IShipCreatedWebServiceNotification
    {
        Task ShipCreatedNotification(int shipId);
    }
}