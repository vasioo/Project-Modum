﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Modum.DataAccess.MainModel;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.FooterItems;
using Modum.Models.BaseModels.Models.LTCs;
using Modum.Models.BaseModels.Models.MongoDb;
using Modum.Models.BaseModels.Models.Payment;
using Modum.Models.BaseModels.Models.PaymentStructure;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Models.ViewModels;
using Modum.Services.Interfaces;
using Modum.Services.Services.ControllerService.HomeController;
using Moq;

namespace Modum.Tests.UnitTests.ControllerTests.HomeControllerFolder
{
    public class HomeControllerHelperTests
    {
        private readonly Mock<IFavouriteService> favouriteServiceMock;
        private readonly Mock<IProductService> productServiceMock;
        private readonly Mock<ICartService> cartServiceMock;
        private readonly Mock<ICategoryService> categoryServiceMock;
        private readonly Mock<ISubcategoryService> subcategoryServiceMock;
        private readonly Mock<IMainCategoryService> mainCategoryServiceMock;
        private readonly Mock<IBrandService> brandServiceMock;
        private readonly Mock<IFirebaseService> firebaseServiceMock;
        private readonly Mock<ILTCService> ltcServiceMock;
        private readonly Mock<UserManager<ApplicationUser>> userManagerMock;
        private readonly Mock<IConfiguration> configurationMock;
        private readonly Mock<IEmailSenderService> emailSenderServiceMock;
        private readonly HomeControllerHelper homeControllerHelper;
        public HomeControllerHelperTests()
        {
            favouriteServiceMock = new Mock<IFavouriteService>();
            productServiceMock = new Mock<IProductService>();
            cartServiceMock = new Mock<ICartService>();
            categoryServiceMock = new Mock<ICategoryService>();
            subcategoryServiceMock = new Mock<ISubcategoryService>();
            mainCategoryServiceMock = new Mock<IMainCategoryService>();
            brandServiceMock = new Mock<IBrandService>();
            firebaseServiceMock = new Mock<IFirebaseService>();
            ltcServiceMock = new Mock<ILTCService>();
            emailSenderServiceMock = new Mock<IEmailSenderService>();

            configurationMock = new Mock<IConfiguration>();

            userManagerMock = new Mock<UserManager<ApplicationUser>>(
               new Mock<IUserStore<ApplicationUser>>().Object,
               null!, null!, null!, null!, null!, null!, null!, null!);

            homeControllerHelper = new HomeControllerHelper(
                favouriteServiceMock.Object,
                productServiceMock.Object,
                cartServiceMock.Object,
                categoryServiceMock.Object,
                subcategoryServiceMock.Object,
                mainCategoryServiceMock.Object,
                brandServiceMock.Object,
                userManagerMock.Object,
                firebaseServiceMock.Object,
                ltcServiceMock.Object,
                configurationMock.Object, emailSenderServiceMock.Object);
        }

        #region ShopHelper
        [Fact]
        public async Task ShopHelper_ReturnsShopViewModel()
        {
            // Arrange
            var productId = "123";
            var user = new ApplicationUser { Id = "userId" };

            favouriteServiceMock.Setup(x => x.GetFavouritesContainerByUserId(user.Id)).ReturnsAsync(new Favourites());
            productServiceMock.Setup(x => x.GetProductsByTenMostAddedToFavourites()).ReturnsAsync(new List<Product>());
            categoryServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Category>());
            brandServiceMock.Setup(x => x.GetMostFavouriteBrandsBySoldItemsBySection("normal")).ReturnsAsync(new List<Brands>());
            brandServiceMock.Setup(x => x.GetMostFavouriteBrandsBySoldItemsBySection("premium")).ReturnsAsync(new List<Brands>());
            firebaseServiceMock.Setup(x => x.GetLastViewedProducts(user.Id)).ReturnsAsync(new Dictionary<string, LastViewedProduct>());

            // Act
            var result = await homeControllerHelper.ShopHelper(productId, user);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ShopViewModel>(result);
        }
        #endregion

        #region FavouritesHelper
        [Fact]
        public async Task FavouritesHelper_ReturnsFavouritesViewModel()
        {
            // Arrange
            var user = new ApplicationUser { Id = "userId" };

            favouriteServiceMock.Setup(x => x.GetFavouritesContainerByUserId(user.Id)).ReturnsAsync(new Favourites());
            productServiceMock.Setup(x => x.GetProductsByTenMostAddedToFavourites()).ReturnsAsync(new List<Product>());
            categoryServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Category>());
            brandServiceMock.Setup(x => x.GetMostFavouriteBrandsBySoldItemsBySection("normal")).ReturnsAsync(new List<Brands>());
            brandServiceMock.Setup(x => x.GetMostFavouriteBrandsBySoldItemsBySection("premium")).ReturnsAsync(new List<Brands>());
            firebaseServiceMock.Setup(x => x.GetLastViewedProducts(user.Id)).ReturnsAsync(new Dictionary<string, LastViewedProduct>());

