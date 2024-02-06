using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Services.Interfaces;
using Modum.Services.Services;

namespace Modum.Tests.UnitTests.ServiceTests.ServiceTestsFolder
{
    public class BaseServiceTests : ServiceTestsBase
    {
        private IAdsService adsService;

        public BaseServiceTests()
        {
            adsService = new AdsService(context);
        }

        [Fact]
        public async Task AddAsync_AddsEntityAndReturnsId()
        {
            // Arrange
            var productToAdd = new Product
            {
                Id = 11,
                ProductSizes = new List<ProductSizesHelpingTable>
                {
                    new ProductSizesHelpingTable { ProductSize = "Size3", AvailableItems = 8, AllTimeAvailableItems = 80 },
                    new ProductSizesHelpingTable { ProductSize = "Size4", AvailableItems = 12, AllTimeAvailableItems = 120 },
                },
                Title = "Bluetooth Speaker",
                Brand = "JBL",
                Price = 69.99m,
                UploadedBy = "User10",
                Description = "Portable Bluetooth speaker with excellent sound quality.",
                MainCategoryId = 5,
                CategoryId = 5,
                SubcategoryId = 5,
                Colour = "White",
                ImageContainerId = "bluetoothspeaker_image",
                Season = "All Seasons"
            };

            // Act
            var addedId = await adsService.AddAsync(productToAdd);

            // Assert
            Assert.NotEqual(0, addedId);
            var retrievedProduct = await adsService.GetByIdAsync(addedId);
            Assert.NotNull(retrievedProduct);
        }

        [Fact]
        public async Task AddRangeAsync_AddsEntitiesAndReturnsCount()
        {
            // Arrange
            var productsToAdd = new List<Product>
        {
            new Product {   Id = 14,
                ProductSizes = new List<ProductSizesHelpingTable>
                {
                    new ProductSizesHelpingTable { ProductSize = "Size3", AvailableItems = 8, AllTimeAvailableItems = 80 },
                    new ProductSizesHelpingTable { ProductSize = "Size4", AvailableItems = 12, AllTimeAvailableItems = 120 },
                },
                Title = "Bluetooth Speaker",
                Brand = "JBL",
                Price = 69.99m,
                UploadedBy = "User10",
                Description = "Portable Bluetooth speaker with excellent sound quality.",
                MainCategoryId = 5,
                CategoryId = 5,
                SubcategoryId = 5,
                Colour = "White",
                ImageContainerId = "bluetoothspeaker_image",
                Season = "All Seasons" },
            new Product {   Id = 12,
                ProductSizes = new List<ProductSizesHelpingTable>
                {
                    new ProductSizesHelpingTable { ProductSize = "Size3", AvailableItems = 8, AllTimeAvailableItems = 80 },
                    new ProductSizesHelpingTable { ProductSize = "Size4", AvailableItems = 12, AllTimeAvailableItems = 120 },
                },
                Title = "Bluetooth Speaker",
                Brand = "JBL",
                Price = 69.99m,
                UploadedBy = "User10",
                Description = "Portable Bluetooth speaker with excellent sound quality.",
                MainCategoryId = 5,
                CategoryId = 5,
                SubcategoryId = 5,
                Colour = "White",
                ImageContainerId = "bluetoothspeaker_image",
                Season = "All Seasons" },
        };

            // Act
            var addedCount = await adsService.AddRangeAsync(productsToAdd);

            // Assert
            Assert.Equal(2, addedCount);
            var allProducts = await adsService.GetAllAsync();
            Assert.Equal(12, allProducts.Count());
        }

