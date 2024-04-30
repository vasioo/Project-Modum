using Modum.Models.BaseModels.Models.FooterItems;

namespace Modum.Models.ViewModels
{
    public class BlogPostViewModel
    {
        public BlogPost Post { get; set; } = new BlogPost();
        public int CartItemsForUser { get; set; }
    }
}
