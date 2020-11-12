using Cryptocop.Software.API.Services.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Channels;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class QueueService : IQueueService, IDisposable
    {
        private const string ExchangeKey = "cryptocop";
        private ConnectionFactory _factory;
        private IModel _channel;
        private IConnection _connection;

        public void PublishMessage(string routingKey, object body)
        {
            _factory = new ConnectionFactory(){ HostName = "cryptocop_rabbit" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: ExchangeKey, type: ExchangeType.Fanout);

            var message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body));

            _channel.BasicPublish(exchange: ExchangeKey,
                routingKey: routingKey,
                basicProperties: null,
                body: message);
                
            Console.WriteLine("RabbitMQ message sent");
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
            Console.WriteLine("Dispose");
        }
    }
}