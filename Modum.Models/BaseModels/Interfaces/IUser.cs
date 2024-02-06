using Modum.Models.BaseModels.Enums;

namespace Modum.Models.BaseModels.Interfaces
{
    public interface IUser
    {
        public String? SecondaryPhone { get; set; }
        public String Street { get; set; }
        public String City { get; set; }
        public String PostalCode { get; set; }
        public String District { get; set; }
        public String Province { get; set; }
        public String Country { get; set; }
        public Int64 NumberOfCardTransactions { get; set; }
        public Double TotalMoneySpent { get; set; }
        public MembershipType MembershipType { get; set; }
        public DateTime AccountOriginDate { get; set; }
        public DateTime LastOrderDate { get; set; }
        public Int64 MostFollowedCategoryId { get; set; }
        public GenderType Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
