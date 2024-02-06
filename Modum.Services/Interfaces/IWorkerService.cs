using Modum.DataAccess.MainModel;

namespace Modum.Services.Interfaces
{
    public interface IWorkerService:IBaseService<Worker>
    {
        Task<bool> DoesThisPersonAlreadyBelongToAPosition(ApplicationUser user);
        Task<Worker> GetWorkerByUserIdAsync(string userId);
    }
}
