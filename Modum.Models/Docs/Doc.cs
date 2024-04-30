using Microsoft.AspNetCore.Identity;
using Modum.Models.BaseModels.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modum.Models.Docs
{
    public class Doc : IEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";

        [ForeignKey("User")]
        public string UserId { get; set; } = "";
    }
}
