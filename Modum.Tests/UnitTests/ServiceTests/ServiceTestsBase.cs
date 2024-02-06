using Modum.DataAccess;
using Modum.DataAccess.MainModel;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.LTCs;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Tests.Mocks;
using NUnit.Framework;

namespace Modum.Tests.UnitTests.ServiceTests
{
    public class ServiceTestsBase
    {
        protected DataContext context;

        public ServiceTestsBase()
        {
            this.context = DatabaseMock.Instance;
            SeedMainCategories();
            SeedProducts();
            SeedUsers();
            SeedBannedUsers();
            SeedCategories();
        }
        public void SeedMainCategories()
        {
            // Seed the main categories
            this.context.Add(new MainCategory { Id = 1, Name = "Man" });
            this.context.Add(new MainCategory { Id = 2, Name = "Women" });
            this.context.Add(new MainCategory { Id = 3, Name = "Girls" });
            this.context.Add(new MainCategory { Id = 4, Name = "Boys" });

            this.context.SaveChanges();
        }

        public void SeedProducts()
        {

            this.context.Add(new Product
            {
                Id = 1,
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
            });
            this.context.Add(new Product
            {
                ProductSizes = new List<ProductSizesHelpingTable>
                {
                    new ProductSizesHelpingTable { ProductSize = "Size3", AvailableItems = 8, AllTimeAvailableItems = 80 },
                    new ProductSizesHelpingTable { ProductSize = "Size4", AvailableItems = 12, AllTimeAvailableItems = 120 },
                },
                Id = 2,
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
            });
            this.context.Add(new Product
            {

                Id = 3,
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
            });
            this.context.Add(new Product
            {
                Id = 4,
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
            });
            this.context.Add(new Product
            {

                Id = 5,
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
            });
            this.context.Add(new Product
            {
                Id = 6,
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
            });
            this.context.Add(new Product
            {
                Id = 7,
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
            });
            this.context.Add(new Product
            {
                Id = 8,
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
            });
            this.context.Add(new Product
            {
                Id = 9,
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
            });
            this.context.Add(new Product
            {
                Id = 10,
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
            });
            this.context.SaveChanges();
        }

        public void SeedUsers()
        {
            for (int i = 1; i <= 10; i++)
            {
                var user = new ApplicationUser
                {
                    Id = i.ToString(),
                    UserName = $"User{i}",
                    Email = $"user{i}@example.com",
                    PasswordHash = "testExample123",
                    FirstName = "user",
                    LastName = "test",
                };

                this.context.Users.Add(user);
            }

            this.context.SaveChanges();
        }

        public void SeedBannedUsers()
        {
            for (int i = 1; i <= 4; i++)
            {
                var user = new ShortUserModel
                {
                    Id = i,
                    UserId = i.ToString(),
                    ReasonOfBanning = $"Reason for User {i} ban",
                    DateOfBan = DateTime.Now.AddMonths(-i),
                };

                this.context.Add(user);
            }

            this.context.SaveChanges();
        }
        
        public void SeedCategories()
        {

            var category = new Category
            {
                Id = 1,
                Name = "Test1",
                CreatorName = "Vasio",
                Subcategories = new List<Subcategory>
                {
                    new Subcategory { Id = 1, Name = "Test-Subc", CreatorName = "Vasio",CategoryName="Test1" },
                    new Subcategory { Id = 2, Name = "Test-Subc2", CreatorName = "Vasio",CategoryName="Test1" },
                },
                MainCategoryId = 1
            };

            this.context.Add(category);

            var category2 = new Category
            {
                Id = 2,
                Name = "Test2",
                CreatorName = "Vasio",
                Subcategories = new List<Subcategory>
                {
                    new Subcategory { Id = 3, Name = "Test-Subc", CreatorName = "Vasio",CategoryName="Test2" },
                    new Subcategory { Id = 4, Name = "Test-Subc2", CreatorName = "Vasio",CategoryName="Test2" },
                },
                MainCategoryId = 1
            };

            this.context.Add(category2);

            var category3 = new Category
            {
                Id = 3,
                Name = "Test3",
                CreatorName = "Vasio",
                Subcategories = new List<Subcategory>
                {
                    new Subcategory { Id = 5, Name = "Test-Subc", CreatorName = "Vasio",CategoryName="Test3" },
                    new Subcategory { Id = 6, Name = "Test-Subc2", CreatorName = "Vasio",CategoryName="Test3" },
                },
                MainCategoryId = 1
            };

            this.context.Add(category3);
            this.context.SaveChanges();
        }

        [OneTimeTearDown]
        public void TearDownBase() => this.context.Dispose();
    }
}
