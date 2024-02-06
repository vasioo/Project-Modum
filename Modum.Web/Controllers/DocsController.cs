using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modum.Models.BaseModels.Models.FooterItems;
using Modum.Models.Docs;
using Modum.Services.Services.ControllerService.DocsController;

namespace Modum.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,Worker")]
    public class DocsController : Controller
    {
        #region ConstructorAndFields
        private readonly IDocsControllerHelper _helper;

        public DocsController(IDocsControllerHelper helper)
        {
            _helper = helper;
        }
        #endregion

        #region DocsShower
        public IActionResult DocsShower()
        {
            return View("~/Views/Docs/DocsShower.cshtml", _helper.GetAllDocuments());
        }

        #endregion

        #region CreateDocument
        public IActionResult Create()
        {
            return View("~/Views/Docs/CreateDocument.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(Doc doc, IFormFile blogImage)
        {
            try
            {
                var blog = new BlogPost();
                blog.Title = doc.Title;
                blog.DateOfCreation = DateTime.Now;
                blog.Content = doc.Content;

                using (var memoryStream = new MemoryStream())
                {
                    await blogImage.CopyToAsync(memoryStream);
                    var base64String = Convert.ToBase64String(memoryStream.ToArray());

                    // Create a data 
                    var dataUrl = $"data:{blogImage.ContentType};base64,{base64String}";
                    await _helper.SaveDocInformation(blog, dataUrl);
                }
            }
            catch (Exception ex)
            {
                return View(DocsShower());
            }
            return View("~/Views/Docs/CreateDocument.cshtml");
        }

        #endregion

        #region EditDocument
        public async Task<IActionResult> EditDocument(Guid id)
        {
            var doc = await _helper.EditDocHelper(id);

            if (doc != null)
            {
                return View("~/Views/Docs/EditDocument.cshtml", doc);
            }
            return View(DocsShower());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDocumentPost(Guid id, BlogPost doc, IFormFile blogImage)
        {
            try
            {
                if (id != doc.Id)
                {
                    return Json(new { status = false, Message = "There is not such entity in the database!" });
                }
                using (var memoryStream = new MemoryStream())
                {
                    await blogImage.CopyToAsync(memoryStream);
                    var base64String = Convert.ToBase64String(memoryStream.ToArray());

                    // Create a data 
                    var dataUrl = $"data:{blogImage.ContentType};base64,{base64String}";
                    await _helper.EditDocPostHelper(doc, dataUrl);
                }

            }
            catch (Exception ex)
            {
                return View(DocsShower());
            }
            return View(EditDocument(id));
        }
        #endregion

        #region DeleteDocument
        public async Task<IActionResult> DeleteDocument(Guid? id)
        {
            var doc = _helper.GetAllDocuments().FirstOrDefault(x => x.Id == id);

            if (doc == null)
            {
                return NotFound();
            }

            return View("~/Views/Docs/DeleteDocument.cshtml", doc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteDocumentPost(Guid id)
        {
            try
            {
                await _helper.DeleteDocPost(id);
            }
            catch (Exception ex)
            {
                return Json(new { status = false });
            }
            return Json(new { status = true });
        }
        #endregion
    }
}
