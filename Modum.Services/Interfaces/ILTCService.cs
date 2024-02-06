using Modum.Models.BaseModels.Models.LTCs;
using Modum.Models.BaseModels.Models.ProductStructure;

namespace Modum.Services.Interfaces
{
    public interface ILTCService:IBaseService<LTC>
    {
        Task<bool> SaveImage(Photo image);
        Task<LTC> GetBestLTCNow();
    }
}
