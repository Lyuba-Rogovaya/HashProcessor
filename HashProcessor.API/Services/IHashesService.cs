using HashProcessor.API.Models;

namespace HashProcessor.API.Services
{
    public interface IHashesService
    {
        Task SendHashAsync(string hash);
        Task<List<HashesData>> GetHashesDataAsync();
    }
}
