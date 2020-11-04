using Cryptocop.Software.API.Services.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class QueueService : IQueueService, IDisposable
    {
        private readonly string exchageKey = "cryptocop";
        public void PublishMessage(string routingKey, object body)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchageKey, type: ExchangeType.Fanout);

                var message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body));

                channel.BasicPublish(exchange: exchageKey,
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: message);
            

                //Console.WriteLine("RabbitMQ message sent");
            }
        }

        public void Dispose()
        {
            Console.WriteLine("Dispose");
        }
    }
}