            // Act
            var result = await homeControllerHelper.FavouritesHelper(user);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FavouritesViewModel>(result);
            Assert.NotNull(result.TenFavItems);
            Assert.NotNull(result.UnderNavCategories);
            Assert.NotNull(result.BasicBrands);
            Assert.NotNull(result.PremiumBrands);

        }

        [Fact]
        public async Task AddToFavouritesHelper_SuccessfullyAddsToFavourites()
        {
            // Arrange
            var productId = 123;
            var user = new ApplicationUser { Id = "userId" };
            var product = new Product { Id = productId, Brand = "TestBrand" };
            var brand = new Brands { Id = 1, BrandName = "TestBrand" };

            productServiceMock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
            brandServiceMock.Setup(x => x.GetBrandByStringName(product.Brand)).ReturnsAsync(brand);
            favouriteServiceMock.Setup(x => x.GetFavouritesContainerByUserId(user.Id)).ReturnsAsync(new Favourites());
            favouriteServiceMock.Setup(x => x.AddAsync(It.IsAny<Favourites>())).ReturnsAsync(1);

            // Act
            await homeControllerHelper.AddToFavouritesHelper(productId, user);

            // Assert
            favouriteServiceMock.Verify(x => x.GetFavouritesContainerByUserId(user.Id), Times.Once);
            favouriteServiceMock.Verify(x => x.AddAsync(It.IsAny<Favourites>()), Times.Once);
            productServiceMock.Verify(x => x.UpdateAsync(product), Times.Once);
            brandServiceMock.Verify(x => x.UpdateAsync(brand), Times.Once);

        }

        [Fact]
        public async Task RemoveFromFavourites_SuccessfullyRemovesFromFavourites()
        {
            // Arrange
            var productId = 123;
            var addToCart = true;
            var size = "size";
            var user = new ApplicationUser { Id = "userId" };
            var product = new Product { Id = productId, Brand = "TestBrand" };
            var favourites = new Favourites { Id = 1, UserId = user.Id, Products = new List<Product> { product } };
            var brand = new Brands { Id = 1, BrandName = "TestBrand" };


            productServiceMock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
            brandServiceMock.Setup(x => x.GetBrandByStringName(product.Brand)).ReturnsAsync(brand);
            favouriteServiceMock.Setup(x => x.GetFavouritesIdByUserId(user.Id)).ReturnsAsync(favourites.Id);
            favouriteServiceMock.Setup(x => x.GetByIdAsync(favourites.Id)).ReturnsAsync(favourites);

            // Act
            await homeControllerHelper.RemoveFromFavourites(productId, addToCart, size, user);

            // Assert
            favouriteServiceMock.Verify(x => x.GetFavouritesIdByUserId(user.Id), Times.Once);
            favouriteServiceMock.Verify(x => x.GetByIdAsync(favourites.Id), Times.Once);
            productServiceMock.Verify(x => x.UpdateAsync(product), Times.Once);
            brandServiceMock.Verify(x => x.UpdateAsync(brand), Times.Once);
            favouriteServiceMock.Verify(x => x.UpdateAsync(favourites), Times.Once);
        }
        #endregion

        #region CartHelper
        [Fact]
        public async Task CartHelper_ReturnsCartViewModel()
        {
            // Arrange
            var user = new ApplicationUser { Id = "userId" };

            cartServiceMock.Setup(x => x.GetCartContainerByUserId(user.Id)).ReturnsAsync(new Cart());
            productServiceMock.Setup(x => x.GetProductsByTenMostAddedToFavourites()).ReturnsAsync(new List<Product>());
            categoryServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Category>());
            brandServiceMock.Setup(x => x.GetMostFavouriteBrandsBySoldItemsBySection("normal")).ReturnsAsync(new List<Brands>());
            brandServiceMock.Setup(x => x.GetMostFavouriteBrandsBySoldItemsBySection("premium")).ReturnsAsync(new List<Brands>());
            firebaseServiceMock.Setup(x => x.GetLastViewedProducts(user.Id)).ReturnsAsync(new Dictionary<string, LastViewedProduct>());

            // Act
            var result = await homeControllerHelper.CartHelper(user);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CartViewModel>(result);
            Assert.NotNull(result.TenFavItems);
            Assert.NotNull(result.UnderNavCategories);
            Assert.NotNull(result.BasicBrands);
            Assert.NotNull(result.PremiumBrands);
        }

        [Fact]
        public async Task AddToCartHelper_SuccessfullyAddsToCart()
        {
            // Arrange
            var productId = 123;
            var size = "size";
            var user = new ApplicationUser { Id = "userId" };
            var product = new Product { Id = productId };
            var cartItem = new CartItem { ProductId = product.Id, Size = size };
            var cart = new Cart { UserId = user.Id };

            productServiceMock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
            cartServiceMock.Setup(x => x.GetCartContainerByUserId(user.Id)).ReturnsAsync(new Cart());
            cartServiceMock.Setup(x => x.AddAsync(cart)).ReturnsAsync(1);

            // Act
            await homeControllerHelper.AddToCartHelper(productId, size, user);

            // Assert
            cartServiceMock.Verify(x => x.GetCartContainerByUserId(user.Id), Times.Once);
            cartServiceMock.Verify(x => x.AddAsync(It.IsAny<Cart>()), Times.Once);
            cartServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Cart>()), Times.Never);
        }

        [Fact]
        public async Task RemoveFromCartHelper_SuccessfullyRemovesFromCart()
        {
            // Arrange
            var productId = 123;
            var size = "size";
            var user = new ApplicationUser { Id = "userId" };
            var product = new Product { Id = productId };
            var cartItemToRemove = new CartItem { ProductId = productId, Size = size };
            var cart = new Cart { UserId = user.Id, CartItems = new List<CartItem> { cartItemToRemove } };

            productServiceMock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
            cartServiceMock.Setup(x => x.GetCartIdByUserId(user.Id)).ReturnsAsync(1);
            cartServiceMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(cart);

            // Act
            await homeControllerHelper.RemoveFromCartHelper(productId, size, user);

            // Assert
            cartServiceMock.Verify(x => x.GetCartIdByUserId(user.Id), Times.Once);
            cartServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
            cartServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Cart>()), Times.Once);
        }
        #endregion

        #region GenderCallTemplateHelper
        [Fact]
        public async Task GenderCallTemplateHelper_ReturnsGenderCallTemplateViewModel()
        {
            // Arrange
            var category = "TestCategory";

            productServiceMock.Setup(x => x.GetProductsByTenMostAddedToFavourites()).ReturnsAsync(new List<Product>());
            productServiceMock.Setup(x => x.GetProductsByTenMostBought()).ReturnsAsync(new List<Product>());
            categoryServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Category>());
            brandServiceMock.Setup(x => x.GetMostFavouriteBrandsBySoldItemsBySection("normal")).ReturnsAsync(new List<Brands>());
            brandServiceMock.Setup(x => x.GetMostFavouriteBrandsBySoldItemsBySection("premium")).ReturnsAsync(new List<Brands>());
            ltcServiceMock.Setup(x => x.GetBestLTCNow()).ReturnsAsync(new LTC());
            firebaseServiceMock.Setup(x => x.GetAllBlogPosts()).Returns(Enumerable.Empty<BlogPost>().AsQueryable());

            // Act
            var result = await homeControllerHelper.GenderCallTemplateHelper(category);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<GenderCallTemplateViewModel>(result);
            Assert.Equal(category, result.Category);
            Assert.NotNull(result.TenFavItems);
            Assert.NotNull(result.MostBoughtItems);
            Assert.NotNull(result.UnderNavCategories);
            Assert.NotNull(result.BasicBrands);
            Assert.NotNull(result.PremiumBrands);
            Assert.NotNull(result.LimitedTimeCampaign);
            Assert.Empty(result.BlogPostsForTemplate);
        }

        #endregion

        #region UserProductsHelper
        [Fact]
        public async Task _UserProductsPartialHelper_ReturnsViewModel()
        {
            // Arrange
            var page = 1;
            var filter = new ProductFilter();
            var sortBy = "Most Popular";
            var mainCategoryId = 1;
            var searchProducts = "test";
            var user = new ApplicationUser { Id = "userId" };

            productServiceMock.Setup(x => x.IQueryableGetAllAsync()).Returns(new List<Product>().AsQueryable());

            categoryServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Category>());
            subcategoryServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Subcategory>());
            mainCategoryServiceMock.Setup(x => x.GetDefaultMainCategory()).ReturnsAsync(new MainCategory { Id = 1 });
            firebaseServiceMock.Setup(x => x.GetLastViewedProducts(user.Id)).ReturnsAsync(new Dictionary<string, LastViewedProduct>());

            // Act
            var result = await homeControllerHelper._UserProductsPartialHelper(page, filter, sortBy, mainCategoryId, searchProducts, user);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<_UserProductsPartialViewModel>(result);

            Assert.NotNull(result.TenFavItems);
            Assert.NotNull(result.UnderNavCategories);
            Assert.NotNull(result.BasicBrands);
            Assert.NotNull(result.PremiumBrands);
            Assert.NotNull(result.Categories);
            Assert.NotNull(result.Subcategories);
            Assert.NotNull(result.DistinctColours);
            Assert.NotNull(result.DistinctBrands);
            Assert.NotNull(result.Sizes);
            Assert.NotNull(result.LastViewedProducts);
            Assert.NotNull(result.FilterSizes);
            Assert.NotNull(result.FilterColors);
            Assert.NotNull(result.FilterBrands);
            Assert.NotNull(result.FilterLTCs);
            Assert.Equal(searchProducts, result.SearchStringContainer);
            Assert.Equal(sortBy, result.SortBy);
            Assert.Equal(mainCategoryId, result.MainCategoryId);
            Assert.Equal(1000, result.MaxPrice);
            Assert.Equal(0, result.MinPrice);
        }

        #endregion
    }
}
