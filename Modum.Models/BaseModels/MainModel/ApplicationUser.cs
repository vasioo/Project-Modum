using Microsoft.AspNetCore.Identity;
using Modum.Models.BaseModels.Enums;
using Modum.Models.BaseModels.Interfaces;

namespace Modum.Models.MainModel
{
    public class ApplicationUser : IdentityUser, IUser
    {
        public String? SecondaryPhone { get; set; }
        public String Street { get; set; } = string.Empty;
        public String City { get; set; } = string.Empty;
        public String PostalCode { get; set; } = string.Empty;
        public String District { get; set; } = string.Empty;
        public String Province { get; set; } = string.Empty;
        public String Country { get; set; } = string.Empty;
        public Int64 NumberOfCardTransactions { get; set; } = 0;
        public Double TotalMoneySpent { get; set; } = 0;
        public MembershipType MembershipType { get; set; } = MembershipType.NonPaymentActiveMember;
        public DateTime AccountOriginDate { get; set; } = DateTime.UtcNow;
        public DateTime LastOrderDate { get; set; }
        public Int64 MostFollowedCategoryId { get; set; }
        public GenderType Gender { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime BirthDate { get; set; }
    }
}
