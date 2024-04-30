namespace Modum.Models.ViewModels
{
    public class StatisticsViewModel
    {
        public int AmountOfUsers { get; set; }
        public int AmountOfWorkers { get; set; }
        public int AmountOfAdmins { get; set; }
        public double TotalIncomeBeforeTax { get; set; }
        public double StripeTakeaway { get; set; }
        
        public string MostBoughtCategory { get; set; } = "";
        public string MostBoughtSubcategory { get; set; } = "";
        public string MostBoughtMaincategory { get; set; } = "";
        public int AmountOfBlogsInApplication { get; set; }
    }
}
