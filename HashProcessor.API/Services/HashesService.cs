using HashProcessor.API.Infrastructure;
using HashProcessor.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HashProcessor.API.Services
{
    public class HashesService : IHashesService
    {
        private readonly IRabbitMqClient _rabbitMqClient;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public HashesService(IRabbitMqClient rabbitMqClient, IServiceScopeFactory serviceScopeFactory)
        {
            _rabbitMqClient = rabbitMqClient;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<List<HashesData>> GetHashesDataAsync()
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<MariaDbContext>();

            return await dbContext
                .Hashes
                .GroupBy(h => h.Date)
                .Select(g => new HashesData { Date = g.Key, Count = g.Count() })
                .ToListAsync();
        }

        public async Task SendHashAsync(string hash)
        {
            await _rabbitMqClient.SendMessageAsync(hash);
        }
    }
}
