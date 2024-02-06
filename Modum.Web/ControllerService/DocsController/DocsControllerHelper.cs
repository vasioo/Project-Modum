using Microsoft.EntityFrameworkCore;
using Modum.Models.BaseModels.Models.FooterItems;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Services.Interfaces;

namespace Modum.Services.Services.ControllerService.DocsController
{
    public class DocsControllerHelper : IDocsControllerHelper
    {
        private readonly IDocsService _docsService;
        private readonly IFirebaseService _firebaseService;

        public DocsControllerHelper(IDocsService docsService, IFirebaseService firebaseService)
        {
            _docsService = docsService;
            _firebaseService = firebaseService;
        }

                
        public async Task<bool> SaveDocInformation(BlogPost doc, string fileName)
        {
            try
            {
                var id = await _firebaseService.AddABlogPost(doc);
                var photo = new Photo();
                if (fileName != null && fileName != "")
                {
                    photo.Image = fileName;
                    photo.ImageName = $"main-image-for-blog-{id}";
                    photo.PublicId = $"main-image-for-blog-{id}";
                }

                await _docsService.SaveImage(photo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IQueryable<BlogPost> GetAllDocuments()
        {
            return _firebaseService.GetAllBlogPosts();
        }

        public async Task<BlogPost> EditDocHelper(Guid id)
        {
            return await _firebaseService.GetBlogPostById(id);
        }

        public async Task<bool> EditDocPostHelper(BlogPost doc, string blogImage)
        {
            try
            {
                var id= await _firebaseService.UpdateABlogPostAsync(doc.Id,blogImage);
                await _firebaseService.DeleteImage(blogImage);
                var photo = new Photo();
                if (blogImage != null && blogImage != "")
                {
                    photo.Image = blogImage;
                    photo.ImageName = $"main-image-for-blog-{id}";
                    photo.PublicId = $"main-image-for-blog-{id}";
                }
                await _docsService.SaveImage(photo);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocExists(doc.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;
        }

        private bool DocExists(Guid id)
        {
            return _firebaseService.GetAllBlogPosts().Any(x => x.Id == id);
        }

        public async Task<string> DeleteDocPost(Guid id)
        {
            var doc =await _firebaseService.GetBlogPostById(id);

            if (doc!=null)
            {
                await _firebaseService.RemoveABlogPostAsync(doc.Id);
                return "";
            }
            return "The entity is not present in the database";
        }
    }
}
