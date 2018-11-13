using System;
using System.Threading.Tasks;
using Rebus;
using Rebus.Bus;

namespace AspNetCoreMvcApp.BusRequestSenders
{
    public class BusRequestSender : IBusRequestSender
    {
        private readonly IBus _bus;
        private readonly double _timeoutInSeconds;

        public BusRequestSender(IBus bus, double timeoutInSeconds = 30)
        {
            _timeoutInSeconds = timeoutInSeconds;
            _bus = bus;
        }

        public async Task<TReply> SendRequest<TReply>(object message)
        {
            return await _bus.SendRequest<TReply>(message, timeout: TimeSpan.FromSeconds(_timeoutInSeconds));
        }
    }
}