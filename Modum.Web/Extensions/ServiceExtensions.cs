using Microsoft.AspNetCore.Identity;
using Modum.DataAccess;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.LTCs;
using Modum.Models.BaseModels.Models.Payment;
using Modum.Models.BaseModels.Models.PaymentStructure;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Models.MainModel;
using Modum.Services.Interfaces;
using Modum.Services.Services;
using Modum.Services.Services.ControllerService.AdminController;
using Modum.Services.Services.ControllerService.DocsController;
using Modum.Services.Services.ControllerService.HomeController;
using Modum.Services.Services.ControllerService.WorkerController;
using Modum.Web.ControllerService.FooterController;

namespace Modum.Web.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddApplication(this IServiceCollection service)
        {
            service.AddScoped<DataContext, DataContext>();

            service.AddScoped<IBaseService<Product>, BaseService<Product>>();
            service.AddScoped<IProductService, ProductService>();

            service.AddScoped<IAdsService, AdsService>();

            service.AddScoped<IBaseService<Favourites>, BaseService<Favourites>>();
            service.AddScoped<IFavouriteService, FavouriteService>();

            service.AddScoped<IBaseService<Cart>, BaseService<Cart>>();
            service.AddScoped<ICartService, CartService>();

            service.AddScoped<IBaseService<MainCategory>, BaseService<MainCategory>>();
            service.AddScoped<IMainCategoryService, MainCategoryService>();

            service.AddScoped<IBaseService<Category>, BaseService<Category>>();
            service.AddScoped<ICategoryService, CategoryService>();

            service.AddScoped<IBaseService<Subcategory>, BaseService<Subcategory>>();
            service.AddScoped<ISubcategoryService, SubcategoryService>();

            service.AddScoped<IBaseService<ShortUserModel>, BaseService<ShortUserModel>>();
            service.AddScoped<IBannedUsersService, BannedUsersService>();

            service.AddScoped<IBaseService<ProductSizesHelpingTable>, BaseService<ProductSizesHelpingTable>>();
            service.AddScoped<IProductSizesService, ProductSizesService>();

            service.AddScoped<IBaseService<LTC>, BaseService<LTC>>();
            service.AddScoped<ILTCService, LTCService>();

            service.AddScoped<IBaseService<Worker>, BaseService<Worker>>();
            service.AddScoped<IWorkerService, WorkerService>();

            service.AddScoped<IBaseService<Order>, BaseService<Order>>();
            service.AddScoped<IOrderService, OrderService>();

            service.AddScoped<IEmailSenderService, EmailSenderService>();

            service.AddScoped<IHomeControllerHelper, HomeControllerHelper>();
            service.AddScoped<IAdminControllerHelper, AdminControllerHelper>();
            service.AddScoped<IDocsControllerHelper, DocsControllerHelper>();
            service.AddScoped<IWorkerControllerHelper, WorkerControllerHelper>();
            service.AddScoped<IFooterControllerHelper, FooterControllerHelper>();

            service.AddScoped<UserManager<ApplicationUser>>();
        }
    }
}
