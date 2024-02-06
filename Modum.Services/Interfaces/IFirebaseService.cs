using FireSharp.Interfaces;
using Modum.Models.BaseModels.Models.FooterItems;
using Modum.Models.BaseModels.Models.MongoDb;

namespace Modum.Services.Interfaces
{
    public interface IFirebaseService
    {
        IFirebaseClient Client { get; }

        Task<Dictionary<string, LastViewedProduct>> GetLastViewedProducts(string userId);
        Task AddLastViewedProduct(string userId, string productId);
        Task<Guid> AddABlogPost(BlogPost post);
        IQueryable<BlogPost> GetAllBlogPosts();
        Task<BlogPost> GetBlogPostById(Guid postId);
        Task<Guid> UpdateABlogPostAsync(Guid id,string filePath);
        Task RemoveABlogPostAsync(Guid id);
        Task<bool> DeleteImage(string link);
    }
}
