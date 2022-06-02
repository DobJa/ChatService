namespace ChatService.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using Services;
    using ChatService.Models;

    public class ChatServiceConsumer :
        IConsumer<AddMessage>, IConsumer<DeleteMessage>
    {
        private readonly MessagesService _messagesService;

        public ChatServiceConsumer(MessagesService messagesService) =>
            _messagesService = messagesService;

        public async Task Consume(ConsumeContext<AddMessage> context)
        {
            Message message = new Message
            {
                MessId = context.Message.Message.MessId,
                User = context.Message.Message.User,
                Text = context.Message.Message.Text,
                image = context.Message.Message.image,
                mid = context.Message.Message.mid,
                Timestamp = context.Message.Message.Timestamp
            };

            await _messagesService.CreateAsync(message);

            await context.Publish<MessageAdded>(new
            {
                Message = context.Message.Message
            });
        }

        public async Task Consume(ConsumeContext<DeleteMessage> context)
        {

            Message message = await _messagesService.GetAsync(context.Message.MessId);
            Message modifiedMessage = new Message
            {
                MessId = message.MessId,
                User = message.User,
                Text = "<this message has been deleted>",
                image = null,
                mid = message.mid,
                Timestamp = message.Timestamp
            };

            await _messagesService.UpdateAsync(context.Message.MessId, modifiedMessage);

            await context.Publish<MessageDeleted>(new
            {
                MessId = context.Message.MessId
            });
        }
    }
}