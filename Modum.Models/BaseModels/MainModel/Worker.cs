using Modum.Models.BaseModels.Interfaces;

namespace Modum.Models.MainModel
{
    public class Worker:IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public ApplicationUser AppUser { get; set; } = new ApplicationUser();
        public string Position { get; set; } = "";
        public double Salary { get; set; }
        public double Bonus { get; set; }
        public DateTime TimeSinceJoiningTheCompany { get; set; } = DateTime.Now;
        public DateTime TimeSinceChangingThePosition { get; set; } = DateTime.Now;
    }
}
