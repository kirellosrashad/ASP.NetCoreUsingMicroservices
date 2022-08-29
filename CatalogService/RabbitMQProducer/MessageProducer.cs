using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.RabbitMQProducer
{
    public class MessageProducer : IMessageProducer
    {
        public void SendMessageQueue<T>(T message)
        {
            try
            {
                var factory = new ConnectionFactory { HostName = "localhost" };
                var connection = factory.CreateConnection();     // create a connection to the localhost server
                using var channel = connection.CreateModel();  // create a channel to interact with the RabbitMQ APIs

                channel.ExchangeDeclare("ProductExchange", ExchangeType.Direct);
                channel.QueueDeclare("ProductItem", true, false, true, null);
                channel.QueueBind("ProductItem", "ProductExchange", "directexchange_key");

                var properties = channel.CreateBasicProperties();
                properties.Persistent = false;

                // plian text isn't allowed in RabbitMQ
                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(exchange: "ProductExchange", routingKey: "directexchange_key", basicProperties: properties, body: body);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        
    }
}
