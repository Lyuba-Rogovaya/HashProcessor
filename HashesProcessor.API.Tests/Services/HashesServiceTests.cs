using HashProcessor.API.Infrastructure;
using HashProcessor.API.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace HashesProcessor.API.Tests.Services
{
    public class HashesServiceTests
    {
        private readonly Mock<IRabbitMqClient> _rabbitMqClientMock;
        private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
        public HashesServiceTests()
        {
            _rabbitMqClientMock = new Mock<IRabbitMqClient>();
            _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        }

        [Fact]
        public async Task SendHash_ShouldCallRabbitMqClient()
        {
            var service = new HashesService(_rabbitMqClientMock.Object, _serviceScopeFactoryMock.Object);

            await service.SendHashAsync("123hash");

            _rabbitMqClientMock.Verify(s => s.SendMessageAsync("123hash"), Times.Once);
        }
    }
}
