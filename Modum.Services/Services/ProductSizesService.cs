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

        public Task<List<ProductSizesHelpingTable>> GetSizesByProductId(Guid productId)
        {
            var sizes = _dataContext.ProductSizesHelpingTable.Where(product => product.Product.Id == productId);
            return Task.FromResult(sizes.ToList());
        }

        public async Task<Product> GetMostBoughtProducts()
        {
            var mostSoldProducts = _dataContext.ProductSizesHelpingTable
                .GroupBy(x => x.Product)
                .Select(group => new
                {
                    Product = group.Key,
                    Difference = group.Sum(item => item.AllTimeAvailableItems) - group.Sum(item => item.AvailableItems)
                })
                .OrderByDescending(x => x.Difference)
                .FirstOrDefault();

            return mostSoldProducts.Product;

        }

        public async Task<ProductSizesHelpingTable> GetSizeBasedOnItsName(Guid productId,string name)
        {
            return _dataContext.ProductSizesHelpingTable.Where(x => x.ProductSize == name && x.Product.Id == productId).FirstOrDefault();
        }

        public async Task<List<ProductSizesHelpingTable>> GetSizesBasedOnTheProductId(Guid productId)
        {
            return _dataContext.ProductSizesHelpingTable.Where(x => x.Product.Id == productId).ToList();
        }

        public async Task<List<ProductSizesHelpingTable>> GetSizesOfProducts(List<Product> products)
        {
            var listOfSizes = new List<ProductSizesHelpingTable>();
            foreach (var product in products)
            {
                var sizes = _dataContext.ProductSizesHelpingTable.Where(x => x.Product.Id == product.Id);
                listOfSizes.AddRange(sizes);
            }
            return listOfSizes;
        }
    }
}
