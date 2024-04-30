using Microsoft.AspNetCore.Identity;
using Modum.Models.MainModel;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Services.Interfaces;
using Modum.Services.Services.ControllerService.AdminController;
using Modum.Services.Services.ControllerService.HomeController;
using Moq;

namespace Modum.Tests.UnitTests.ControllerTests.AdminControllerFolder
{
    public class AdminControllerHelperTests
    {
        private readonly Mock<IBannedUsersService> bannedUsersServiceMock;
        private readonly Mock<IWorkerService> workerServiceMock;
        private readonly Mock<IProductSizesService> productSizesServiceMock;
        private readonly Mock<IFirebaseService> firebaseServiceMock;
        private readonly Mock<ICategoryService> categoryServiceMock;
        private readonly Mock<ISubcategoryService> subcategoryServiceMock;
        private readonly Mock<IMainCategoryService> mainCategoryServiceMock;
        private Mock<UserManager<ApplicationUser>> userManagerMock;
        private readonly AdminControllerHelper adminControllerHelper;

        public AdminControllerHelperTests()
        {
            userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                null!, null!, null!, null!, null!, null!, null!, null!);

            bannedUsersServiceMock = new Mock<IBannedUsersService>();
            workerServiceMock = new Mock<IWorkerService>();
            productSizesServiceMock = new Mock<IProductSizesService>();
            firebaseServiceMock = new Mock<IFirebaseService>();
            categoryServiceMock = new Mock<ICategoryService>();
            subcategoryServiceMock = new Mock<ISubcategoryService>();
            mainCategoryServiceMock = new Mock<IMainCategoryService>();

            adminControllerHelper = new AdminControllerHelper(
                bannedUsersServiceMock.Object,
                workerServiceMock.Object,
                productSizesServiceMock.Object,
                firebaseServiceMock.Object,
                categoryServiceMock.Object,
                subcategoryServiceMock.Object,
                mainCategoryServiceMock.Object,
                userManagerMock.Object);
        }
        #region ManageUsers
        [Fact]
        public async Task BanUserHelper_RemovesExistingBan_AddsNewBan()
        {
            // Arrange
            var user = new ApplicationUser { Id = "userId" };
            var reasonOfBanning = "Violating terms";

            bannedUsersServiceMock.Setup(x => x.GetUserByUserId(user.Id)).ReturnsAsync(new ShortUserModel());

            // Act
            await adminControllerHelper.BanUserHelper(user, reasonOfBanning);

            // Assert
            bannedUsersServiceMock.Verify(x => x.RemoveAsync(It.IsAny<Guid>()), Times.Once);
            bannedUsersServiceMock.Verify(x => x.AddAsync(It.IsAny<ShortUserModel>()), Times.Once);

        }

        [Fact]
        public async Task AddUserToPosition_AddsUserToPosition_ReturnsEmptyString()
        {
            //Arrange
            var user = new ApplicationUser { Id = "userId" };
            var position = "CTO";

            //Act
            var result = await adminControllerHelper.AddUserToPosition(user, position);

            // Assert
            workerServiceMock.Verify(x => x.AddAsync(It.IsAny<Worker>()), Times.Once);
            Assert.Equal("", result);

        }

        #endregion

        #region ManageWorkers
        [Fact]
        public async Task GetWorkerByUserIdHelper_ReturnsWorker()
        {
            // Arrange
            var userId = "userId";
            var expectedWorker = new Worker();

            workerServiceMock.Setup(x => x.GetWorkerByUserIdAsync(userId)).ReturnsAsync(expectedWorker);

            // Act
            var result = await adminControllerHelper.GetWorkerByUserIdHelper(userId);

            // Assert
            Assert.Equal(expectedWorker, result);
        }

        [Fact]
        public void GetAllWorkers_ReturnsQueryableOfWorkers()
        {
            // Arrange

            var expectedWorkers = new List<Worker> { new Worker(), new Worker() }.AsQueryable();
            workerServiceMock.Setup(x => x.IQueryableGetAllAsync()).Returns(expectedWorkers);

            // Act
            var result = adminControllerHelper.GetAllWorkers();

            // Assert
            Assert.Equal(expectedWorkers, result);
        }
        #endregion
    }
}
