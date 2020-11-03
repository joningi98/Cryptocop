using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class QueueService : IQueueService, IDisposable
    {
        private byte[] ObjectToByte(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using(MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public void PublishMessage(string routingKey, object body)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "email_queue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var bodyJson = JsonConvert.SerializeObject(body, Formatting.Indented); 
                var message = new Buffer(bodyJson);          
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                                     routingKey: routingKey,
                                     basicProperties: properties,
                                     body: new Buffer(bodyJson),
                                     mandatory: false);
                
                Console.WriteLine("RabbitMQ message sent");
            }
        }

        public void Dispose()
        {
            // TODO: Dispose the connection and channel
            throw new NotImplementedException();
        }
    }
}