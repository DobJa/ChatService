using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ChatService.Broker
{
    public class RabbitMQConsumer
    {
        protected readonly ConnectionFactory _factory;
        protected readonly IConnection _connection;
        protected readonly IModel _channel;

        protected readonly IServiceProvider _serviceProvider;

        public RabbitMQConsumer(IServiceProvider serviceProvider)
        {
            _factory = new ConnectionFactory() { HostName = "rabbitmq" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            System.Console.WriteLine("\nConnected to poop factory as the eater!\n");

            _serviceProvider = serviceProvider;
        }

        public void ReceiveMessage<T>(T message)
        {
            System.Console.WriteLine("\nConnected to poop factory as the eater!\n");

            _channel.QueueDeclare(queue: "chat", durable: false, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);

                        consumer.Received += (sender, e) => {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var json = JsonConvert.DeserializeObject(message);
                Console.WriteLine(message);
                Console.WriteLine(json);
            };

            _channel.BasicConsume(queue: "chat", autoAck: true, consumer: consumer);
        }
    }
}