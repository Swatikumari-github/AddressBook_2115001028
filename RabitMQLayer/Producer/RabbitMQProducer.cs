using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
namespace RabitMQLayer.Producer
{
    public class RabbitMQProducer
    {
        private readonly IModel _channel;

        public RabbitMQProducer(RabbitMQService rabbitMQService)
        {
            _channel = rabbitMQService.GetChannel();
        }

        // Publish a message to the specified queue
        public void Publish(string queueName, string message)
        {
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

            Console.WriteLine($"Message Published to {queueName}: {message}");
        }
    }
}
