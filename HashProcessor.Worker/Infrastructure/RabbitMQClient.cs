using HashProcessor.Worker.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace HashProcessor.Worker.Infrastructure
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

        async Task<List<Hash>> IRabbitMqClient.GetMessagesAsync()
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            var result = new ConcurrentBag<Hash>();

            var tasks = Enumerable.Range(0, 4).Select((i) =>
            {
                return Task.Factory.StartNew(async () =>
                {
                    using var channel = await connection.CreateChannelAsync();
                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.ReceivedAsync += async (sender, eventArgs) =>
                    {
                        var body = eventArgs.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        result.Add(new Hash { Date = DateOnly.FromDateTime(DateTime.Now), Sha1 = message });

                        await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
                    };
                    await channel.BasicConsumeAsync(_rabbitMqQueueName, false, consumer);
                });
            }).ToList();

            await Task.WhenAll(tasks);
            return result.ToList();
        }
    }
}
