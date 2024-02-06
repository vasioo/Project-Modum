using Modum.Models.BaseModels.Models.FooterItems;

namespace Modum.Services.Services.ControllerService.DocsController
{
    public interface IDocsControllerHelper
    {
        IQueryable<BlogPost> GetAllDocuments();
        Task<BlogPost> EditDocHelper(Guid id);
        Task<bool> EditDocPostHelper(BlogPost doc, string fileName);
        Task<string> DeleteDocPost(Guid id);
        Task<bool> SaveDocInformation(BlogPost doc, string fileName);
    }
}
