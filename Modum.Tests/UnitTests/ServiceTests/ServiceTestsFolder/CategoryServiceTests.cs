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
        public async Task GetCategoriesByMainCategoryAsync_ReturnsEmptyListForNonexistentMainCategory()
        {
            // Arrange
            var nonExistentMainCategoryId = Guid.NewGuid();

            // Act
            var result = await categoryService.GetCategoriesByMainCategoryAsync(nonExistentMainCategoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
