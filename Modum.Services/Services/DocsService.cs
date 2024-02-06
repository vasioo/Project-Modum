using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Modum.DataAccess;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Models.Docs;
using Modum.Services.Interfaces;
using Modum.Models.BaseModels.Models.Images;
using Microsoft.Extensions.Configuration;

namespace Modum.Services.Services
{
    public class DocsService : BaseService<Doc>, IDocsService
    {
        private readonly DataContext _context;
        public IConfiguration Configuration { get; }
        private CloudinarySettings _cloudinarySettings;
        private Cloudinary _cloudinary;

        public DocsService(DataContext context,IConfiguration configuration) : base(context)
        {
            Configuration = configuration;
            _cloudinarySettings = Configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>() ?? new CloudinarySettings();
            Account account = new Account(
                _cloudinarySettings.CloudName,
            _cloudinarySettings.ApiKey,
                _cloudinarySettings.ApiSecret);
            _cloudinary = new Cloudinary(account);
            _context = context;
        }
        public async Task<bool> SaveImage(Photo image)
        {
            try
            {
                await _cloudinary.UploadAsync(new ImageUploadParams()
                {
                    File = new FileDescription(image.Image),
                    DisplayName = image.ImageName,
                    PublicId = image.PublicId,
                    Overwrite = false,
                });

                return true;
            }
            catch (Exception)
            {
                // log error
                return false;
            }
        }
    }
}
