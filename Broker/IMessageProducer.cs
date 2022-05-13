namespace ChatService.Broker
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T user, T content);
    }
}