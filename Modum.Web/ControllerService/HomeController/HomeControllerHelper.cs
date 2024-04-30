using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modum.Models.BaseModels.Enums;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.Payment;
using Modum.Models.BaseModels.Models.PaymentStructure;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Models.MainModel;
using Modum.Models.ViewModels;
using Modum.Services.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace Modum.Services.Services.ControllerService.HomeController
{
    public class HomeControllerHelper : IHomeControllerHelper
    {
        #region Constructor
        private readonly IFavouriteService _favouriteService;
        private readonly IProductService _adsService;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly ICategoryService _categoryService;
        private readonly ISubcategoryService _subcategoryService;
        private readonly IMainCategoryService _mainCategoryService;
        private readonly IFirebaseService _firebaseService;
        private readonly ILTCService _ltcService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IOrderService _orderService;
        private readonly IProductSizesService _productSizesService;
        public HomeControllerHelper(
            IFavouriteService favouriteService,
            IProductService productService,
            ICartService cartService,
            ICategoryService categoryService,
            ISubcategoryService subcategoryService,
            IMainCategoryService mainCategoryService,
            UserManager<ApplicationUser> userManager,
            IFirebaseService firebaseService,
            ILTCService ltcService,
            IConfiguration configuration,
            IEmailSenderService emailSenderService,
            IOrderService orderService,
            IProductSizesService productSizesService)
        {
            _ltcService = ltcService;
            _favouriteService = favouriteService;
            _productService = productService;
            _cartService = cartService;
            _categoryService = categoryService;
            _subcategoryService = subcategoryService;
            _mainCategoryService = mainCategoryService;
            _userManager = userManager;
            _firebaseService = firebaseService;
            _configuration = configuration;
            _emailSenderService = emailSenderService;
            _orderService = orderService;
            _productSizesService = productSizesService;
        }
        #endregion

        #region ShopHelper

        public async Task<ShopViewModel> ShopHelper(string productId, ApplicationUser user)
        {
            var shopViewModel = new ShopViewModel();

            await _firebaseService.AddLastViewedProduct(user.Id, productId);

            shopViewModel.Product = await _productSizesService.GetSizesByProductId(Guid.Parse(productId));
            if (user != null)
            {
                shopViewModel.Cart = await _cartService.GetCartContainerByUserId(user.Id.ToString());
                shopViewModel.Favourites = await _favouriteService.GetFavouritesContainerByUserId(user.Id.ToString());
            }
            var userCart = await _cartService.GetCartContainerByUserId(user.Id);
            if (userCart != null)
            {
                shopViewModel.CartItemsForUser = userCart.CartItems.Count();
            }
            shopViewModel.TenFavItems = await _productService.GetProductsByTenMostAddedToFavourites();
            shopViewModel.UnderNavCategories = _categoryService.IQueryableGetAllAsync()
                .Where(x => x.MainCategoryId == _mainCategoryService.IQueryableGetAllAsync()
                    .Where(y => y.Name == "Women").Select(z => z.Id).FirstOrDefault());
            shopViewModel.BasicBrands = await _productService.GetMostFavouriteBrandsBySoldItemsBySection("normal");
            shopViewModel.PremiumBrands = await _productService.GetMostFavouriteBrandsBySoldItemsBySection("premium");

            if (user != null)
            {
                var productIds = await _firebaseService.GetLastViewedProducts(user!.Id!);
                var myNewDic = productIds.OrderByDescending(x => x.Value.Timestamp);
                var productList = await Task.WhenAll(myNewDic.Select(product => _productService.GetByIdAsync(Guid.Parse(product.Key))));
                shopViewModel.LastViewedProducts = productList.ToList();
            }


            return shopViewModel;
        }

        #endregion

        #region FavouritesHelper

        public async Task<FavouritesViewModel> FavouritesHelper(ApplicationUser user)
        {
            var viewModel = new FavouritesViewModel();

            viewModel.TenFavItems = await _productService.GetProductsByTenMostAddedToFavourites();
            viewModel.UnderNavCategories = _categoryService.IQueryableGetAllAsync()
                .Where(x => x.MainCategoryId == _mainCategoryService.IQueryableGetAllAsync()
                    .Where(y => y.Name == "Women").Select(z => z.Id).FirstOrDefault());
            viewModel.BasicBrands = await _productService.GetMostFavouriteBrandsBySoldItemsBySection("normal");
            viewModel.PremiumBrands = await _productService.GetMostFavouriteBrandsBySoldItemsBySection("premium");
            var userCart = await _cartService.GetCartContainerByUserId(user.Id);
            if (userCart != null)
            {
                viewModel.CartItemsForUser = userCart.CartItems.Count();
            }
            if (user != null && !string.IsNullOrEmpty(user.Id))
            {
                var items = await _favouriteService.GetFavouritesContainerByUserId(user.Id);

                viewModel.Cart = items != null ? await _cartService.GetCartContainerByUserId(user!.Id!) : null;

                if (items?.Products != null && items.Products.Any())
                {
                    viewModel.FavoriteProducts =await _productSizesService.GetSizesOfProducts(items.Products);
                }
            }
            else
            {
                viewModel.Cart = new Cart();
                viewModel.FavoriteProducts = new List<ProductSizesHelpingTable>();
            }
            if (user != null)
            {
                var productIds = await _firebaseService.GetLastViewedProducts(user!.Id!);
                var myNewDic = productIds.OrderByDescending(x => x.Value.Timestamp);
                var productList = await Task.WhenAll(myNewDic.Select(product => _productService.GetByIdAsync(Guid.Parse(product.Key))));
                viewModel.LastViewedProducts = productList.ToList();
            }

            return viewModel;
        }

        public async Task AddToFavouritesHelper(Guid productId, ApplicationUser user)
        {
            if (user != null)
            {
                var product = await _productService.GetByIdAsync(productId);

                if (product != null)
                {

                    var favourites = await _favouriteService.GetFavouritesContainerByUserId(user.Id) ?? new Favourites
                    {
                        UserId = user.Id,
                        Products = new List<Models.BaseModels.Models.ProductStructure.Product>()
                    };

                    if (favourites.Products == null)
                    {
                        favourites.Products = new List<Models.BaseModels.Models.ProductStructure.Product>();
                    }

                    if (!favourites.Products.Contains(product))
                    {
                        favourites.Products.Add(product);
                    }

                    if (product != null)
                    {
                        product.AmountOfTimesInFavourites++;
                        await _productService.UpdateAsync(product);
                    }

                    if (favourites.Id == Guid.Empty)
                    {
                        await _favouriteService.AddAsync(favourites);
                    }
                    else
                    {
                        await _favouriteService.UpdateAsync(favourites);
                    }
                }
            }
        }

        public async Task RemoveFromFavourites(Guid productId, bool addToCart, string size, ApplicationUser user)
        {
            var product = await _productService.GetByIdAsync(productId);
            var favouritesId = await _favouriteService.GetFavouritesIdByUserId($"{user?.Id}");
            var favourites = await _favouriteService.GetByIdAsync(favouritesId);

            if (favourites != null)
            {
                if (user != null && product != null)
                {
                    if (favourites.Products != null)
                    {
                        var itemToRemove = favourites.Products
                            .Where(pr => pr.Id == productId)
                            .FirstOrDefault();

                        if (itemToRemove != null)
                        {
                            favourites.Products.Remove(itemToRemove);
                        }
                    }

                }
                await _favouriteService.UpdateAsync(favourites);
            }
            if (product != null)
            {
                product.AmountOfTimesInFavourites--;
                await _productService.UpdateAsync(product);
            }
        }

        #endregion

        #region CartHelper

        public async Task<CartViewModel> CartHelper(ApplicationUser user)
        {
            var viewModel = new CartViewModel();

            viewModel.TenFavItems = await _productService.GetProductsByTenMostAddedToFavourites();
            viewModel.UnderNavCategories = _categoryService.IQueryableGetAllAsync()
                .Where(x => x.MainCategoryId == _mainCategoryService.IQueryableGetAllAsync()
                    .Where(y => y.Name == "Women").Select(z => z.Id).FirstOrDefault());
            viewModel.BasicBrands = await _productService.GetMostFavouriteBrandsBySoldItemsBySection("normal");
            viewModel.PremiumBrands = await _productService.GetMostFavouriteBrandsBySoldItemsBySection("premium");
            var userCart = await _cartService.GetCartContainerByUserId(user.Id);
            if (userCart != null)
            {
                viewModel.CartItemsForUser = userCart.CartItems.Count();
            }
            if (user != null && !string.IsNullOrEmpty(user.Id))
            {
                var items = await _cartService.GetCartContainerByUserId(user.Id);

                if (items != null && items.CartItems != null && items.CartItems.Any())
                {
                    viewModel.ProductSizes = items.CartItems;
                    viewModel.Products = _productService.IQueryableGetAllAsync()
                        .Where(pr => items.CartItems.Select(item => item.ProductId).Contains(pr.Id));
                }
            }
            if (user != null)
            {
                var productIds = await _firebaseService.GetLastViewedProducts(user!.Id!);
                var myNewDic = productIds.OrderByDescending(x => x.Value.Timestamp);
                if (myNewDic.Count() > 0)
                {
                    var productList = await Task.WhenAll(myNewDic.Select(product => _productService.GetByIdAsync(Guid.Parse(product.Key))));
                    viewModel.LastViewedProducts = productList.ToList();
                }
            }


            return viewModel;
        }

        public async Task AddToCartHelper(Guid productId, string size, ApplicationUser user)
        {
            if (user != null)
            {
                var product = await _productService.GetByIdAsync(productId);

                if (product != null)
                {
                    var cartItem = new CartItem { ProductId = product.Id, Size = size };
                    var cart = await _cartService.GetCartContainerByUserId(user.Id) ?? new Cart { UserId = user.Id };

                    if (cart.CartItems == null)
                    {
                        cart.CartItems = new List<CartItem>();
                    }
                    var existingCartItem = cart.CartItems.FirstOrDefault(item => item.ProductId == product.Id);

                    if (existingCartItem != null)
                    {
                        existingCartItem.Size = size;
                    }
                    else
                    {
                        cartItem.CartId = cart.Id;
                        cart.CartItems.Add(cartItem);
                    }

                    if (cart.Id == Guid.Empty)
                    {
                        await _cartService.AddAsync(cart);
                    }
                    else
                    {
                        await _cartService.UpdateAsync(cart);
                    }
                }
            }
        }

        public async Task RemoveFromCartHelper(Guid productId, string size, ApplicationUser user)
        {
            var product = await _productService.GetByIdAsync(productId);
            var cartId = await _cartService.GetCartIdByUserId($"{user?.Id}");
            var cart = await _cartService.GetByIdAsync(cartId);


            if (cart != null)
            {
                if (user != null && product != null)
                {
                    var itemToRemove = cart.CartItems
                        .Where(pr => pr.ProductId == productId && pr.Size == size)
                        .FirstOrDefault();

                    if (itemToRemove != null)
                    {
                        cart.CartItems.Remove(itemToRemove);
                    }

                }
                await _cartService.UpdateAsync(cart);
            }
        }

        #endregion

        #region GenderCallTemplateHelper
        public async Task<GenderCallTemplateViewModel> GenderCallTemplateHelper(string category, string userId)
        {
            var viewModel = new GenderCallTemplateViewModel();
            viewModel.Category = category;
            viewModel.TenFavItems = await _productService.GetProductsByTenMostAddedToFavourites();
            viewModel.MostBoughtItems = await _productService.GetProductsByTenMostBought();
            viewModel.UnderNavCategories = _categoryService.IQueryableGetAllAsync()
                .Where(x => x.MainCategoryId == _mainCategoryService.IQueryableGetAllAsync()
                    .Where(y => y.Name == category).Select(z => z.Id).FirstOrDefault());
            viewModel.BasicBrands = await _productService.GetMostFavouriteBrandsBySoldItemsBySection("normal");
            viewModel.PremiumBrands = await _productService.GetMostFavouriteBrandsBySoldItemsBySection("premium");
            viewModel.LimitedTimeCampaign = await _ltcService.GetBestLTCNow();
            viewModel.BlogPostsForTemplate = _firebaseService.GetAllBlogPosts().OrderByDescending(x => x.DateOfCreation).Take(10);
            var userCart = await _cartService.GetCartContainerByUserId(userId);
            if (userCart != null)
            {
                viewModel.CartItemsForUser = userCart.CartItems.Count();
            }
            return viewModel;
        }


        #endregion

        #region UserProductsHelper

        public async Task<_UserProductsPartialViewModel> _UserProductsPartialHelper(int? page, ProductFilter filter, string sortBy, string searchProducts, ApplicationUser user)
        {
            var viewModel = new _UserProductsPartialViewModel();
            var mainCategory = _mainCategoryService.IQueryableGetAllAsync()
                .Where(x => x.Name == filter.MainCategoryName)
                .FirstOrDefault();

            Guid mainCategoryId = mainCategory != null ? mainCategory.Id : Guid.NewGuid();

            viewModel.TenFavItems = await _productService.GetProductsByTenMostAddedToFavourites();
            viewModel.BasicBrands = await _productService.GetMostFavouriteBrandsBySoldItemsBySection("normal");
            viewModel.PremiumBrands = await _productService.GetMostFavouriteBrandsBySoldItemsBySection("premium");
            if (user != null)
            {
                var userCart = await _cartService.GetCartContainerByUserId(user.Id);
                if (userCart != null)
                {
                    viewModel.CartItemsForUser = userCart.CartItems.Count();
                }
            }
            if (page == null)
            {
                page = 1;
            }
            if (searchProducts != null)
            {
                filter.SearchString = searchProducts;
                viewModel.SearchStringContainer = searchProducts;
            }
            var products = _productService.IQueryableGetAllAsync();

            var categories = _categoryService.IQueryableGetAllAsync().Where(x => x.MainCategoryId == mainCategoryId).AsEnumerable();
            var subcategories = _subcategoryService.IQueryableGetAllAsync().Where(x => x.MainCategoryId == mainCategoryId).AsEnumerable();
            if (mainCategoryId == Guid.Empty)
            {
                categories = await _categoryService.GetAllAsync();
                subcategories = await _subcategoryService.GetAllAsync();
            }
            var currentSeason = GetCurrentSeason();

            if (mainCategoryId == Guid.Empty)
            {
                var tempdata = await _mainCategoryService.GetDefaultMainCategory();
                mainCategoryId = tempdata.Id;
            }
            if (mainCategoryId != Guid.Empty)
            {
                var mainCategoryEntity = _mainCategoryService.IQueryableGetAllAsync().Where(x => x.Name == filter.MainCategoryName).FirstOrDefault();
                viewModel.Subcategories = subcategories.Where(sub => sub.MainCategoryId == mainCategoryId);
                viewModel.Categories = categories.Where(cat => cat.MainCategoryId == mainCategoryId);
                if (mainCategoryEntity != null)
                {
                    viewModel.MainCategoryName = mainCategoryEntity.Name;
                }
            }
            if (!string.IsNullOrEmpty(filter.SelectedSubcategories))
            {
                List<Guid> selectedSubs = filter.SelectedSubcategories.Split('_').Select(Guid.Parse).ToList();
                List<Guid> subcategoryItemsSelected = new List<Guid>();

                if (selectedSubs != null && selectedSubs.Any())
                {
                    foreach (var selectedSubcategory in selectedSubs)
                    {
                        var subcategory = await _subcategoryService.GetByIdAsync(selectedSubcategory);
                        if (subcategory != null)
                        {
                            subcategoryItemsSelected.Add(subcategory.Id);
                        }
                    }
                }
                viewModel.SelectedSubcategoriesItems = subcategoryItemsSelected;
            }

            if (products.Count() > 0)
            {
                var distinctColors = products.Select(p => p.Colour).Distinct().ToList();
                var distinctBrands = products.Select(p => p.Brand).Distinct().ToList();
                var distinctLimitedTimeCampaigns = products
                    .SelectMany(p => p.LTCs.Select(ltc => ltc.Title))
                    .Distinct()
                    .ToList();

                viewModel.DistinctColours = distinctColors;
                viewModel.DistinctBrands = distinctBrands;
                viewModel.DistinctLTCs = distinctLimitedTimeCampaigns;
            }
            else
            {
                viewModel.DistinctColours = new List<string>();
                viewModel.DistinctBrands = new List<string>();
                viewModel.DistinctLTCs = new List<string>();
            }

            if (filter != null)
            {
                products = await _productService.GetProductsByFiltersAsync(filter);
                if (!string.IsNullOrEmpty(filter.Sizes))
                {
                    viewModel.FilterSizes = filter.Sizes;
                }
                if (!string.IsNullOrEmpty(filter.ProductColours))
                {
                    viewModel.FilterColors = filter.ProductColours.Split('_');
                }
                if (!string.IsNullOrEmpty(filter.Brands))
                {
                    viewModel.FilterBrands = filter.Brands.Split('_');
                }
                if (!string.IsNullOrEmpty(filter.LTCs))
                {
                    viewModel.FilterLTCs = filter.LTCs.Split('_');
                }
            }
            switch (sortBy)
            {
                case "lowest-price":
                    sortBy = "Lowest Price";
                    break;
                case "highest-price":
                    sortBy = "Highest Price";
                    break;
                case "newest":
                    sortBy = "Newest";
                    break;
                case "most-popular":
                    sortBy = "Most Popular";
                    break;
                default:
                    sortBy = "Most Popular";
                    break;
            }
            viewModel.SortBy = sortBy;
            if (filter!.MaxPrice == 0)
            {
                viewModel.MaxPrice = 1000;
            }
            else
            {
                viewModel.MaxPrice = filter.MaxPrice;

            }
            viewModel.MinPrice = filter.MinPrice;

            var sizes = Enum.GetValues(typeof(ClothesSizes));
            viewModel.Sizes = sizes;

            if (user != null)
            {
                var productIds = await _firebaseService.GetLastViewedProducts(user!.Id!);
                var myNewDic = productIds.OrderByDescending(x => x.Value.Timestamp);
                var productList = await Task.WhenAll(myNewDic.Select(product => _productService.GetByIdAsync(Guid.Parse(product.Key))));
                viewModel.LastViewedProducts = productList.ToList();
            }

            products = products
            .OrderByDescending(pr =>
                (pr.Season == currentSeason || pr.Season == "All Seasons") ? 3 :
                (pr.Season == "Spring" && currentSeason == "Winter") ||
                (pr.Season == "Summer" && currentSeason == "Spring") ||
                (pr.Season == "Autumn" && currentSeason == "Summer") ||
                (pr.Season == "Winter" && currentSeason == "Autumn") ? 2 : 1);

            viewModel.PaginateProducts(products);
            return viewModel;
        }

        private static string GetCurrentSeason()

        {
            DateTime currentDate = DateTime.Now;

            DateTime winterStart = new DateTime(currentDate.Year, 12, 1);
            DateTime winterEnd = new DateTime(currentDate.Year + 1, 3, 10);

            DateTime springStart = new DateTime(currentDate.Year, 3, 11);
            DateTime springEnd = new DateTime(currentDate.Year, 5, 20);

            DateTime summerStart = new DateTime(currentDate.Year, 5, 21);
            DateTime summerEnd = new DateTime(currentDate.Year, 8, 25);

            if (currentDate >= winterStart && currentDate <= winterEnd)
            {
                return "Winter";
            }
            else if (currentDate >= springStart && currentDate <= springEnd)
            {
                return "Spring";
            }
            else if (currentDate >= summerStart && currentDate <= summerEnd)
            {
                return "Summer";
            }
            else
            {
                return "Autumn";
            }
        }

        #endregion

        #region CheckoutHelper
        public async Task<SessionCreateOptions> CheckoutHelper(ApplicationUser user, HttpContext context, string location, string deliveryPlan)
        {
            try
            {
                var items = await _cartService.GetCartContainerByUserId(user!.Id);
               

                if (items != null && items.CartItems != null && items.CartItems.Count() > 0)
                {
                    string domain = context.Request.Scheme + "://" + context.Request.Host.Value + "/";

                    var options = new SessionCreateOptions
                    {
                        CustomerEmail = user.Email,
                        SuccessUrl = $"{domain}Home/SuccessfullPayment?paymentSuccess=false",
                        CancelUrl = domain + "Home/Cart",
                        LineItems = new List<SessionLineItemOptions>(),
                        Mode = "payment",
                        Metadata = new Dictionary<string, string>
                        {
                            { "location", location },
                        }
                    };

                    var orderedProducts = new List<string>();

                    foreach (var item in items.CartItems)
                    {
                        var product = _productService.IQueryableGetAllAsync().Where(pr => pr.Id == item.ProductId).FirstOrDefault();

                        decimal percentageToBeRemoved = 0;
                        foreach (var ltc in product.LTCs)
                        {
                            percentageToBeRemoved += ltc.PercentageOfDiscount;
                        }

                        var discFromPrice = product.DiscountFromPrice + ((product.Price + product.DiscountFromPrice) * Math.Round((percentageToBeRemoved / 100m), 2));
                        if (product != null)
                        {
                            var sessionListItem = new SessionLineItemOptions
                            {
                                PriceData = new SessionLineItemPriceDataOptions
                                {
                                    UnitAmount = (long)(product.Price * 100 - discFromPrice * 100),
                                    Currency = "USD",
                                    ProductData = new SessionLineItemPriceDataProductDataOptions
                                    {
                                        Name = $"{product.Brand}-{product.Title}"
                                    }
                                },
                                Quantity = 1,
                            };
                            options.LineItems.Add(sessionListItem);

                            var productLink = $"{domain}/Home/Shop?productId={product.Id}";

                            var productInfo = $"<a href=\"{productLink}\">{product.Brand} - {product.Title}</a>";
                            orderedProducts.Add(productInfo);
                        }
                    }

                    double deliveryFee = 5;

                    if (deliveryPlan == "custom-standart")
                    {
                        deliveryFee = 2.99;
                    }
                    else if (deliveryPlan == "custom-express")
                    {
                        deliveryFee = 5.99;
                    }
                    else if (deliveryPlan == "econt-standart")
                    {
                        deliveryFee = 2.54;
                    }
                    else if (deliveryPlan == "econt-express")
                    {
                        deliveryFee = 4.79;
                    }
                    else if (deliveryPlan == "speedy-standart")
                    {
                        deliveryFee = 2.77;
                    }
                    else if (deliveryPlan == "speedy-express")
                    {
                        deliveryFee = 4.39;
                    }
                    context.Session.SetString("delivery", deliveryPlan);
                    context.Session.SetString("location", location);
                    var deliveryFeeItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(deliveryFee * 100),
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Delivery Fee"
                            }
                        },
                        Quantity = 1,
                    };
                    options.LineItems.Add(deliveryFeeItem);

                    var service = new SessionService();
                    Session session = service.Create(options);

                    var stripeCustomer = new Customer();
                    stripeCustomer.Email = user.Email;
                    stripeCustomer.Name = $"{user.FirstName} {user.LastName}";
                    if (stripeCustomer.Shipping == null)
                    {
                        stripeCustomer.Shipping = new Shipping();
                    }

                    if (stripeCustomer.Shipping.Address == null)
                    {
                        stripeCustomer.Shipping.Address = new Address();
                    }
                    stripeCustomer.Shipping.Address.Line1 = location;
                    session.Customer = stripeCustomer;

                    context.Response.Headers.Add("Location", session.Url);
                    context.Response.StatusCode = 303;

                    return options;
                }
                else
                {
                    return new SessionCreateOptions();
                }
            }
            catch (Exception ex)
            {
                return new SessionCreateOptions();
            }
        }

        public async Task<bool> ProcessPayment(ApplicationUser user, HttpContext context)
        {
            if (user?.Id != null)
            {
                if (user?.Id != "")
                {
                    var userCart = await _cartService.GetCartContainerByUserId(user!.Id);
                    var userName = !string.IsNullOrEmpty(user.FirstName) ? user.FirstName : user.UserName;

                    var orderedProducts = new List<string>();
                    double totalValue = 0.00;
                    double priceValue = 0.00;
                    double discountValue = 0.00;

                    var productLinkEntites = new List<ProductSizesHelpingTable>();

                    if (userCart != null)
                    {
                        foreach (var cartItem in userCart.CartItems)
                        {
                            var product = await _productService.GetByIdAsync(cartItem.ProductId);

                            if (product != null)
                            {
                                productLinkEntites.Add(await _productSizesService.GetSizeBasedOnItsName(product.Id, cartItem.Size));

                                string link = $"https://res.cloudinary.com/dzaicqbce/image/upload/v1695818842/image-1-for-{product.ImageContainerId}.png";
                                decimal percentageToBeRemoved = 0;
                                foreach (var ltc in product.LTCs)
                                {
                                    percentageToBeRemoved += ltc.PercentageOfDiscount;
                                }
                                double discFromPrice = Convert.ToDouble(product.DiscountFromPrice + ((product.Price + product.DiscountFromPrice) * Math.Round((percentageToBeRemoved / 100m), 2)));
                                double prodPrice = Convert.ToDouble(product.Price) - discFromPrice;
                                var prLink = $"https://modum.azurewebsites.net/Home/Shop?productId={product.Id}";
                                prodPrice = Math.Round(prodPrice, 2);
                                discFromPrice = Math.Round(discFromPrice, 2);

                                var productInfo = $@"
                                <tr style='font-family:Arial,sans-serif'>
                                    <td align='none' style='font-size:0px;padding:8px 0;word-break:break-word;font-family:Arial,sans-serif'>
                                        <div style='font-size:13px;line-height:1;text-align:none;color:#000000;font-family:Arial,sans-serif'>
                                            <table style='width:100%;font-family:Arial,sans-serif' width='100%'>
                                                <tbody>
                                                    <tr style='font-family:Arial,sans-serif'>
                                                        <td style='font-size:12px;line-height:20px;font-weight:bold;padding:0;width:300px;font-family:Arial,sans-serif' width='300'>
                                                            Product
                                                        </td>
                                                        <td style='display:block;width:210px;font-family:Arial,sans-serif' width='210'></td>
                                                        <td style='font-size:12px;line-height:20px;font-weight:bold;padding:0;width:15%;text-align:right;white-space:nowrap;font-family:Arial,sans-serif' width='15%' align='left'>
                                                            Price
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr style='font-family:Arial,sans-serif'>
                                    <td align='none' style='font-size:0px;padding:12px 0;word-break:break-word;font-family:Arial,sans-serif'>
                                        <div style='font-size:13px;line-height:1;text-align:none;color:#000000;font-family:Arial,sans-serif'>
                                            <table style='width:100%;font-family:Arial,sans-serif' width='100%'>
                                                <tbody>
                                                    <tr style='font-family:Arial,sans-serif'>
                                                        <td rowspan='6' style='height:109px;min-width:72px;padding-right:24px;font-family:Arial,sans-serif' height='109'>
                                                            <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='border-collapse:collapse;border-spacing:0px;font-family:Arial,sans-serif'>
                                                                <tbody>
                                                                    <tr style='font-family:Arial,sans-serif'>
                                                                        <td style='width:72px;font-family:Arial,sans-serif' width='72'>
                                                                            <a href='{prLink}' style='text-decoration:underline;height:109px;min-width:72px;font-family:Arial,sans-serif;color:black' target='_blank' data-saferedirecturl='{prLink}'>
                                                                                <img height='109' src='{link}' style='border:0;display:block;outline:none;text-decoration:none;height:109px;width:100%' width='72' class='CToWUd' data-bit='iit'>
                                                                            </a>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                        <td style='font-size:12px;line-height:20px;width:200px;max-width:250px;font-family:Arial,sans-serif' width='200'>
                                                                {product.Title} - {product.Brand}
                                                        </td>
                                                        <td style='display:block;width:210px;font-family:Arial,sans-serif' width='210'></td>
                                                        <td style='width:15%;text-align:left;white-space:nowrap;font-size:12px;line-height:20px;font-weight:bold;font-family:Arial,sans-serif' width='15%' align='left'>
                                                            <div style='display:block;font-weight:900;color:#000000;text-align:right;font-size:12px;line-height:20px;font-family:Arial,sans-serif'>
                                                                {prodPrice.ToString("0.00")}&nbsp;$
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr style='white-space:nowrap;font-family:Arial,sans-serif'>
                                                        <td style='height:8px;font-family:Arial,sans-serif' height='8'></td>
                                                    </tr>
                                                    <tr style='white-space:nowrap;font-family:Arial,sans-serif'>
                                                        <td style='font-size:12px;line-height:20px;width:200px;max-width:250px;font-family:Arial,sans-serif' width='200'>
                                                            Size: {cartItem.Size}
                                                        </td>
                                                    </tr>
                                                    <tr style='white-space:nowrap;font-family:Arial,sans-serif'>
                                                        <td style='font-size:12px;line-height:20px;width:200px;max-width:250px;font-family:Arial,sans-serif' width='200'>
                                                            Colour: {product.Colour}
                                                        </td>
                                                    </tr>
                                                    <tr style='white-space:nowrap;font-family:Arial,sans-serif'>
                                                        <td style='font-size:12px;line-height:20px;width:200px;max-width:250px;font-family:Arial,sans-serif' width='200'>
                                                            Quantity: 1
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr style='width:100%;white-space:nowrap;font-family:Arial,sans-serif'>
                                    <td style='border-bottom:1px solid #ededed;font-size:12px;line-height:20px;width:200px;max-width:250px;font-family:Arial,sans-serif' width='200'></td>
                                </tr>";
                                orderedProducts.Add(productInfo);
                                totalValue += prodPrice;
                                priceValue += Convert.ToDouble(product.Price);
                                discountValue += discFromPrice;
                            }
                        }


                        var service = new ChargeService();

                        var options = new ChargeListOptions
                        {
                            Expand = new List<string> { "data.source" },
                            Limit = 100
                        };

                        var deliveryPlan = context.Session.GetString("delivery");
                        var location = context.Session.GetString("location");

                        context.Session.Remove("delivery");
                        context.Session.Remove("location");

                        var charge = service.ListAsync(options).Result.OrderByDescending(x => x.ReceiptEmail == user.Email).FirstOrDefault();

                        var orderElementForLocalDb = new Models.BaseModels.Models.PaymentStructure.Order();

                        orderElementForLocalDb.ApplicationUser = user;
                        orderElementForLocalDb.StripeReceiptURL = charge.ReceiptUrl;
                        orderElementForLocalDb.DateOfOrdering = DateTime.Now;
                        orderElementForLocalDb.TypeOfDelivery = deliveryPlan;
                        orderElementForLocalDb.DeliveryLocation = location;
                        orderElementForLocalDb.PricePaid = charge.Amount / 100M;
                        orderElementForLocalDb.Products = productLinkEntites;
                        orderElementForLocalDb.OrderStatus = "new";
                        orderElementForLocalDb.StripeId = charge.Id;
                        await _orderService.AddAsync(orderElementForLocalDb);

                        double deliveryFee = 5.00;

                        if (deliveryPlan == "custom-standart")
                        {
                            deliveryFee = 2.99;
                        }
                        else if (deliveryPlan == "custom-express")
                        {
                            deliveryFee = 5.99;
                        }
                        else if (deliveryPlan == "econt-standart")
                        {
                            deliveryFee = 2.54;
                        }
                        else if (deliveryPlan == "econt-express")
                        {
                            deliveryFee = 4.79;
                        }
                        else if (deliveryPlan == "speedy-standart")
                        {
                            deliveryFee = 2.77;
                        }
                        else if (deliveryPlan == "speedy-express")
                        {
                            deliveryFee = 4.39;
                        }
                        totalValue += deliveryFee;
                        var emailBody = $@"
                         <html>
                           <body>
                               <div style='text-align: center;'>
                                   <img src='https://res.cloudinary.com/dzaicqbce/image/upload/v1695818842/modum-transparent2-removebg-preview_s19kzz' alt='Website Logo' style='width: 40%; height: auto;' />
                               </div>
                               <div style='text-align: center; font-size: 22px; font-weight: bold; margin-top: 20px; text-decoration:none; color: black;'>
                                   {userName},<br/> Your order has been received!
                               </div>
                               <br/>
                               <div style='text-align: center; font-size: 18px; margin-top: 10px; color: black;'>
                                   <a href='{charge.ReceiptUrl}'>View Receipt(click)</a>
                               </div>
                               <br/>
                               <hr>
                               <br/>
                               <div style='background:#ffffff;background-color:#ffffff;margin:0px auto;font-family:Arial,sans-serif;max-width:864px'>
                                   <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='background:#ffffff;background-color:#ffffff;width:100%;font-family:Arial,sans-serif' width='100%' bgcolor='#ffffff'>
                                       <tbody>
                                           <tr style='font-family:Arial,sans-serif'>
                                               <td style='direction:ltr;font-size:0px;padding:32px 8px 16px 8px;text-align:center;font-family:Arial,sans-serif' align='center'>
                                                   <div class='m_-6124457663806841501mj-column-per-100' style='font-size:0px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%;font-family:Arial,sans-serif'>
                                                       <table border='0' cellpadding='0' cellspacing='0' role='presentation' width='100%' style='font-family:Arial,sans-serif'>
                                                           <tbody>
                                                               <tr style='font-family:Arial,sans-serif'>
                                                                   <td style='vertical-align:top;padding:0;font-family:Arial,sans-serif' valign='top'>
                                                                       <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='font-family:Arial,sans-serif' width='100%'>
                                                                           <tbody>
                                                                               <tr style='font-family:Arial,sans-serif'>
                                                                                   <td align='none' style='font-size:0px;padding:0px 0px 24px 0px;word-break:break-word;font-family:Arial,sans-serif'>
                                                                                       <div style='font-size:16px;font-weight:900;line-height:18px;text-align:none;color:#000000;font-family:Arial,sans-serif'>
                                                                                           Ordered Items:
                                                                                       </div>
                                                                                   </td>
                                                                               </tr>
                                                                                <hr/>
                                                                               {string.Join("", orderedProducts)}
                                                                           </tbody>
                                                                       </table>
                                                                   </td>
                                                               </tr>
                                                           </tbody>
                                                       </table>
                                                   </div>
                                                   <div style='background:#ffffff;background-color:#ffffff;margin:0px auto;font-family:Arial,sans-serif;max-width:864px'>
                                                       <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='background:#ffffff;background-color:#ffffff;width:100%;font-family:Arial,sans-serif' width='100%' bgcolor='#ffffff'>
                                                           <tbody>
                                                               <tr style='font-family:Arial,sans-serif'>
                                                                   <td style='direction:ltr;font-size:0px;padding:20px 8px 22px 8px;text-align:right;font-family:Arial,sans-serif' align='right'>
                                                                       <div class='m_-6124457663806841501mj-column-per-100' style='font-size:0px;text-align:right;direction:ltr;display:inline-block;vertical-align:top;width:100%;font-family:Arial,sans-serif'>
                                                                           <table border='0' cellpadding='0' cellspacing='0' role='presentation' width='100%' style='font-family:Arial,sans-serif'>
                                                                               <tbody>
                                                                                   <tr style='font-family:Arial,sans-serif'>
                                                                                       <td style='vertical-align:top;padding:0;font-family:Arial,sans-serif' valign='top'>
                                                                                           <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='font-family:Arial,sans-serif' width='100%'>
                                                                                               <tbody>
                                                                                                   <tr style='font-family:Arial,sans-serif'>
                                                                                                       <td align='none' style='font-size:0px;padding:0;word-break:break-word;font-family:Arial,sans-serif'>
                                                                                                           <table cellpadding='0' cellspacing='0' width='100%' border='0' style='color:#000000;font-size:13px;line-height:22px;table-layout:auto;width:100%;border:none;font-family:Arial,sans-serif'>
                                                                                                               <tbody>
                                                                                                                   <tr style='font-family:Arial,sans-serif'>
                                                                                                                       <td style='display:block;font-size:12px;line-height:20px;width:25%;font-family:Arial,sans-serif' width='25%'></td>
                                                                                                                       <td style='display:block;font-size:12px;line-height:20px;width:25%;font-family:Arial,sans-serif' width='25%'></td>
                                                                                                                       <td style='font-size:12px;line-height:20px;width:25%;font-family:Arial,sans-serif' width='25%'>
                                                                                                                           Product Prices
                                                                                                                       </td>
                                                                                                                       <td style='text-align:right;font-weight:900;font-size:12px;line-height:20px;width:25%;font-family:Arial,sans-serif' width='25%' align='right'>
                                                                                                                           {priceValue.ToString("0.00")}&nbsp;$
                                                                                                                       </td>
                                                                                                                   </tr>
                                                                                                                   <tr style='font-family:Arial,sans-serif'>
                                                                                                                       <td style='display:block;font-size:12px;line-height:20px;width:25%;font-family:Arial,sans-serif' width='25%'></td>
                                                                                                                       <td style='display:block;font-size:12px;line-height:20px;width:25%;font-family:Arial,sans-serif' width='25%'></td>
                                                                                                                       <td style='font-size:12px;line-height:20px;width:25%;font-family:Arial,sans-serif' width='25%'>
                                                                                                                           Discount
                                                                                                                       </td>
                                                                                                                       <td style='text-align:right;font-weight:900;font-size:12px;line-height:20px;width:25%;font-family:Arial,sans-serif' width='25%' align='right'>
                                                                                                                          {discountValue.ToString("0.00")}&nbsp;$
                                                                                                                       </td>
                                                                                                                   </tr>
                                                                                                                   <tr style='font-family:Arial,sans-serif'>
                                                                                                                       <td style='display:block;font-size:12px;line-height:20px;width:25%;font-family:Arial,sans-serif' width='25%'></td>
                                                                                                                       <td style='display:block;font-size:12px;line-height:20px;width:25%;font-family:Arial,sans-serif' width='25%'></td>
                                                                                                                       <td style='font-size:12px;line-height:20px;width:25%;font-family:Arial,sans-serif' width='25%'>
                                                                                                                           Delivery Fee
                                                                                                                       </td>
                                                                                                                       <td style='text-align:right;font-weight:900;font-size:12px;line-height:20px;width:25%;font-family:Arial,sans-serif' width='25%' align='right'>
                                                                                                                          {deliveryFee.ToString("0.00")}&nbsp;$
                                                                                                                       </td>
                                                                                                                   </tr>
                                                                                                                   <tr style='font-family:Arial,sans-serif'>
                                                                                                                       <td style='display:block;font-size:12px;line-height:20px;width:25%;font-family:Arial,sans-serif' width='25%'></td>
                                                                                                                       <td style='display:block;font-size:12px;line-height:20px;width:25%;font-family:Arial,sans-serif' width='25%'></td>
                                                                                                                       <td style='font-weight:900;width:25%;font-size:16px;line-height:18px;padding-top:27px;font-family:Arial,sans-serif' width='25%'>Total:</td>
                                                                                                                       <td style='text-align:right;font-weight:900;width:25%;font-size:16px;line-height:18px;padding-top:27px;font-family:Arial,sans-serif' width='25%' align='right'>
                                                                                                                           <div style='font-family:Arial,sans-serif'>{totalValue.ToString("0.00")}&nbsp;$</div>
                                                                                                                       </td>
                                                                                                                   </tr>
                                                                                                               </tbody>
                                                                                                           </table>
                                                                                                       </td>
                                                                                                   </tr>
                                                                                               </tbody>
                                                                                           </table>
                                                                                       </td>
                                                                                   </tr>
                                                                               </tbody>
                                                                           </table>
                                                                       </div>
                                                                   </td>
                                                               </tr>
                                                           </tbody>
                                                       </table>
                                                   </div>
                                               </td>
                                           </tr>
                                       </tbody>
                                   </table>
                               </div>
                           </body>
                        </html>";

                        var subjectText = "Order Confirmation";

                        _emailSenderService.SendEmail(user!.Email!, emailBody, subjectText);

                        if (userCart != null)
                        {
                            await _cartService.RemoveAsync(userCart.Id);
                        }
                        user.NumberOfCardTransactions++;
                        await _userManager.UpdateAsync(user);

                        return true;
                    }
                }

                return false;
            }

            return false;
        }

        #endregion

        #region RecurringJobs
        public async Task RecurringJobs()
        {
            RecurringJob.AddOrUpdate(() => _productService.DailyCheckForLTC(), "0 14 * * *", TimeZoneInfo.Local);
        }
        #endregion
    }
}
