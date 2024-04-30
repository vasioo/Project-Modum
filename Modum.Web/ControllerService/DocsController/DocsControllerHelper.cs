using Microsoft.EntityFrameworkCore;
using Modum.Models.BaseModels.Models.FooterItems;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Services.Interfaces;
using System.Web;

namespace Modum.Services.Services.ControllerService.DocsController
{
    public class DocsControllerHelper : IDocsControllerHelper
    {
        private readonly IFirebaseService _firebaseService;
        private readonly IProductService _productService;

        public DocsControllerHelper(IFirebaseService firebaseService, IProductService productService)
        {
            _firebaseService = firebaseService;
            _productService = productService;
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

                await _productService.SaveImage(photo);
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
                var id= await _firebaseService.UpdateABlogPostAsync(doc.Id,doc,blogImage);
                await _firebaseService.DeleteImage(blogImage);
                var photo = new Photo();
                if (blogImage != null && blogImage != "")
                {
                    photo.Image = blogImage;
                    photo.ImageName = $"main-image-for-blog-{id}";
                    photo.PublicId = $"main-image-for-blog-{id}";
                }
                await _productService.SaveImage(photo);
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
