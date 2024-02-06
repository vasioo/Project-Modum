using Modum.Services.Interfaces;
using Modum.Services.Services;

namespace Modum.Tests.UnitTests.ServiceTests.ServiceTestsFolder
{
    public class AdServiceTests : ServiceTestsBase
    {
        private IAdsService adsService;

        public AdServiceTests()
        {
            adsService = new AdsService(context);
        }

        [Fact]
        public async Task GetProductsByTenMostBought_ReturnsCorrectProducts()
        {
            var result = await adsService.GetProductsByTenMostBought();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Count());
        }

        [Fact]
        public async Task GetProductsByTenMostAddedToFavourites_ReturnsCorrectProducts()
        {
            // Act
            var result = await adsService.GetProductsByTenMostAddedToFavourites();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Count());

            var orderedProducts = result.OrderByDescending(p => p.AmountOfTimesInFavourites).ToList();
            for (int i = 0; i < orderedProducts.Count - 1; i++)
            {
                Assert.True(orderedProducts[i].AmountOfTimesInFavourites >= orderedProducts[i + 1].AmountOfTimesInFavourites);
            }
        }

    }
}
