using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.RabbitMQProducer
{
    public interface IMessageProducer
    {
        void SendMessageQueue<T>(T message);

    }
}
