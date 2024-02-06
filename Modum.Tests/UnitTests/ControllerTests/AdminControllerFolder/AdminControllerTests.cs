using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Modum.DataAccess.MainModel;
using Modum.Models.ViewModels;
using Modum.Services.Services.ControllerService.AdminController;
using Modum.Web.Controllers;
using Moq;
using Newtonsoft.Json;

namespace Modum.Tests.UnitTests.ControllerTests.AdminControllerFolder
{
    public class AdminControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> userManagerMock;
        private readonly Mock<IAdminControllerHelper> controllerHelperMock;
        private readonly AdminController adminController;

        public AdminControllerTests()
        {
            userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                null!, null!, null!, null!, null!, null!, null!, null!);

            controllerHelperMock = new Mock<IAdminControllerHelper>();
            adminController = new AdminController(userManagerMock.Object, controllerHelperMock.Object);
        }

        #region ManageUsers

        [Fact]
        public async Task ManageUsers_ReturnsViewResult()
        {
            // Arrange
            userManagerMock.Setup(u => u.Users).Returns(new List<ApplicationUser>().AsQueryable());

            // Act
            var result = await adminController.ManageUsers(null, "searchString", "currentFilter");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("~/Views/Admin/ManageUsers.cshtml", viewResult.ViewName);
        }

        [Fact]
        public async Task BanUser_Success_ReturnsJsonResult()
        {
            // Arrange
            var userId = "userId";
            var reasonOfBanning = "Reason";
            var user = new ApplicationUser { Id = userId };
            userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await adminController.BanUser(userId, reasonOfBanning);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.NotNull(jsonResult);
            Assert.Equal(
                JsonConvert.SerializeObject(new { status = true, Message = "The user was banned successfully" }),
                JsonConvert.SerializeObject(jsonResult.Value)
            );
        }

        [Fact]
        public async Task MakeAdmin_Success_ReturnsJsonResult()
        {
            // Arrange
            var userId = "userId";
            var user = new ApplicationUser { Id = userId };
            userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await adminController.MakeAdmin(userId);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.NotNull(jsonResult);
            Assert.Equal(
                JsonConvert.SerializeObject(new { status = true, Message = "The user was given rights of admin" }),
                JsonConvert.SerializeObject(jsonResult.Value)
            );
        }

        [Fact]
        public async Task RemoveAdmin_Success_ReturnsJsonResult()
        {
            // Arrange
            var userId = "userId";
            var user = new ApplicationUser { Id = userId };
            userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await adminController.RemoveAdmin(userId);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.NotNull(jsonResult);
            Assert.Equal(
                JsonConvert.SerializeObject(new { status = true, Message = "The user was given rights of admin" }),
                JsonConvert.SerializeObject(jsonResult.Value)
            );
        }

        [Fact]
        public async Task MakeWorker_Success_ReturnsJsonResult()
        {
            // Arrange
            var userId = "userId";
            var position = "Worker";
            var user = new ApplicationUser { Id = userId };
            userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(user);
            controllerHelperMock.Setup(h => h.AddUserToPosition(user, position)).ReturnsAsync("");

            // Act
            var result = await adminController.MakeWorker(userId, position);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.NotNull(jsonResult);
            // Change this line
            // Replace this line
            Assert.Equal(
            JsonConvert.SerializeObject(new { status = true, Message = "The user now has worker's rights" }),
            JsonConvert.SerializeObject(jsonResult.Value)
            );
        }

        [Fact]
        public async Task RemoveWorker_Success_ReturnsJsonResult()
        {
            // Arrange
            var userId = "userId";
            var user = new ApplicationUser { Id = userId };
            userManagerMock.Setup(u => u.FindByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await adminController.RemoveWorker(userId);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.NotNull(jsonResult);
            Assert.Equal(
                JsonConvert.SerializeObject(new { status = true, Message = "The user does not have worker's rights anymore" }),
                JsonConvert.SerializeObject(jsonResult.Value)
            );

        }
        #endregion

        #region ManageOrders
        [Fact]
        public async Task AdditionalOrderInformation_ReturnsViewResult()
        {
            // Arrange
            var orderId = "orderId";
            var orderHelper = new OrderLogViewModel(); // Replace with actual instance

            // Mocking AdditionalOrderInformationHelper
            controllerHelperMock.Setup(helper => helper.AdditionalOrderInformationHelper(orderId))
                               .ReturnsAsync(orderHelper);

            // Act
            var result = await adminController.AdditionalOrderInformation(orderId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("~/Views/Admin/AdditionalOrderInformation.cshtml", viewResult.ViewName);
        }
        #endregion

        #region ManageWorkers

        [Fact]
        public async Task EditWorkerInformation_ReturnsViewResult()
        {
            // Arrange
            var userId = "userId";
            var worker = new Worker();

            controllerHelperMock.Setup(helper => helper.GetWorkerByUserIdHelper(userId))
                               .ReturnsAsync(worker);

            // Act
            var result = await adminController.EditWorkerInformation(userId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("~/Views/Admin/EditWorkerInformation.cshtml", viewResult.ViewName);
        }

        [Fact]
        public async Task EditInformationForWorker_ReturnsViewResult()
        {
            // Arrange
            var worker = new Worker();

            // Act
            var result = await adminController.EditInformationForWorker(worker);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            Assert.Equal("~/Views/Admin/EditWorkerInformation.cshtml", viewResult.ViewName);
        }
        #endregion

    }
}
