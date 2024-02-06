using Modum.Models.BaseModels.Enums;
using Modum.Models.BaseModels.Interfaces;

namespace Modum.Models.BaseModels.Models.BaseStructure
{
    public class MostFollowedCategory : IEntity
    {
        public int Id { get; set; } = 0;

        public int UserId { get; set; } = 0;

        /// <summary>
        /// The 30 percentile of most followed items by the user
        /// That includes the main category which will contain the  subcategories for specification
        /// </summary>
        public CategoryType UserBasedMostFollowedCategoryItems { get; set; }

        public SubcategoryType UserBasedMostFollowedSubcategoryItems { get; set; }

        /// <summary>
        /// The 18 percentile of most followed items by the user
        /// That includes the main category which will contain the  subcategories for specification
        /// </summary>
        public CategoryType UserBasedSecondMostFollowedCategoryItems { get; set; }
        public SubcategoryType UserBasedSecondMostFollowedSubcategoryItems { get; set; }

        /// <summary>
        /// The 14 percentile of most followed items by the user
        /// That includes the main category which will contain the  subcategories for specification
        /// </summary>
        public CategoryType UserBasedThirdMostFollowedCategoryItems { get; set; }
        public SubcategoryType UserBasedThirdMostFollowedSubcategoryItems { get; set; }

        /// <summary>
        /// The 10 percentile of most followed items by the user
        /// That includes the main category which will contain the  subcategories for specification
        /// </summary>
        public CategoryType UserBasedFourthMostFollowedCategoryItems { get; set; }
        public SubcategoryType UserBasedFourthMostFollowedSubcategoryItems { get; set; }

        //The other 28 percents are for other->20% and for the opposite gender->8%


    }
}
