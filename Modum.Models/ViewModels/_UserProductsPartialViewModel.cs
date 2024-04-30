using Modum.Models.BaseModels.Enums;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Web.Models.Models.Pagination;

namespace Modum.Models.ViewModels
{
    public class _UserProductsPartialViewModel
    {
        public PaginatedList<Product> PaginatedProducts { get; set; }
        public IEnumerable<Product> TenFavItems { get; set; } = new List<Product>();
        public IEnumerable<Category> UnderNavCategories { get; set; } = new List<Category>();
        public List<string> BasicBrands { get; set; } = new List<string>();
        public List<string> PremiumBrands { get; set; } = new List<string>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
        public IEnumerable<Product> LastViewedProducts { get; set; } = Enumerable.Empty<Product>();
        public int CartItemsForUser { get; set; }

        public Array Sizes { get; set; } = Enum.GetValues(typeof(ClothesSizes));
        public List<Guid> SelectedSubcategoriesItems { get; set; } = new List<Guid>();
        public List<string> DistinctColours { get; set; } = new List<string>();
        public List<string> DistinctBrands { get; set; } = new List<string>();
        public List<string> DistinctLTCs { get; set; } = new List<string>();

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => PaginatedProducts?.TotalPages ?? 0;

        public string MainCategoryName { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }

        public string SearchStringContainer { get; set; } = "";
        public string SortBy { get; set; } = "";
        public string FilterSizes { get; set; } = "";
        public string[] FilterColors { get; set; } = new string[0];
        public string[] FilterBrands { get; set; } = new string[0];
        public string[] FilterLTCs { get; set; } = new string[0];

        public _UserProductsPartialViewModel()
        {
            PageSize = 30;
            CurrentPage = 1;
        }

        public void PaginateProducts(IQueryable<Product> products)
        {
            PaginatedProducts = PaginatedList<Product>.CreateAsync(products, CurrentPage, PageSize);
        }
    }
}
