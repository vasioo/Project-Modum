using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Models.Docs;

namespace Modum.Services.Interfaces
{
    public interface IDocsService : IBaseService<Doc>
    {
        Task<bool> SaveImage(Photo image);
    }
}
