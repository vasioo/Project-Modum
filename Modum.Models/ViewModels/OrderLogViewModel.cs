using Modum.Models.BaseModels.Models.BaseStructure;
using Stripe;

namespace Modum.Models.ViewModels
{
    public class OrderLogViewModel
    {
        public Charge Charge { get; set; } = new Charge();
    }
}
