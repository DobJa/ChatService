namespace ChatService.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;

    public class ChatServiceConsumer :
        IConsumer<AddMessage>
    {
        public async Task Consume(ConsumeContext<AddMessage> context)
        {
            await context.Publish<MessageAdded>(new
            {
                Message = context.Message.Message
            });
        }
    }
}