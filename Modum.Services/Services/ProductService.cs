﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Modum.DataAccess;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.Images;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Services.Interfaces;

namespace Modum.Services.Services
{
    public class ProductService : BaseService<Product>, IProductService
    {
        public IConfiguration Configuration { get; }
        private CloudinarySettings _cloudinarySettings;
        private Cloudinary _cloudinary;
        private readonly DataContext _dataContext;
        public ProductService(IConfiguration configuration, DataContext context) : base(context)
        {
            Configuration = configuration;
            ConfigureCloudinary();
            _dataContext = context;
        }
        public async Task ConfigureCloudinary()
        {
            _cloudinarySettings = Configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>() ?? new CloudinarySettings();
            Account account = new Account(
                _cloudinarySettings.CloudName,
                _cloudinarySettings.ApiKey,
                _cloudinarySettings.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }


        public Task<IEnumerable<Product>> GetProductsByAvailableSizesAsync(IEnumerable<string> availableSizes)
        {
            var matchingProducts = _dataContext.Product
                .Where(product => product.ProductSizes.Any(ps => availableSizes.Contains(ps.ProductSize.ToString())))
                .ToList();

            return Task.FromResult(matchingProducts.AsEnumerable());
        }


        public Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = _dataContext.Product.Where(product => product.CategoryId == categoryId);
            return Task.FromResult(products.AsEnumerable());
        }

        public Task<IEnumerable<Product>> GetProductsByMainCategoryAsync(int mainCategoryId)
        {
            var products = _dataContext.Product.Where(product => product.MainCategoryId == mainCategoryId);
            return Task.FromResult(products.AsEnumerable());
        }

        public Task<IEnumerable<Product>> GetProductsByPriceAsync(decimal price)
        {
            var products = _dataContext.Product.Where(product => product.Price == price);
            return Task.FromResult(products.AsEnumerable());
        }

        public Task<IEnumerable<Product>> GetProductsBySubcategoryAsync(int subcategoryId)
        {
            var products = _dataContext.Product.Where(product => product.SubcategoryId == subcategoryId);
            return Task.FromResult(products.AsEnumerable());
        }

        public Task<IEnumerable<Product>> GetProductsByTitleAsync(string title)
        {
            var products = _dataContext.Product.Where(product => product.Title == title);
            return Task.FromResult(products.AsEnumerable());
        }

        public Task<IEnumerable<Product>> GetProductsByWhoUploadedThemAsync(string username)
        {
            var products = _dataContext.Product.Where(product => product.UploadedBy == username);
            return Task.FromResult(products.AsEnumerable());
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

        public Task<IQueryable<Product>> GetProductsByFiltersAsync(ProductFilter filter)
        {
            IQueryable<Product> query = _dataContext.Product;

            List<string> productColours = new List<string>();
            List<string> brands = new List<string>();
            List<string> sizes = new List<string>();
            if (filter.SearchString != "" && filter.SearchString != null)
            {
                query = query.Where(pr => pr.Title.Contains(filter.SearchString) ||
                pr.Brand.Contains(filter.SearchString) ||
                pr.Colour.Contains(filter.SearchString));
            }
            if (filter.ProductColours != "" && filter.ProductColours != null)
            {
                productColours = filter.ProductColours.Split('_').ToList();
            }
            if (filter.Brands != "" && filter.Brands != null)
            {
                brands = filter.Brands.Split('_').ToList();
            }
            if (filter.Sizes != "" && filter.Sizes != null)
            {
                sizes = filter.Sizes.Split('_').ToList();
            }
            if (filter.MainCategoryId > 0)
            {
                query = query.Where(product => product.MainCategoryId == filter.MainCategoryId);
            }

            if (filter.SelectedSubcategories != null && filter.SelectedSubcategories.Any())
            {
                List<int> selectedSubs = filter.SelectedSubcategories.Split('_').Select(int.Parse).ToList();
                query = query.Where(product => selectedSubs.Contains(product.SubcategoryId));
            }

            if (productColours != null && productColours.Any() && productColours[0] != "")
            {
                query = query.Where(product => productColours.Contains(product.Colour));
            }

            if (filter.MinPrice > 0)
            {
                query = query.Where(product => product.Price - product.DiscountFromPrice >= filter.MinPrice);
            }

            if (filter.MaxPrice > 0)
            {

                query = query.Where(product => product.Price - product.DiscountFromPrice <= filter.MaxPrice);
            }
            if (brands != null && brands.Any() && brands[0] != "")
            {
                query = query.Where(product => brands.Contains(product.Brand));
            }

            if (sizes != null && sizes.Any() && sizes[0] != "")
            {
                var enumSizes = sizes.ToList(); // Assuming sizes is a collection of strings

                var matchingProductIds = _dataContext.ProductSizesHelpingTable
                    .Where(size => enumSizes.Contains(size.ProductSize))
                    .Select(size => size.Product.Id)
                    .Distinct()
                    .ToList();

                query = query.Where(product => matchingProductIds.Contains(product.Id));
            }
            if (filter.CategoryId > 0)
            {
                query = query.Where(product => product.CategoryId == filter.CategoryId);
            }
            return Task.FromResult(query);
        }

        public async Task<bool> SaveImages(List<Photo> images)
        {
            try
            {
                foreach (var image in images)
                {
                    await _cloudinary.UploadAsync(new ImageUploadParams()
                    {
                        File = new FileDescription(image.Image),
                        DisplayName = image.ImageName,
                        PublicId = image.PublicId,
                        Overwrite = false,
                    });
                }

                return true;
            }
            catch (Exception)
            {
                // log error
                return false;
            }
        }

        public async Task DailyCheckForLTC()
        {
            var currentDate = DateTime.Now;

            var expiredLTCs = _dataContext.LTCs.Where(ltc => ltc.EndDate < currentDate).ToList();

            foreach (var expiredLTC in expiredLTCs)
            {
                _dataContext.LTCs.Remove(expiredLTC);
            }

            await _dataContext.SaveChangesAsync();
        }
    }
}