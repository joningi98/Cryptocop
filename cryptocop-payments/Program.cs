using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace cryptocop_payments
{
    class Program
    {
        static void Main(string[] args)
        {
            const string exchangeKey = "cryptocop";
            const string routingKey = "create-order";
            const string paymentQueue = "payment-queue";

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            
            channel.ExchangeDeclare(exchange: exchangeKey, type: ExchangeType.Fanout);

            channel.QueueDeclare(queue: paymentQueue,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            
            channel.QueueBind(queue: paymentQueue,
                              exchange: exchangeKey,
                              routingKey: routingKey);
     
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) => {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var json = JObject.Parse(message);
                
                Console.WriteLine(json["CreditCard"]?.ToString());
                CreditCardValidator.ValidateCard(json["CreditCard"]?.ToString());
            };

            channel.BasicConsume(queue: paymentQueue,
                                 autoAck: true,
                                 consumer: consumer);
            
            Console.ReadLine();
        }
    }
}
