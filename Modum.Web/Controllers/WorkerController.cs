using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Modum.Models.BaseModels.Models.FooterItems;
using Modum.Models.BaseModels.Models.LTCs;
using Modum.Models.BaseModels.Models.PaymentStructure;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Models.DTO;
using Modum.Services.Services.ControllerService.WorkerController;
using Modum.Web.Models.Models.DTO;
using Modum.Web.Models.Models.Pagination;

namespace Modum.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,Worker")]

    public class WorkerController : Controller
    {
        #region Fields&Constructor

        private readonly IWorkerControllerHelper _helper;

        public WorkerController(IWorkerControllerHelper helper)
        {
            _helper =helper;
        }
        #endregion

        #region ManageBlogs

        [Authorize(Roles = "Admin,SuperAdmin,Worker")]
        public IActionResult AddABlog()
        {
            return View("~/Views/Worker/AddABlog.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> AddAPostAction(BlogPost model, List<ImageDTO> imagesDTO)
        {
            await _helper.AddABlogPostToFirebase(model, imagesDTO);

            return RedirectToAction("AddABlog", "Worker");
        }

        public async Task<IActionResult> EditBlog(Guid blogId)
        {
            var blog = await _helper.GetBlogById(blogId);

            return View("~/Views/Worker/EditBlogPost.cshtml", blog);
        }

        #endregion

        #region ManageSubSelection
        [HttpGet]
        public async Task<IActionResult> ManageSubSelection()
        {
            try
            {
                var mainCategories = await _helper.GetMainCategoriesAsyncHelper();

                if (mainCategories != null)
                {
                    SelectList mainCategoryList = new SelectList(mainCategories, "Id", "Name");
                    ViewBag.MainCategoryList = mainCategoryList;
                    return View("~/Views/Worker/ManageSubSelection.cshtml", mainCategories);
                }
                return View("~/Views/Worker/ManageSubSelection.cshtml");
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        public async Task<JsonResult> ManageSubSelection(Guid mainCategoryId, List<CategoryDTO> categoriesDTO, List<SubcategoryDTO> subcategoriesDTO)
        {

            try
            {
                if (categoriesDTO == null || subcategoriesDTO == null)
                {
                    return Json(new { status = false, Message = "Main Category,Categories and Subcategories are required" });
                }
                var username = HttpContext.User?.Identity?.Name ?? "";

                var result = await _helper.ManageSubSelectionHelper(mainCategoryId, categoriesDTO, subcategoriesDTO, username);

                if (result)
                {
                    return Json(new { status = true, Message = "All saved successfully" });
                }
                else
                {
                    return Json(new { status = false, Message = "Something went wrong while saving data" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, Message = "Unexpected error occured while saving data. " + ex.Message });
            }
        }

       
        
        #region HelperMethods
        [HttpPost]
        public async Task<JsonResult> LoadMainCategoryData(Guid mainCategoryId)
        {
            var model = await _helper.LoadMainCategoryHelper(mainCategoryId);
            return Json(model);
        }
        [HttpPost]
        public async Task<JsonResult> FilterMainCategoryData(Guid mainCategoryId, Guid categoryId)
        {
            try
            {
                var result = await _helper.FilterMainCategoryDataHelper(mainCategoryId, categoryId);

                return Json(result);

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    Message = "Unexpected error occured while saving data. " + ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetCategoryBySubcategory(Guid subcategoryId)
        {
            try
            {
                if (subcategoryId != Guid.Empty)
                {
                    var categoryId = await _helper.GetCategoryBySubcategoryAsyncHelper(subcategoryId);

                    return Json(categoryId);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    Message = "Unexpected error occured while saving data. " + ex.Message
                });
            }
            return Json(true);
        }
        #endregion

        #endregion

        #region ManageProducts
        [HttpGet]
        public async Task<IActionResult> ManageProducts(int? page, string searchString, string currentFilter)
        {
            ViewData["CurrentFilter"] = searchString;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            var products = _helper.ManageProductsJSON();
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(prdct => prdct.Product.Title.Contains(searchString));
            }
            int pageSize = 30;
            var paginatedList = PaginatedList<ProductSizesHelpingTable>.CreateAsync(products.AsNoTracking(), page ?? 1, pageSize);
            return View("~/Views/Worker/ManageProducts.cshtml", paginatedList);
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            var model = await _helper.AddProductHelper();

            return View("~/Views/Worker/AddProduct.cshtml",model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDTO productDTO, List<ImageDTO> imagesDTO)
        {
            try
            {
                var username = HttpContext.User?.Identity?.Name ?? "";

                await _helper.AddProductJSONHelper(productDTO, imagesDTO, username);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, Message = "Unexpected error occured while saving data. " + ex.Message });
            }
            return Json(true);
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(Guid productId)
        {
            try
            {
                var product = await _helper.EditProductHelper(productId);

                if (product == null)
                {
                    return BadRequest(product);
                }
                return View("~/Views/Worker/EditProduct.cshtml", product);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductDTO productDTO, List<ImageDTO> imagesDTO)
        {
            try
            {

                var username = HttpContext.User?.Identity?.Name ?? "";

                await _helper.EditProductJSONHelper(productDTO, imagesDTO, username);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, Message = "Unexpected error occured while saving data. " + ex.Message });
            }
            return Json(true);
        }

        public async Task<JsonResult> DeleteProduct(Guid productId)
        {
            try
            {
                await _helper.DeleteProductHelper(productId);
            }
            catch (Exception)
            {
                return Json(new { status = true, Message = "Error Conflicted" });
            }
            return Json(new { status = true, Message = "The product was deleted successfully" });
        }
        #endregion

        #region ManageLimitedTimeCampaigns
        [HttpGet]
        public async Task<IActionResult> ManageLTCs(int? page, string searchString, string currentFilter)
        {
            ViewData["CurrentFilter"] = searchString;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var ltcs = _helper.ManageLTCHelper();
            if (!String.IsNullOrEmpty(searchString))
            {
                ltcs = ltcs.Where(prdct => prdct.Title.Contains(searchString));
            }
            int pageSize = 30;
            var paginatedList = PaginatedList<LTC>.CreateAsync(ltcs.AsNoTracking(), page ?? 1, pageSize);
            return View("~/Views/Worker/ManageLTCs.cshtml", paginatedList);
        }

        [HttpGet]
        public async Task<IActionResult> AddALTC()
        {
            return View("~/Views/Worker/AddALTC.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> AddLTCPost(LTCDTO ltcDTO, ImageDTO imageDTO)
        {
            try
            {
                await _helper.AddLTCJSONHelper(ltcDTO, imageDTO);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, Message = "Unexpected error occured while saving data. " + ex.Message });
            }
            return Json(true);
        }

        [HttpGet]
        public async Task<IActionResult> EditLTC(Guid ltcId)
        {
            try
            {
                var ltc = await _helper.EditLTCHelper(ltcId);

                if (ltc == null)
                {
                    return BadRequest(ltc);
                }
                return View("~/Views/Worker/EditALTC.cshtml", ltc);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditLTCPost(LTCDTO ltcDTO, ImageDTO imageDTO)
        {
            try
            {
                await _helper.EditLTCJSONHelper(ltcDTO, imageDTO);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, Message = "Unexpected error occured while saving data. " + ex.Message });
            }
            return Json(true);
        }

        public async Task<JsonResult> DeleteLTC(Guid ltcId)
        {
            try
            {
                await _helper.DeleteLTCHelper(ltcId);
            }
            catch (Exception)
            {
                return Json(new { status = true, Message = "Error Conflicted" });
            }
            return Json(new { status = true, Message = "The product was deleted successfully" });
        }
        #endregion

        #region ManageOrders
        public async Task<IActionResult> ManageOrders(int? page, string searchString, string currentFilter, string orderCategory)
        {
            ViewData["CurrentFilter"] = searchString;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (String.IsNullOrEmpty(orderCategory))
            {
                orderCategory = "new";
            }
            ViewData["CurrentOrderCategory"] = orderCategory;
            var orders = _helper.GetAllOrdersFromCategory(orderCategory);
            if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(order=>order.ApplicationUser.Email.Contains(searchString));
            }
            int pageSize = 30;
            var paginatedList = PaginatedList<Order>.CreateAsync(orders.AsNoTracking(), page ?? 1, pageSize);
            return View("~/Views/Worker/ManageOrders.cshtml", orders);
        }

        [HttpPost]
        public async Task<JsonResult> ChangeDeliveryStatus(Guid orderId,string newStatus)
        {
            try
            {
                if (orderId==Guid.Empty||String.IsNullOrEmpty(newStatus))
                {
                    return Json(new { status = false, Message = "Error with parameters!" });
                }
                if (await _helper.ChangeDeliveryStatusHelper(orderId, newStatus))
                {
                    return Json(new { status = true, Message = "The order status was updated!" });
                }
                else
                {
                    return Json(new { status = false, Message = "Error with updating the information!" });
                }
            }
            catch (Exception)
            {
                return Json(new { status = false, Message = "Error Conflicted on Server-Side!" });
            }
        }
        #endregion
    }
}
