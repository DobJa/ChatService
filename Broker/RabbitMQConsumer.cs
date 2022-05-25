using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

using ChatService.Model;

namespace ChatService.Broker
{
    public class RabbitMQConsumer : IMessageConsumer
    {
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;

        protected readonly IServiceProvider _serviceProvider;

        public RabbitMQConsumer(IServiceProvider serviceProvider)
        {
            _factory = new ConnectionFactory() { HostName = "rabbitmq" };
            const int retries = 3;
            for (int i = 1; i <= retries; i++)
            {
                try
                {
                    _connection = _factory.CreateConnection();
                    break;
                } catch (BrokerUnreachableException)
                {
                    Console.WriteLine($"[{i}/{retries}] Could not connect to RabbitMQ. Retrying...");
                }
            }
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            System.Console.WriteLine("\nConnected to poop factory as the eater!\n");

            _serviceProvider = serviceProvider;
        }

        public void ReceiveMessage()
        {
            System.Console.WriteLine("\nConnected to poop factory as the eater!\n");

            _channel.QueueDeclare(queue: "chat", durable: false, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, e) => {
                var body = e.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<Message>(json);
                Console.WriteLine(message);
                Console.WriteLine(json);

                var producer = _serviceProvider.GetService<IMessageProducer>();

                producer.SendMessage<Message>(message);
            };

            _channel.BasicConsume(queue: "chat", autoAck: true, consumer: consumer);
        }
    }
}