        [Fact]
        public async Task FindAsync_ReturnsMatchingEntities()
        {
            // Act
            var foundProducts = await adsService.FindAsync(p => p.Price > 50);

            // Assert
            Assert.Equal(10, foundProducts.Count());
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllEntities()
        {
            // Act
            var allProducts = await adsService.GetAllAsync();

            // Assert
            Assert.Equal(10, allProducts.Count());
        }

        [Fact]
        public async Task IQueryableGetAllAsync_ReturnsQueryable()
        {
            // Act
            var queryable = adsService.IQueryableGetAllAsync();

            // Assert
            Assert.NotNull(queryable);
            // Add more specific assertions if needed
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectEntity()
        {
            // Act
            var retrievedProduct = await adsService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(retrievedProduct);
            Assert.Equal(1, retrievedProduct.Id);
        }

        [Fact]
        public async Task GetCountOfAllItems_ReturnsCorrectCount()
        {
            // Act
            var count = await adsService.GetCountOfAllItems();

            // Assert
            Assert.Equal(10, count);
        }

        [Fact]
        public async Task RemoveAsync_RemovesEntityAndReturnsCount()
        {
            // Arrange
            var productToAdd = new Product
            {
                Id=11,
                ProductSizes = new List<ProductSizesHelpingTable>
                {
                    new ProductSizesHelpingTable { ProductSize = "Size3", AvailableItems = 8, AllTimeAvailableItems = 80 },
                    new ProductSizesHelpingTable { ProductSize = "Size4", AvailableItems = 12, AllTimeAvailableItems = 120 },
                },
                Title = "Bluetooth Speaker",
                Brand = "JBL",
                Price = 69.99m,
                UploadedBy = "User10",
                Description = "Portable Bluetooth speaker with excellent sound quality.",
                MainCategoryId = 5,
                CategoryId = 5,
                SubcategoryId = 5,
                Colour = "White",
                ImageContainerId = "bluetoothspeaker_image",
                Season = "All Seasons"
            };
            var addedId = await adsService.AddAsync(productToAdd);

            // Act
            var removedCount = await adsService.RemoveAsync(addedId);

            // Assert
            Assert.NotEqual(0, removedCount);
            var retrievedProduct = await adsService.GetByIdAsync(addedId);
            Assert.Equal(0,retrievedProduct.Id);
        }

        [Fact]
        public async Task RemoveRangeAsync_RemovesEntitiesAndReturnsCount()
        {
            // Arrange
            var product1 = new Product
            {
                Id = 16,
                ProductSizes = new List<ProductSizesHelpingTable>
                {
                    new ProductSizesHelpingTable { ProductSize = "Size3", AvailableItems = 8, AllTimeAvailableItems = 80 },
                    new ProductSizesHelpingTable { ProductSize = "Size4", AvailableItems = 12, AllTimeAvailableItems = 120 },
                },
                Title = "Bluetooth Speaker",
                Brand = "JBL",
                Price = 69.99m,
                UploadedBy = "User10",
                Description = "Portable Bluetooth speaker with excellent sound quality.",
                MainCategoryId = 5,
                CategoryId = 5,
                SubcategoryId = 5,
                Colour = "White",
                ImageContainerId = "bluetoothspeaker_image",
                Season = "All Seasons"
            };
            var product2 = new Product
            {
                Id = 17,
                ProductSizes = new List<ProductSizesHelpingTable>
                {
                    new ProductSizesHelpingTable { ProductSize = "Size3", AvailableItems = 8, AllTimeAvailableItems = 80 },
                    new ProductSizesHelpingTable { ProductSize = "Size4", AvailableItems = 12, AllTimeAvailableItems = 120 },
                },
                Title = "Bluetooth Speaker",
                Brand = "JBL",
                Price = 69.99m,
                UploadedBy = "User10",
                Description = "Portable Bluetooth speaker with excellent sound quality.",
                MainCategoryId = 5,
                CategoryId = 5,
                SubcategoryId = 5,
                Colour = "White",
                ImageContainerId = "bluetoothspeaker_image",
                Season = "All Seasons"
            };

            await adsService.AddRangeAsync(new List<Product> { product1, product2 });

            Assert.Equal(12, await adsService.GetCountOfAllItems());

            // Act
            var removedCount = await adsService.RemoveRangeAsync(new List<Product> { product1, product2 });

            // Assert
            Assert.Equal(10, await adsService.GetCountOfAllItems());
        }

        [Fact]
        public async Task UpdateAsync_UpdatesEntityAndReturnsCount()
        {
            // Arrange
            var productToAdd = new Product { /* Initialize properties */ };
            var addedId = await adsService.AddAsync(productToAdd);

            // Modify some properties
            productToAdd.Price = 89.99m;

            // Act
            var updatedCount = await adsService.UpdateAsync(productToAdd);

            // Assert
            Assert.Equal(1, updatedCount);
            var updatedProduct = await adsService.GetByIdAsync(addedId);
            Assert.Equal(89.99m, updatedProduct.Price);
        }

        [Fact]
        public async Task UpdateRangeAsync_UpdatesEntitiesAndReturnsCount()
        {
            // Arrange
            var product1 = new Product
            {
                Id = 18,
                ProductSizes = new List<ProductSizesHelpingTable>
                {
                    new ProductSizesHelpingTable { ProductSize = "Size3", AvailableItems = 8, AllTimeAvailableItems = 80 },
                    new ProductSizesHelpingTable { ProductSize = "Size4", AvailableItems = 12, AllTimeAvailableItems = 120 },
                },
                Title = "Bluetooth Speaker",
                Brand = "JBL",
                Price = 69.99m,
                UploadedBy = "User10",
                Description = "Portable Bluetooth speaker with excellent sound quality.",
                MainCategoryId = 5,
                CategoryId = 5,
                SubcategoryId = 5,
                Colour = "White",
                ImageContainerId = "bluetoothspeaker_image",
                Season = "All Seasons"
            };
            var product2 = new Product
            {
                Id = 19,
                ProductSizes = new List<ProductSizesHelpingTable>
                {
                    new ProductSizesHelpingTable { ProductSize = "Size3", AvailableItems = 8, AllTimeAvailableItems = 80 },
                    new ProductSizesHelpingTable { ProductSize = "Size4", AvailableItems = 12, AllTimeAvailableItems = 120 },
                },
                Title = "Bluetooth Speaker",
                Brand = "JBL",
                Price = 69.99m,
                UploadedBy = "User10",
                Description = "Portable Bluetooth speaker with excellent sound quality.",
                MainCategoryId = 5,
                CategoryId = 5,
                SubcategoryId = 5,
                Colour = "White",
                ImageContainerId = "bluetoothspeaker_image",
                Season = "All Seasons"
            };

            await adsService.AddRangeAsync(new List<Product> { product1, product2 });

            // Modify some properties
            product1.Price = 89.99m;
            product2.Price = 99.99m;

            // Act
            var updatedCount = await adsService.UpdateRangeAsync(new List<Product> { product1, product2 });

            // Assert
            Assert.Equal(2, updatedCount);
            var updatedProduct1 = await adsService.GetByIdAsync(product1.Id);
            Assert.Equal(89.99m, updatedProduct1.Price);
            var updatedProduct2 = await adsService.GetByIdAsync(product2.Id);
            Assert.Equal(99.99m, updatedProduct2.Price);
        }
    }
}
