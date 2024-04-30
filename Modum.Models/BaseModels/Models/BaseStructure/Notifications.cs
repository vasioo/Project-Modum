using Modum.Models.BaseModels.Enums;
using Modum.Models.BaseModels.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Modum.Models.BaseModels.Models.BaseStructure
{
    public class Notifications : IEntity
    {
        [Required]
        public Guid Id { get; set; } 

        [Required]
        public NotificationCause NotificationCause { get; set; } = NotificationCause.NotDefined;

        [Required]
        public GroupOfPeople GroupOfPeople { get; set; } = GroupOfPeople.Admin;

        [Required]
        public DateTime TimeSent { get; set; } = DateTime.Now;

        [Required]
        public string Title { get; set; } = "";

        [Required]
        public string Image { get; set; } = "";

        [Required]
        public string ShortDescription { get; set; } = "";

    }
}
