using EmailService.Model;
using EmailService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.RabbitMQReceiver
{
    public class MessageReceiver
    {
        ConnectionFactory factory;
        MailSetting mailSetting;
        public string Message { get; set; }

        public MessageReceiver(ConnectionFactory _factory, MailSetting _mailSetting)
        {
            factory = _factory;
            mailSetting = _mailSetting;
            ListenToQueue();
        }

        void ListenToQueue()
        {
            try
            {
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                //channel.QueueDeclare(queue: "", exclusive: true, autoDelete: true);
                channel.BasicQos(0, 1, false);
                List<Object> msg = new List<object>();

                var receiver = new EventingBasicConsumer(channel);
                receiver.Received += (model, eventArgs) =>
                {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    msg.Add(message);
                    Console.WriteLine(message);
                };

                //Receiver receiver = new Receiver(channel);
                channel.BasicConsume(queue: "ProductItem", autoAck: true, consumer: receiver);

                IEmailService e = new Services.EmailService(mailSetting);
                List<string> to = new List<string>();
                to.Add("kirellos_rashad@hotmail.com");
                List<string> cc = new List<string>();
                List<string> bcc = new List<string>();
                e.SendEmail(to, cc, bcc, "New item added", msg.Select(x => x.ToString()).FirstOrDefault().ToString());
                Message = msg.Select(x => x.ToString()).FirstOrDefault().ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
