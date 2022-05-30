namespace ChatService.Consumers
{
    using MassTransit;

    public class ChatServiceConsumerDefinition :
        ConsumerDefinition<ChatServiceConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ChatServiceConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}