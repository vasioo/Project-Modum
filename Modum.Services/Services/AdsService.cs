using Microsoft.EntityFrameworkCore;
using Modum.DataAccess;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Services.Interfaces;

namespace Modum.Services.Services
{
    public class AdsService : BaseService<Product>, IAdsService
    {
        private readonly DataContext _dataContext;

        public AdsService(DataContext dataContext):base(dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IEnumerable<Product>> GetProductsByTenMostBought()
        {
            return await _dataContext.Product
                .OrderByDescending(x => x.ProductSizes.Max(ps => ps.AllTimeAvailableItems - ps.AvailableItems))
                .Take(10)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByTenMostAddedToFavourites()
        {
            return await _dataContext.Product
                .OrderByDescending(x => x.AmountOfTimesInFavourites)
                .Take(10)
                .ToListAsync();
        }
    }
}
