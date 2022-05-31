namespace ChatService.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;

    public class ChatServiceConsumer :
        IConsumer<AddMessage>, IConsumer<DeleteMessage>
    {
        public async Task Consume(ConsumeContext<AddMessage> context)
        {
            await context.Publish<MessageAdded>(new
            {
                Message = context.Message.Message
            });
        }

        public async Task Consume(ConsumeContext<DeleteMessage> context)
        {
            await context.Publish<MessageDeleted>(new
            {
                MessId = context.Message.MessId
            });
        }
    }
}