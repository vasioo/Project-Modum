using Modum.Models.BaseModels.Models.BaseStructure;

namespace Modum.Services.Interfaces
{
    public interface IBrandService : IBaseService<Brands>
    {
        public Task<IEnumerable<Brands>> GetMostFavouriteBrandsBySoldItemsBySection(string section);
        public Task<Brands> GetBrandByStringName(string name);
    }
}
