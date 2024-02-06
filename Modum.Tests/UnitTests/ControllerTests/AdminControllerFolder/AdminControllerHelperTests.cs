using Modum.DataAccess.MainModel;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Services.Interfaces;
using Modum.Services.Services.ControllerService.AdminController;
using Moq;

namespace Modum.Tests.UnitTests.ControllerTests.AdminControllerFolder
{
    public class AdminControllerHelperTests
    {
        private readonly Mock<IBannedUsersService> bannedUsersServiceMock;
        private readonly Mock<IWorkerService> workerServiceMock;

        public AdminControllerHelperTests()
        {
            bannedUsersServiceMock = new Mock<IBannedUsersService>();
            workerServiceMock = new Mock<IWorkerService>();
        }
        #region ManageUsers
        [Fact]
        public async Task BanUserHelper_RemovesExistingBan_AddsNewBan()
        {
            // Arrange
            var user = new ApplicationUser { Id = "userId" };
            var reasonOfBanning = "Violating terms";

            var adminControllerHelper = new AdminControllerHelper(
                bannedUsersServiceMock.Object,
                workerServiceMock.Object);

            bannedUsersServiceMock.Setup(x => x.GetUserByUserId(user.Id)).ReturnsAsync(new ShortUserModel());

            // Act
            await adminControllerHelper.BanUserHelper(user, reasonOfBanning);

            // Assert
            bannedUsersServiceMock.Verify(x => x.RemoveAsync(It.IsAny<int>()), Times.Once);
            bannedUsersServiceMock.Verify(x => x.AddAsync(It.IsAny<ShortUserModel>()), Times.Once);

        }

        [Fact]
        public async Task AddUserToPosition_AddsUserToPosition_ReturnsEmptyString()
        {
            //Arrange
            var user = new ApplicationUser { Id = "userId" };
            var position = "CTO";

            var adminControllerHelper = new AdminControllerHelper(
                bannedUsersServiceMock.Object,
                workerServiceMock.Object);

            //Act
            var result = await adminControllerHelper.AddUserToPosition(user, position);

            // Assert
            workerServiceMock.Verify(x => x.AddAsync(It.IsAny<Worker>()), Times.Once);
            Assert.Equal("", result);

        }

        [Fact]
        public async Task RemoveUserFromPosition_ReturnsTask()
        {
            //Arrange
            var user = new ApplicationUser { Id = "userId" };

            var adminControllerHelper = new AdminControllerHelper(
                bannedUsersServiceMock.Object,
                workerServiceMock.Object);


            workerServiceMock.Setup(x => x.DoesThisPersonAlreadyBelongToAPosition(user)).Returns(Task.FromResult(true));

            //Act
            await adminControllerHelper.RemoveUserFromPosition(user);

            // Assert
            workerServiceMock.Verify(x => x.RemoveAsync(It.IsAny<int>()), Times.Once);
        }
        #endregion

        #region ManageWorkers
        [Fact]
        public async Task GetWorkerByUserIdHelper_ReturnsWorker()
        {
            // Arrange
            var userId = "userId";
            var expectedWorker = new Worker();

            var adminControllerHelper = new AdminControllerHelper(
                bannedUsersServiceMock.Object,
                workerServiceMock.Object
            );

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
            var adminControllerHelper = new AdminControllerHelper(
                bannedUsersServiceMock.Object,
                workerServiceMock.Object
            );

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
