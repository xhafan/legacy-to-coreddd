using System.Threading.Tasks;

namespace AspNetCoreMvcApp.BusRequestSenders
{
    public interface IBusRequestSender
    {
        Task<TReply> SendRequest<TReply>(object message);
    }
}