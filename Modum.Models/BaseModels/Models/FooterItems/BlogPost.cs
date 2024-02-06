using Modum.Web.Models.Models.DTO;

namespace Modum.Models.BaseModels.Models.FooterItems
{
    public class BlogPost
    {
        public Guid Id { get; set; }= Guid.NewGuid();
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime DateOfCreation { get; set; } = DateTime.Now;
    }
}
