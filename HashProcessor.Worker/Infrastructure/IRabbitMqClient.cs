using HashProcessor.Worker.Models;
namespace HashProcessor.Worker.Infrastructure
{
    public interface IRabbitMqClient
    {
        Task<List<Hash>> GetMessagesAsync();
    }
}
