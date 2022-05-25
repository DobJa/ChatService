using ChatService.Model;

namespace ChatService.Broker
{
    public interface IMessageProducer
    {
        void SendMessage<T>(Message msg);
    }
}