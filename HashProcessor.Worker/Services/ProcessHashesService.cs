using HashProcessor.Worker.Infrastructure;

namespace HashProcessor.Worker.Services
{
    public class ProcessHashesService : IProcessHashesService
    {
        private readonly IRabbitMqClient _rabbitMqClient;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public ProcessHashesService(IRabbitMqClient rabbitMqClient, IServiceScopeFactory serviceScopeFactory)
        {
            _rabbitMqClient = rabbitMqClient;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ProcessHashesAsync()
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<MariaDbContext>();

            var messages = await _rabbitMqClient.GetMessagesAsync();
            foreach (var message in messages)
            {
                await dbContext.Hashes.AddAsync(message);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
