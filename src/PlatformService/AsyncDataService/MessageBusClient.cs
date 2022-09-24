using System.Text;
using System.Text.Json;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataService
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private  IConnection _connection;
        private  IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {           
            _configuration=configuration;
            setupRabbitConnection();
        }

        private void setupRabbitConnection()
        {
             var rabbitMQfactory = new ConnectionFactory()
            {
                HostName=_configuration["RabbitMQHost"],
                Port=int.Parse(_configuration["RabbitMQPort"])
            };
            try
            {
                _connection = rabbitMQfactory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange:"trigger", type:ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                System.Console.WriteLine("--> Connected to the message bus" );

            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("-->Message bus problem");
                throw;
            }
        }



        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            System.Console.WriteLine("-->RabbitMQ ConnectionShutdown" );
        }

        public void PublishNewPlatform(PlatformPublishDto platformPublishDto)
        {
            var message = JsonSerializer.Serialize(platformPublishDto);
            if (_connection.IsOpen)
            {
                SendMessage(message);
                System.Console.WriteLine($"-->RabbitMQ connection open,sending message{message}");
                return;
            }
            System.Console.WriteLine($"-->RabbitMQ connection is closed");
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange:"trigger", 
                routingKey:"",
                basicProperties:null,
                body:body            
            );
            System.Console.WriteLine($"-->We have send a message");
        }
        public void Dispose()
        {
            System.Console.WriteLine($"-->Message bus disposed");
            if (_connection.IsOpen)
            {
                _channel.Close();
                _connection.Close();                
            }
        }

    }

}