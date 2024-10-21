using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace EmailConsumer
{
    public class RegistrationEmailConsumer
    {
        private readonly IModel _chanel;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        private readonly string _host = Environment.GetEnvironmentVariable("RabbitMQ_Host")??"localhost";
        private readonly string _username = Environment.GetEnvironmentVariable("RabbitMQ_Username") ?? "guest";
        private readonly string _password = Environment.GetEnvironmentVariable("RabbitMQ_Password") ?? "guest";
        private readonly string _virtualhost = Environment.GetEnvironmentVariable("RabbitMQ_Virtualhost") ?? "/";

        public RegistrationEmailConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;

            var factory = new ConnectionFactory()
            {
                HostName = _host,
                UserName = _username,
                Password = _password,
            };
            var connection = factory.CreateConnection();
            _chanel = connection.CreateModel();
        }

        public void SendEmailForRegistration()
        {
            _chanel.QueueDeclare(queue: _configuration["RabbitMQ:QueueName"],
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

            var consumer = new EventingBasicConsumer(_chanel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
                var email = ExtractEmailFromMessage(message);
                if (!string.IsNullOrEmpty(email))
                {
                    _emailService.SendEmailForRegistration(email, "You are successfully register to Rekreacija application");
                }
            };
            _chanel.BasicConsume(queue: _configuration["RabbitMQ:QueueName"],
                             autoAck: true,
                             consumer: consumer);
        }
        private string ExtractEmailFromMessage(string message)
        {
            var parts = message.Split(' ');
            return parts.Length > 3 ? parts[3] : string.Empty;
        }
    }
}
