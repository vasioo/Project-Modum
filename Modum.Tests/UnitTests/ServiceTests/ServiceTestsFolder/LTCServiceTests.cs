using Microsoft.Extensions.Configuration;
using Modum.Services.Interfaces;
using Modum.Services.Services;

namespace Modum.Tests.UnitTests.ServiceTests.ServiceTestsFolder
{
    public class LTCServiceTests : ServiceTestsBase
    {
        private ILTCService ltcService;
        private IConfiguration configurationMock;

        public LTCServiceTests()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection();

            configurationMock = configurationBuilder.Build();

            ltcService = new LTCService(context, configurationMock);
        }
        [Fact]
        public async Task GetBestLTCNow_ReturnsCorrectResult()
        {
            // Act
            var result = await ltcService.GetBestLTCNow();

            // Assert
            Assert.NotNull(result);
        }
    }
}
