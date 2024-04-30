using Modum.Models.MainModel;
using Modum.Services.Interfaces;
using Modum.Services.Services;

namespace Modum.Tests.UnitTests.ServiceTests.ServiceTestsFolder
{
    public class WorkerServiceTests : ServiceTestsBase
    {
        private IWorkerService workerService;

        public WorkerServiceTests()
        {
            workerService = new WorkerService(context);
        }
        [Fact]
        public async Task DoesThisPersonAlreadyBelongToAPosition_ReturnsFalseForNewUser()
        {
            // Arrange
            var newUser = new ApplicationUser { Id = "newUserId" };

            // Act
            var result = await workerService.DoesThisPersonAlreadyBelongToAPosition(newUser);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DoesThisPersonAlreadyBelongToAPosition_ReturnsTrueForExistingUser()
        {
            // Arrange
            var existingUser = new ApplicationUser { Id = "existingUserId" };
            await context.Workers.AddAsync(new Worker { AppUser = existingUser });
            await context.SaveChangesAsync();

            // Act
            var result = await workerService.DoesThisPersonAlreadyBelongToAPosition(existingUser);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetWorkerByUserIdAsync_ReturnsNullForNonexistentUser()
        {
            // Arrange
            var nonExistentUserId = "NonExistentUser";

            // Act
            var result = await workerService.GetWorkerByUserIdAsync(nonExistentUserId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetWorkerByUserIdAsync_ReturnsWorkerForExistingUser()
        {
            // Arrange
            var existingUser = new ApplicationUser { Id = "existingUserId" };
            await context.Workers.AddAsync(new Worker { AppUser = existingUser });
            await context.SaveChangesAsync();

            // Act
            var result = await workerService.GetWorkerByUserIdAsync(existingUser.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingUser.Id, result.AppUser.Id);
        }
    }
}
