using RabbitMQ.Client;

namespace HashProcessor.API.Infrastructure
{
    public class RabbitMQClient : IRabbitMqClient
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly string _rabbitMqConnectionString;
        private readonly string _rabbitMqQueueName;

        public RabbitMQClient(IConfiguration config)
        {
            _rabbitMqConnectionString = config.GetValue<string>("RabbitMq:ConnectionString")!;
            _rabbitMqQueueName = config.GetValue<string>("RabbitMq:QueueName")!;

            _connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(_rabbitMqConnectionString)
            };
        }

        public async Task SendMessageAsync(string message)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: _rabbitMqQueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var routingKey = _rabbitMqQueueName;
            var exchangeName = "";
            var props = new BasicProperties();
            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(message);
            await channel.BasicPublishAsync(exchangeName, routingKey, false, props, messageBodyBytes);
        }
    }
}
