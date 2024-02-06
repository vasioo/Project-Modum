using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Modum.DataAccess.MainModel;
using Modum.Models.ViewModels;
using Modum.Services.Services.ControllerService.HomeController;
using Modum.Web.Controllers;
using Moq;
using System.Security.Claims;

namespace Modum.Tests.UnitTests.ControllerTests.HomeControllerFolder
{
    public class HomeControllerTests
    {
        private Mock<UserManager<ApplicationUser>> userManagerMock;
        private Mock<IHomeControllerHelper> controllerHelperMock;
        private DefaultHttpContext httpContext;
        private HomeController controller;

        public HomeControllerTests()
        {
            userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                null!, null!, null!, null!, null!, null!, null!, null!);

            controllerHelperMock = new Mock<IHomeControllerHelper>();

            httpContext = new DefaultHttpContext();
            controller = new HomeController(controllerHelperMock.Object, userManagerMock.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };
        }

        private void SetupUser(string username)
        {
            var user = new ApplicationUser { UserName = username };

            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, username) }));

            userManagerMock.Setup(x => x.FindByNameAsync(username)).ReturnsAsync(user);
        }

        #region Shop
        [Fact]
        public async Task Shop_ReturnsView()
        {
            // Arrange
            var productId = "123";
            var username = "testuser";
            SetupUser(username);

            controllerHelperMock.Setup(x => x.ShopHelper(productId, It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new ShopViewModel());

            // Act
            var result = await controller.Shop(productId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("~/Views/UserViews/Shop.cshtml", result.ViewName);
            Assert.NotNull(result.Model);
        }
        #endregion

        #region UserProductsPartial
        [Fact]
        public async Task _UserProductsPartial_ReturnsView()
        {
            // Arrange
            var username = "testuser";
            SetupUser(username);

            controllerHelperMock.Setup(x => x._UserProductsPartialHelper(null, null, null, 0, null, It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new _UserProductsPartialViewModel());

            // Act
            var result = await controller._UserProductsPartial(null, null!, null!, 0, null!) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<_UserProductsPartialViewModel>(result.Model!);
        }
        #endregion

        #region Favourites
        [Fact]
        public async Task AddToFavourites_ReturnsJson()
        {
            // Arrange
            var username = "testuser";
            SetupUser(username);

            controllerHelperMock.Setup(x => x.AddToFavouritesHelper(It.IsAny<int>(), It.IsAny<ApplicationUser>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await controller.AddToFavourites(123) as JsonResult;

            // Assert
            Assert.NotNull(result);
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.True((bool)jsonResult.Value!.GetType().GetProperty("status")?.GetValue(jsonResult.Value!)!);
        }

        [Fact]
        public async Task RemoveFromFavourites_ReturnsJson()
        {
            // Arrange
            var username = "testuser";
            SetupUser(username);

            controllerHelperMock.Setup(x => x.RemoveFromFavourites(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<ApplicationUser>()))
                .Returns(Task.CompletedTask);
            // Act
            var result = await controller.RemoveFromFavourites(123, true, "size") as JsonResult;

            // Assert
            Assert.NotNull(result);
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.True((bool)jsonResult.Value!.GetType().GetProperty("status")?.GetValue(jsonResult.Value)!);
        }

        [Fact]
        public async Task Favourites_ReturnsView()
        {
            // Arrange
            var username = "testuser";
            SetupUser(username);

            controllerHelperMock.Setup(x => x.FavouritesHelper(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new FavouritesViewModel());

            // Act
            var result = await controller.Favourites() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("~/Views/UserViews/Favourites.cshtml", result.ViewName);
            Assert.NotNull(result.Model);
        }
        #endregion

        #region Cart
        [Fact]
        public async Task AddToCart_ReturnsJson()
        {
            // Arrange
            var username = "testuser";
            SetupUser(username);

            controllerHelperMock.Setup(x => x.AddToCartHelper(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ApplicationUser>()))
                .Returns(Task.CompletedTask);

            var controller = new HomeController(controllerHelperMock.Object, userManagerMock.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            // Act
            var result = await controller.AddToCart(123, "size") as JsonResult;

            // Assert
            Assert.NotNull(result);
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.True((bool)jsonResult.Value!.GetType().GetProperty("status")?.GetValue(jsonResult.Value)!);
        }

        [Fact]
        public async Task RemoveFromCart_ReturnsJson()
        {
            // Arrange
            var username = "testuser";
            SetupUser(username);

            controllerHelperMock.Setup(x => x.RemoveFromCartHelper(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ApplicationUser>()))
                .Returns(Task.CompletedTask);

            var controller = new HomeController(controllerHelperMock.Object, userManagerMock.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            // Act
            var result = await controller.RemoveFromCart(123, true, "size") as JsonResult;

            // Assert
            Assert.NotNull(result);
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.True((bool)jsonResult.Value!.GetType().GetProperty("status")?.GetValue(jsonResult.Value)!);
        }

        [Fact]
        public async Task Cart_ReturnsView()
        {
            // Arrange
            var username = "testuser";
            SetupUser(username);

            controllerHelperMock.Setup(x => x.CartHelper(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new CartViewModel());

            var controller = new HomeController(controllerHelperMock.Object, userManagerMock.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            // Act
            var result = await controller.Cart() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("~/Views/UserViews/Cart.cshtml", result.ViewName);
            Assert.NotNull(result.Model);
        }
        #endregion

        #region GenderCallTemplate
        [Fact]
        public async Task GenderCallTemplate_ReturnsView()
        {
            // Arrange
            var category = "TestCategory";
            controllerHelperMock.Setup(x => x.GenderCallTemplateHelper(category))
                .ReturnsAsync(new GenderCallTemplateViewModel());

            // Act
            var result = await controller.GenderCallTemplate(category) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("~/Views/UserViews/GenderCallTemplate.cshtml", result.ViewName);
            Assert.NotNull(result.Model);
        }
        #endregion
    }

}
