﻿using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Services.Interfaces;
using Modum.Services.Services;
using Moq;

namespace Modum.Tests.UnitTests.ServiceTests.ServiceTestsFolder
{
    public class MainCategoryServiceTests : ServiceTestsBase
    {
        private IMainCategoryService mainCategoryService;
        private ICategoryService categoryService;

        public MainCategoryServiceTests()
        {
            categoryService = new CategoryService(context);
            mainCategoryService = new MainCategoryService(context, categoryService);
        }
        [Fact]
        public async Task GetDefaultMainCategory_ReturnsCorrectResult()
        {
            // Act
            var result = await mainCategoryService.GetDefaultMainCategory();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task SaveCategoriesAndSubcategoriesAsync_SuccessfullySavesCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category
                {
                    Id=1,Name="Test",
                    CreatorName="Vasio",
                    Subcategories = new List<Subcategory>
                        {
                            new Subcategory{Id=2,Name="Test-Subc",CreatorName="Vasio"}
                        } ,
                    MainCategoryId=1
                },
            };

            // Act
            var result = await mainCategoryService.SaveCategoriesAndSubcategoriesAsync(categories);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SaveCategoriesAndSubcategoriesAsync_FailsToSaveCategories()
        {
            var categories = new List<Category>
            {
                new Category
                {
                    Id=1,Name="Test",
                    CreatorName="Vasio",
                    Subcategories = new List<Subcategory>
                        {
                            new Subcategory{Id=2,Name="Test-Subc",CreatorName="Vasio"}
                        } ,
                    MainCategoryId=1
                },
            };

            var mockCategoryService = new Mock<ICategoryService>();
            mockCategoryService.Setup(service => service.GetCategoriesByMainCategoryAsync(It.IsAny<int>()))
                .Throws(new Exception("Simulated exception"));

            var failingMainCategoryService = new MainCategoryService(context, mockCategoryService.Object);

            // Act
            var result = await failingMainCategoryService.SaveCategoriesAndSubcategoriesAsync(categories);

            // Assert
            Assert.False(result);
        }
    }
}