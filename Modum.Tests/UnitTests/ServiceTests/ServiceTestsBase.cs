using Modum.DataAccess;
using Modum.Models.MainModel;
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
            this.context.Add(new MainCategory {Name = "Man" });
            this.context.Add(new MainCategory {Name = "Women" });
            this.context.Add(new MainCategory {Name = "Girls" });
            this.context.Add(new MainCategory {Name = "Boys" });

            this.context.SaveChanges();
        }

        public void SeedProducts()
        {

            this.context.Add(new Product
            {
                Id = Guid.NewGuid(),
                Title = "Bluetooth Speaker",
                Brand = "JBL",
                Price = 69.99m,
                UploadedBy = "User10",
                Description = "Portable Bluetooth speaker with excellent sound quality.",
                MainCategoryId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                SubcategoryId = Guid.NewGuid(),
                Colour = "White",
                ImageContainerId = "bluetoothspeaker_image",
                Season = "All Seasons"
            });
            this.context.Add(new Product
            {
                Id = Guid.NewGuid(),
                Title = "Bluetooth Speaker",
                Brand = "JBL",
                Price = 69.99m,
                UploadedBy = "User10",
                Description = "Portable Bluetooth speaker with excellent sound quality.",
                MainCategoryId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                SubcategoryId = Guid.NewGuid(),
                Colour = "White",
                ImageContainerId = "bluetoothspeaker_image",
                Season = "All Seasons"
            });
            this.context.Add(new Product
            {

                Id = Guid.NewGuid(),
                Title = "Bluetooth Speaker",
                Brand = "JBL",
                Price = 69.99m,
                UploadedBy = "User10",
                Description = "Portable Bluetooth speaker with excellent sound quality.",
                MainCategoryId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                SubcategoryId = Guid.NewGuid(),
                Colour = "White",
                ImageContainerId = "bluetoothspeaker_image",
                Season = "All Seasons"
            });
            this.context.Add(new Product
            {
                Id = Guid.NewGuid(),
                Title = "Bluetooth Speaker",
                Brand = "JBL",
                Price = 69.99m,
                UploadedBy = "User10",
                Description = "Portable Bluetooth speaker with excellent sound quality.",
                MainCategoryId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                SubcategoryId = Guid.NewGuid(),
                Colour = "White",
                ImageContainerId = "bluetoothspeaker_image",
                Season = "All Seasons"
            });
            this.context.Add(new Product
            {

                Id = Guid.NewGuid(),
                Title = "Bluetooth Speaker",
                Brand = "JBL",
                Price = 69.99m,
                UploadedBy = "User10",
                Description = "Portable Bluetooth speaker with excellent sound quality.",
                MainCategoryId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                SubcategoryId = Guid.NewGuid(),
                Colour = "White",
                ImageContainerId = "bluetoothspeaker_image",
                Season = "All Seasons"
            });
            this.context.Add(new Product
            {
                Id = Guid.NewGuid(),
                Title = "Bluetooth Speaker",
                Brand = "JBL",
                Price = 69.99m,
                UploadedBy = "User10",
                Description = "Portable Bluetooth speaker with excellent sound quality.",
                MainCategoryId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                SubcategoryId = Guid.NewGuid(),
                Colour = "White",
                ImageContainerId = "bluetoothspeaker_image",
                Season = "All Seasons"
            });
            this.context.Add(new Product
            {
                Id = Guid.NewGuid(),
                Title = "Bluetooth Speaker",
                Brand = "JBL",
                Price = 69.99m,
                UploadedBy = "User10",
                Description = "Portable Bluetooth speaker with excellent sound quality.",
                MainCategoryId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                SubcategoryId = Guid.NewGuid(),
                Colour = "White",
                ImageContainerId = "bluetoothspeaker_image",
                Season = "All Seasons"
            });
            this.context.Add(new Product
            {
                Id = Guid.NewGuid(),
                Title = "Bluetooth Speaker",
                Brand = "JBL",
                Price = 69.99m,
                UploadedBy = "User10",
                Description = "Portable Bluetooth speaker with excellent sound quality.",
                MainCategoryId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                SubcategoryId = Guid.NewGuid(),
                Colour = "White",
                ImageContainerId = "bluetoothspeaker_image",
                Season = "All Seasons"
            });
            this.context.Add(new Product
            {
                Id = Guid.NewGuid(),
                Title = "Bluetooth Speaker",
                Brand = "JBL",
                Price = 69.99m,
                UploadedBy = "User10",
                Description = "Portable Bluetooth speaker with excellent sound quality.",
                MainCategoryId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                SubcategoryId = Guid.NewGuid(),
                Colour = "White",
                ImageContainerId = "bluetoothspeaker_image",
                Season = "All Seasons"
            });
            this.context.Add(new Product
            {
                Id = Guid.NewGuid(),
                Title = "Bluetooth Speaker",
                Brand = "JBL",
                Price = 69.99m,
                UploadedBy = "User10",
                Description = "Portable Bluetooth speaker with excellent sound quality.",
                MainCategoryId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                SubcategoryId = Guid.NewGuid(),
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
                    Id = Guid.NewGuid(),
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
                Id = Guid.NewGuid(),
                Name = "Test1",
                CreatorName = "Vasio",
                Subcategories = new List<Subcategory>
                {
                    new Subcategory { Id = Guid.NewGuid(), Name = "Test-Subc", CreatorName = "Vasio",CategoryName="Test1" },
                    new Subcategory { Id = Guid.NewGuid(), Name = "Test-Subc2", CreatorName = "Vasio",CategoryName="Test1" },
                },
                MainCategoryId = Guid.NewGuid()
            };

            this.context.Add(category);

            var category2 = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Test2",
                CreatorName = "Vasio",
                Subcategories = new List<Subcategory>
                {
                    new Subcategory { Id = Guid.NewGuid(), Name = "Test-Subc", CreatorName = "Vasio",CategoryName="Test2" },
                    new Subcategory { Id = Guid.NewGuid(), Name = "Test-Subc2", CreatorName = "Vasio",CategoryName="Test2" },
                },
                MainCategoryId = Guid.NewGuid()
            };

            this.context.Add(category2);

            var category3 = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Test3",
                CreatorName = "Vasio",
                Subcategories = new List<Subcategory>
                {
                    new Subcategory { Id = Guid.NewGuid(), Name = "Test-Subc", CreatorName = "Vasio",CategoryName="Test3" },
                    new Subcategory { Id = Guid.NewGuid(), Name = "Test-Subc2", CreatorName = "Vasio",CategoryName="Test3" },
                },
                MainCategoryId = Guid.NewGuid()
            };

            this.context.Add(category3);
            this.context.SaveChanges();
        }

        [OneTimeTearDown]
        public void TearDownBase() => this.context.Dispose();
    }
}
