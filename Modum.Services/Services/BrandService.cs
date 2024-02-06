using Modum.DataAccess;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Services.Interfaces;

namespace Modum.Services.Services
{
    public class BrandService : BaseService<Brands>, IBrandService
    {
        private DataContext _dataContext;

        public BrandService(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public Task<Brands> GetBrandByStringName(string name)
        {
            var brand = _dataContext.Brands.Where(br => br.BrandName == name).FirstOrDefault();
            return Task.FromResult(brand!);
        }

        public Task<IEnumerable<Brands>> GetMostFavouriteBrandsBySoldItemsBySection(string section)
        {
            var brands = _dataContext.Brands.Where(br => br.TypeOfBrand == section).OrderBy(br => br.AmountOfTimesInFavourites).Take(20);
            return Task.FromResult(brands!.AsEnumerable());
        }
    }
}
