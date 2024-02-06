using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Services.Interfaces;
using Modum.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modum.Tests.UnitTests.ServiceTests.ServiceTestsFolder
{
    public class CategoryServiceTests : ServiceTestsBase
    {
        private ICategoryService categoryService;

        public CategoryServiceTests()
        {
            categoryService = new CategoryService(context);
        }
        [Fact]
        public async Task GetCategoriesByMainCategoryAsync_ReturnsCategoriesForMainCategory()
        {
            // Act
            var result = await categoryService.GetCategoriesByMainCategoryAsync(1);

            var expected = await categoryService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Contains(expected, result);
        }

        [Fact]
        public async Task GetCategoriesByMainCategoryAsync_ReturnsEmptyListForNonexistentMainCategory()
        {
            // Arrange
            var nonExistentMainCategoryId = 100;

            // Act
            var result = await categoryService.GetCategoriesByMainCategoryAsync(nonExistentMainCategoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
