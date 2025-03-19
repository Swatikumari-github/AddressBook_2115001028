using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabitMQLayer.Consumer
{
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly IModel _channel;
        private readonly string _queueName = "default_queue";

        public RabbitMQConsumer(RabbitMQService rabbitMQService, string queueName = "default_queue")
        {
            _channel = rabbitMQService.GetChannel();
            _queueName = queueName;

            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Message Received from {_queueName}: {message}");

                // Acknowledge message after processing
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Close();
            base.Dispose();
        }
    }
}
