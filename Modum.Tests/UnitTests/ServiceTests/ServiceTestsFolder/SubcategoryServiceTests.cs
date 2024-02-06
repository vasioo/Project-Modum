using Modum.Services.Interfaces;

namespace Modum.Tests.UnitTests.ServiceTests.ServiceTestsFolder
{
    public class SubcategoryServiceTests : ServiceTestsBase
    {
        private ISubcategoryService subcategoryService;

        public SubcategoryServiceTests()
        {
            subcategoryService = new SubcategoryService(context);
        }

        [Fact]
        public async Task GetSubcategoriesByCategoryNameAsync_ReturnsCorrectResult()
        {
            // Arrange
            var result = await subcategoryService.GetSubcategoriesByCategoryNameAsync("Test2");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}
