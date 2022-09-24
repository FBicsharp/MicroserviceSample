
using System.Text;
using CommandsService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandsService.AsyncDataService
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
         private  IConnection _connection;
        private  IModel _channel;
        private string _queueName;

        public MessageBusSubscriber(IConfiguration configuration,IEventProcessor eventProcessor )
        {
            _configuration=configuration;
            _eventProcessor=eventProcessor;
            InitializeRabbitConnection();
        }

        private void InitializeRabbitConnection()
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
                _queueName = _channel.QueueDeclare().QueueName;
                _channel.QueueBind(
                    queue: _queueName,
                    exchange:"trigger",
                    routingKey:""
                );                
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                System.Console.WriteLine("--> Listening to the message bus" );

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

        public override void Dispose()
        {
            System.Console.WriteLine($"-->Message bus disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();                
            }
            base.Dispose();
        }



        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received+=(ModuleHandle,ea)=>
            {
                System.Console.WriteLine("-->Event recived");
                var body = ea.Body;
                var notificationMessage= Encoding.UTF8.GetString(body.ToArray());
                _eventProcessor.ProcessEvent(notificationMessage);
            };
            _channel.BasicConsume(queue:_queueName, autoAck:true,consumer:consumer);
            return Task.CompletedTask;            
        }
    }

}