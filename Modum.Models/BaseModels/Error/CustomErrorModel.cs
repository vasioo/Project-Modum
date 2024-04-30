namespace Modum.Models.BaseModels.Error
{
    public class CustomErrorModel
    {
        public int StatusCode { get; set; }
        public string CustomErrorMessage { get; set; } = "";
    }
}
