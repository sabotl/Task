
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using Task.Domain.Enums;
using Task.Infrastructure.Options;

namespace Task.Application.Services
{
    public class RabbitMqLogger : Interfaces.ILoggerService
    {
        private readonly Infrastructure.Options.RabbitMqOptions _options;

        public RabbitMqLogger(IOptions<RabbitMqOptions> options)
        {
            _options = options.Value;
        }

        public void Log(string message, string IpAddress, LogLevel logLevel)
        {
            var factory = new ConnectionFactory() { 
                HostName = _options.HostName,
                Port = _options.Port,
                Password = _options.Password,
                UserName = _options.UserName,
            };

            using (var connection =  factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _options.QueueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {IpAddress}: \"{message}\"";
                var body = Encoding.UTF8.GetBytes(logMessage);

                channel.BasicPublish(exchange: "",
                                     routingKey: _options.QueueName,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
