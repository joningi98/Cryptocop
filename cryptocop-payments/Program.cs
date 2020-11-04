using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq.JObject;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace cryptocop_emails
{
    class Program
    {
        static void Main(string[] args)
        {
            var exchageKey = "cryptocop";
            var routingKey = "create-order";
            var emailQueue = "payment-queue";

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchageKey, type: ExchangeType.Fanout);

                channel.QueueDeclare(queue: emailQueue,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.QueueBind(queue: emailQueue,
                                  exchange: exchageKey,
                                  routingKey: routingKey);
     
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, e) => {
                    var body = e.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    JObject json = JObject.Parse(message);
                    CreditCardValidator.ValidateCard(json.creditCard);
                    Console.WriteLine(message);
                };

                channel.BasicConsume(queue: emailQueue,
                                     autoAck: true,
                                     consumer: consumer);

                
                
                Console.ReadLine();
            }
        }
    }
}
