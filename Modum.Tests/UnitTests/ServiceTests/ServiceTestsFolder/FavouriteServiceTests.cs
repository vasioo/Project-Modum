using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Services.Interfaces;
using Modum.Services.Services;

namespace Modum.Tests.UnitTests.ServiceTests.ServiceTestsFolder
{
    public class FavouriteServiceTests : ServiceTestsBase
    {
        private IFavouriteService favouritesService;

        public FavouriteServiceTests()
        {
            favouritesService = new FavouriteService(context);
        }

        [Fact]
        public async Task GetFavouritesContainerByUserId_ReturnsCorrectFavouritesContainer()
        {
            // Arrange
            var userId = "TestUser";
            var favouritesContainerToAdd = new Favourites { UserId = userId };
            await context.Favourites.AddAsync(favouritesContainerToAdd);
            await context.SaveChangesAsync();

            // Act
            var result = await favouritesService.GetFavouritesContainerByUserId(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task GetFavouritesContainerByUserId_ReturnsNullForNonexistentUser()
        {
            // Arrange
            var nonExistentUserId = "NonExistentUser";

            // Act
            var result = await favouritesService.GetFavouritesContainerByUserId(nonExistentUserId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetFavouritesIdByUserId_ReturnsCorrectFavouritesId()
        {
            // Arrange
            var userId = "TestUser";
            var favouritesContainerToAdd = new Favourites { UserId = userId };
            await context.Favourites.AddAsync(favouritesContainerToAdd);
            await context.SaveChangesAsync();

            // Act
            var result = await favouritesService.GetFavouritesIdByUserId(userId);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            Assert.Equal(favouritesContainerToAdd.Id, result);
        }

        [Fact]
        public async Task GetFavouritesIdByUserId_ReturnsZeroForNonexistentUser()
        {
            // Arrange
            var nonExistentUserId = "NonExistentUser";

            // Act
            var result = await favouritesService.GetFavouritesIdByUserId(nonExistentUserId);

            // Assert
            Assert.Equal(Guid.Empty, result);
        }
    }
}
