using Modum.DataAccess;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Services.Interfaces;

namespace Modum.Services.Services
{
    public class ProductSizesService : BaseService<ProductSizesHelpingTable>, IProductSizesService
    {
        private DataContext _dataContext;

        public ProductSizesService(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public Task<IEnumerable<ProductSizesHelpingTable>> GetSizesByProductId(int productId)
        {
            var sizes = _dataContext.ProductSizesHelpingTable.Where(product => product.Product.Id == productId);
            return Task.FromResult(sizes.AsEnumerable());
        }
    }
}
