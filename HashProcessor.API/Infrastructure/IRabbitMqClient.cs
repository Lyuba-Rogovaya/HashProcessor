namespace HashProcessor.API.Infrastructure
{
    public interface IRabbitMqClient
    {
        Task SendMessageAsync(string message);
    }
}
