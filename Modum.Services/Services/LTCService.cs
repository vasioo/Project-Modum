using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Modum.DataAccess;
using Modum.Models.BaseModels.Models.LTCs;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Modum.Models.BaseModels.Models.Images;
using Microsoft.EntityFrameworkCore;

namespace Modum.Services.Services
{
    public class LTCService : BaseService<LTC>, ILTCService
    {
        private readonly DataContext _context;
        public IConfiguration Configuration { get; }
        private CloudinarySettings _cloudinarySettings;
        private Cloudinary _cloudinary;

        public LTCService(DataContext context, IConfiguration configuration) : base(context)
        {
            Configuration = configuration;
            ConfigureCloudinary();
            _context = context;
        }

        public async Task ConfigureCloudinary()
        {
            _cloudinarySettings = Configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>() ?? new CloudinarySettings();
            Account account = new Account(
                _cloudinarySettings.CloudName,
                _cloudinarySettings.ApiKey,
                _cloudinarySettings.ApiSecret);
            _cloudinary = new Cloudinary(account);
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

        public async Task<LTC> GetBestLTCNow()
        {
            try
            {
                var mostCommonLTC = _context.Product
                    .SelectMany(product => product.LTCs.Where(x=>x.EndDate>DateTime.Now&&x.StartDate<DateTime.Now))
                    .GroupBy(ltc => ltc)
                    .OrderByDescending(group => group.Count())
                    .Select(group => group.Key)
                    .FirstOrDefault();

                if (mostCommonLTC != null)
                {
                    return mostCommonLTC;
                }
                else
                {
                    var alternative= await _context.LTCs.Where(x => x.EndDate > DateTime.Now && DateTime.Now > x.StartDate).OrderByDescending(x => x.StartDate).FirstOrDefaultAsync();
                    if (alternative!=null)
                    {
                        return alternative;
                    }
                }
                return new LTC();
            }
            catch (Exception)
            {
                return new LTC();
                throw;
            }
        }
    }
}
