using Modum.Models.BaseModels.Models.BaseStructure;

namespace Modum.Services.Interfaces
{
    public interface IFavouriteService : IBaseService<Favourites>
    {
        Task<Favourites> GetFavouritesContainerByUserId(string userId);
        Task<Guid> GetFavouritesIdByUserId(string userId);
    }
}
