using Modum.Models.BaseModels.Models.FooterItems;

namespace Modum.Models.ViewModels
{
    public class BlogViewModel
    {
        public IQueryable<BlogPost> BlogPosts { get; set; }
        public int CartItemsForUser { get; set; }
    }
}
