using Microsoft.EntityFrameworkCore;
using Modum.DataAccess;
using Modum.DataAccess.MainModel;
using Modum.Services.Interfaces;

namespace Modum.Services.Services
{
    public class WorkerService:BaseService<Worker>,IWorkerService
    {
        private DataContext _dataContext;

        public WorkerService(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public async Task<bool> DoesThisPersonAlreadyBelongToAPosition(ApplicationUser user)
        {
            return _dataContext.Workers.Where(x=>x.User.Id==user.Id).Any();
        }

        public async Task<Worker> GetWorkerByUserIdAsync(string userId)
        {
            return await _dataContext.Workers.Where(x => x.User.Id == userId).FirstOrDefaultAsync();
        }
    }
}
