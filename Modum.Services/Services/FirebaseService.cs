using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FireSharp.Config;
using FireSharp.Interfaces;
using Microsoft.Extensions.Configuration;
using Modum.Models.BaseModels.Models.FooterItems;
using Modum.Models.BaseModels.Models.Images;
using Modum.Models.BaseModels.Models.MongoDb;
using Modum.Services.Interfaces;
using Newtonsoft.Json;

namespace Modum.Services.Services
{
    public class FirebaseService : IFirebaseService
    {
        private static FirebaseService _instance;
        private readonly IFirebaseClient _client;
        public IConfiguration Configuration { get; }
        private CloudinarySettings _cloudinarySettings;
        private Cloudinary _cloudinary;

        private FirebaseService(IConfiguration configuration)
        {
            Configuration = configuration;
            _cloudinarySettings = Configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>() ?? new CloudinarySettings();
            Account account = new Account(
                _cloudinarySettings.CloudName,
            _cloudinarySettings.ApiKey,
                _cloudinarySettings.ApiSecret);
            _cloudinary = new Cloudinary(account);
            var authSecret = configuration.GetSection("Firebase:AuthSecret").Value;
            var basePath = configuration.GetSection("Firebase:BasePath").Value;

            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = authSecret,
                BasePath = basePath
            };

            _client = new FireSharp.FirebaseClient(config);
        }

        public static FirebaseService Instance(IConfiguration configuration)
        {
            if (_instance == null)
            {
                _instance = new FirebaseService(configuration);
            }
            return _instance;
        }

        public IFirebaseClient Client => _client;

        public async Task<Dictionary<string, LastViewedProduct>> GetLastViewedProducts(string userId)
        {
            var response = _client.Get($"lastViewedProducts/{userId}");

            if (response == null || response.StatusCode != System.Net.HttpStatusCode.OK || response.Body == "null")
            {
                return new Dictionary<string, LastViewedProduct>();
            }

            try
            {
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, LastViewedProduct>>(response.Body);
                return data;
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                return new Dictionary<string, LastViewedProduct>();
            }
        }

        public async Task AddLastViewedProduct(string userId, string productId)
        {
            var existingProducts = await GetLastViewedProducts(userId);
            var existingProduct = existingProducts.FirstOrDefault(p => p.Key == $"{productId}");

            if (existingProduct.Value != null)
            {
                await RemoveLastViewedProduct(userId, existingProduct.Key);
            }
            if (existingProducts.Count >= 10)
            {
                var oldestProduct = existingProducts.OrderBy(p => p.Value.Timestamp).First();
                await RemoveLastViewedProduct(userId, oldestProduct.Key);
            }
            var data = new LastViewedProduct
            {
                Timestamp = DateTime.Now
            };

            await _client.SetAsync($"lastViewedProducts/{userId}/{productId}", data);
        }

        private async Task RemoveLastViewedProduct(string userId, string productId)
        {
            await _client.DeleteAsync($"lastViewedProducts/{userId}/{productId}");
        }

        public async Task<Guid> AddABlogPost(BlogPost post)
        {
            try
            {
                await _client.SetAsync($"blogposts/{post.Id}", post);
                return post.Id;
            }
            catch (ArgumentException ex)
            {
                throw new Exception("The item couldn't be saved", ex);
            }

        }

        public async Task<int> GetAllBlogPostsCount()
        {
            var response = _client.Get("blogposts/");

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                dynamic data = response.ResultAs<dynamic>();

                int itemCount = 0;
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        itemCount++;
                    }
                    return itemCount;
                }
            }
            return 0;
        }

        public IQueryable<BlogPost> GetAllBlogPosts()
        {
            var response = _client.Get("blogposts/");

            if (response == null || response.Body == null)
            {
                return Enumerable.Empty<BlogPost>().AsQueryable();
            }

            var data = response.ResultAs<Dictionary<Guid, BlogPost>>();

            if (data == null)
            {
                return Enumerable.Empty<BlogPost>().AsQueryable();
            }

            return data.Values.AsQueryable();
        }

        public async Task<BlogPost> GetBlogPostById(Guid postId)
        {
            try
            {
                var documentPath = $"blogposts/{postId}";

                var response = await _client.GetAsync(documentPath);
                if (response == null || response.Body == null)
                {
                    return null;
                }

                return response.ResultAs<BlogPost>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving the blog post", ex);
            }
        }

        public async Task<Guid> UpdateABlogPostAsync(Guid id,BlogPost newItem, string filePath)
        {
            var item = await GetBlogPostById(id);
            if (item!=null)
            {
                await _client.DeleteAsync($"blogposts/{item.Id}");
            }
            newItem.Id = id;
            await DeleteImage($"https://res.cloudinary.com/dzaicqbce/image/upload/v1695818842/main-image-for-blog-{id}.png");
            return await AddABlogPost(newItem);
        }

        public async Task RemoveABlogPostAsync(Guid id)
        {
            var item = await GetBlogPostById(id);
            if (item != null)
            {
                await _client.DeleteAsync($"blogposts/{item.Id}");
            }
            await DeleteImage($"https://res.cloudinary.com/dzaicqbce/image/upload/v1695818842/main-image-for-blog-{id}.png");
        }

        public async Task<bool> DeleteImage(string link)
        {
            try
            {
                DeletionParams deletionParams = new DeletionParams(link);
                DeletionResult deletionResult = _cloudinary.Destroy(deletionParams);

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
