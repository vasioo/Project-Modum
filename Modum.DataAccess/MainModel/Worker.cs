using Modum.Models.BaseModels.Enums;
using Modum.Models.BaseModels.Interfaces;

namespace Modum.DataAccess.MainModel
{
    public class Worker:IEntity
    {
        public int Id { get; set; } = 0;
        public ApplicationUser User { get; set; } = new ApplicationUser();
        public string Position { get; set; } = "";
        public double Salary { get; set; }
        public double Bonus { get; set; }
        public DateTime TimeSinceJoiningTheCompany { get; set; } = DateTime.Now;
        public DateTime TimeSinceChangingThePosition { get; set; } = DateTime.Now;
    }
}
