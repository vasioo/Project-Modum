using Microsoft.AspNetCore.Mvc;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Services.Interfaces;
using System.Text;

namespace Modum.AdsWebApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class AdsApiController : ControllerBase
    {
        private readonly IAdsService _adsService;

        public AdsApiController(IAdsService adsService)
        {
            _adsService = adsService;
        }

        [HttpGet("mostAddedToFav")]
        public async Task<IActionResult> GetMostAddedToFavAdData()
        {
            var items = await _adsService.GetProductsByTenMostAddedToFavourites();

            if (items != null && items.Count() > 0)
            {
                var htmlString = await BuildProductHtml(items);
                return Content(htmlString, "text/html");
            }
            else
            {
                var htmlString = "The provided Api is not currently working!We are having problems with our database";
                return Content(htmlString, "text/html");
            }
        }

        [HttpGet("mostBought")]
        public async Task<IActionResult> GetMostBoughtAdData()
        {
            var items = await _adsService.GetProductsByTenMostBought();

            if (items!=null&&items.Count()>0)
            {
                var htmlString = await BuildProductHtml(items);
                return Content(htmlString, "text/html");
            }
            else
            {
                var htmlString = "The provided Api is not currently working!We are having problems with our database";
                return Content(htmlString, "text/html");
            }
        }

        private async Task<string> BuildProductHtml(IEnumerable<Product> items)
        {

            var htmlBuilder = new StringBuilder();

            htmlBuilder.AppendLine("<a href=\"https://modum.azurewebsites.net/\">"); 
            htmlBuilder.AppendLine("    <div id=\"ad-section\" class=\"container\">"); 
            htmlBuilder.AppendLine("        <h2 class=\"\">Modum:</h2>"); 
            htmlBuilder.AppendLine("        <div class=\"product-cards-container\">");
            htmlBuilder.AppendLine("            <div class=\"container\">");
            htmlBuilder.AppendLine("                <button class=\"arr arr-prev\" id=\"arrow-left\"></button>");
            htmlBuilder.AppendLine("                <ul class=\"cards\">");

            foreach (var product in items)
            {
                string link = $"https://res.cloudinary.com/dzaicqbce/image/upload/v1695818842/image-1-for-{product.ImageContainerId}.png";
                htmlBuilder.AppendLine("                <li class=\"card\">");
                htmlBuilder.AppendLine($"                    <a asp-area=\"\" asp-controller=\"Home\" asp-action=\"Shop\" asp-route-productId=\"{product.Id}\">");
                htmlBuilder.AppendLine($"                        <img max-width=\"300px\" height=\"300px\" class=\"card-img-top\" src=\"{link}\" alt=\"Product Image\" />");
                htmlBuilder.AppendLine("                    </a>");
                htmlBuilder.AppendLine("                    <div class=\"card-body\">");
                htmlBuilder.AppendLine($"                        <h2 class=\"card-title d-flex align-items-center justify-content-center\">{product.Brand}</h2>");
                htmlBuilder.AppendLine($"                        <h5 class=\"d-flex align-items-center justify-content-center\" style=\"color:grey\">{product.Title}</h5>");

                if (product.DiscountFromPrice > 0)
                {
                    var newValue = product.Price - product.DiscountFromPrice;
                    htmlBuilder.AppendLine("                        <div class=\"row d-flex align-items-center justify-content-center\">");
                    htmlBuilder.AppendLine($"                            <h6 class=\"\" style=\"text-decoration:line-through; color:gray\">${product.Price}</h6>");
                    htmlBuilder.AppendLine($"                            <h6 class=\"discount-container\" style=\"color:green\">- ${product.DiscountFromPrice}</h6>");
                    htmlBuilder.AppendLine("                        </div>");
                    htmlBuilder.AppendLine($"                        <h4 class=\"price-container d-flex align-items-center justify-content-center discounted-price\">${newValue}</h4>");
                }
                else
                {
                    htmlBuilder.AppendLine($"                        <h4 class=\"price-container d-flex align-items-center justify-content-center discounted-price\">${product.Price}</h4>");
                }

                htmlBuilder.AppendLine("                    </div>");
                htmlBuilder.AppendLine("                </li>");
            }

            htmlBuilder.AppendLine("                </ul>");
            htmlBuilder.AppendLine("                <button class=\"arr arr-next\" id=\"arrow-right\"></button>");
            htmlBuilder.AppendLine("            </div>");
            htmlBuilder.AppendLine("        </div>");
            htmlBuilder.AppendLine("    </div>");
            htmlBuilder.AppendLine("</a>");

            return htmlBuilder.ToString();
        }
    }
}