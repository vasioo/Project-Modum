using Modum.DataAccess;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Services.Interfaces;

namespace Modum.Services.Services
{
    public class FavouriteService : BaseService<Favourites>, IFavouriteService
    {
        private DataContext _dataContext;

        public FavouriteService(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public Task<Favourites> GetFavouritesContainerByUserId(string userId)
        {
            var userFavourites = _dataContext.Favourites.FirstOrDefault(item => item.UserId == userId);

            return Task.FromResult(userFavourites);
        }

        public Task<int> GetFavouritesIdByUserId(string userId)
        {
            var favourites = _dataContext.Favourites.FirstOrDefault(item => item.UserId == userId);

            if (favourites != null)
            {
                return Task.FromResult(favourites.Id);
            }

            return Task.FromResult(0);
        }

    }
}
