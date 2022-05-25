using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

using ChatService.Model;

namespace ChatService.Broker
{
    public class RabbitMQProducer : IMessageProducer
    {
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;

        protected readonly IServiceProvider _serviceProvider;

        public RabbitMQProducer(IServiceProvider serviceProvider)
        {
            _factory = new ConnectionFactory() { HostName = "rabbitmq" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            System.Console.WriteLine("\nConnected to the poop factory (2)!\n");

            _serviceProvider = serviceProvider;
        }

        public void SendMessage<T>(Message message)
        {
            System.Console.WriteLine("\nConnected to the poop factory (2)!\n");

            _channel.QueueDeclare(queue: "chat2", durable: false, exclusive: false, autoDelete: false);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(exchange: "", routingKey: "chat2", body: body);
            System.Console.WriteLine($"Sent message: {message} to broker\n");
        }
    }
}