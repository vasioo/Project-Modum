using Modum.DataAccess;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Services.Interfaces;
using Modum.Services.Services;

namespace Modum.AdsWebApi.DataGetting
{
    public static class DataServiceExtensions
    {
        public static void AddData(this IServiceCollection service)
        {
            service.AddScoped<DataContext, DataContext>();

            service.AddScoped<IBaseService<Product>, BaseService<Product>>();
            service.AddScoped<IAdsService, AdsService>();
        }
    }
}
