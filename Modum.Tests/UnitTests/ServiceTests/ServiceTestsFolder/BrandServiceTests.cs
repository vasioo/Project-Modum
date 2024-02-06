using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Services.Interfaces;
using Modum.Services.Services;

namespace Modum.Tests.UnitTests.ServiceTests.ServiceTestsFolder
{
    public class BrandServiceTests : ServiceTestsBase
    {
        private IBrandService brandService;

        public BrandServiceTests()
        {
            brandService = new BrandService(context);
        }
        [Fact]
        public async Task GetBrandByStringName_ReturnsCorrectBrand()
        {
            // Arrange
            var brandName = "TestBrand";
            var brandToAdd = new Brands { BrandName = brandName };
            await context.Brands.AddAsync(brandToAdd);
            await context.SaveChangesAsync();

            // Act
            var result = await brandService.GetBrandByStringName(brandName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(brandName, result.BrandName);
        }

        [Fact]
        public async Task GetBrandByStringName_ReturnsNullForNonexistentBrand()
        {
            // Arrange
            var nonExistentBrandName = "NonExistentBrand";

            // Act
            var result = await brandService.GetBrandByStringName(nonExistentBrandName);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetMostFavouriteBrandsBySoldItemsBySection_ReturnsCorrectBrands()
        {
            // Arrange
            var section = "Electronics";
            var brand1 = new Brands { TypeOfBrand = section, AmountOfTimesInFavourites = 30 };
            var brand2 = new Brands { TypeOfBrand = section, AmountOfTimesInFavourites = 25 };
            var brand3 = new Brands { TypeOfBrand = "Clothing", AmountOfTimesInFavourites = 20 };

            await context.Brands.AddRangeAsync(brand1, brand2, brand3);
            await context.SaveChangesAsync();

            // Act
            var result = await brandService.GetMostFavouriteBrandsBySoldItemsBySection(section);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetMostFavouriteBrandsBySoldItemsBySection_ReturnsEmptyListForNonexistentSection()
        {
            // Arrange
            var nonExistentSection = "NonExistentSection";

            // Act
            var result = await brandService.GetMostFavouriteBrandsBySoldItemsBySection(nonExistentSection);

            // Assert
            Assert.Empty(result);
        }
    }
}
