using HashProcessor.API.Models;
using HashProcessor.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace HashProcessor.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HashesController : ControllerBase
    {
        private readonly IHashesService _hashesService;

        public HashesController(IHashesService hashesService)
        {
            _hashesService = hashesService;
        }

        [HttpGet]
        public async Task<List<HashesData>> Get()
        {
            return await _hashesService.GetHashesDataAsync();
        }

        [HttpPost]
        public async void Post()
        {
            for (var i = 0; i < 4000; i++)
            {
                var randomString = DateTime.Now.ToString();
                var hash = Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(randomString)));
                await _hashesService.SendHashAsync(hash);
            }
        }
    }
}
