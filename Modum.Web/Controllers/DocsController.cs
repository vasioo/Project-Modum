using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<JsonResult> CreatePost(Doc doc, string blogImage)
        {
            try
            {
                var blog = new BlogPost();
                blog.Title = doc.Title;
                blog.DateOfCreation = DateTime.Now;
                blog.Content = doc.Content;

                await _helper.SaveDocInformation(blog, blogImage);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, Message = "An error occured!" });
            }
            return Json(new { status = true, Message = "The entity was saved in the database!" });
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
        public async Task<JsonResult> EditDocumentPost(BlogPost doc, string blogImage)
        {
            try
            {
                if (doc.Id == Guid.Empty)
                {
                    return Json(new { status = false, Message = "There is not such entity in the database!" });
                }
                await _helper.EditDocPostHelper(doc, blogImage);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, Message = "An Error occured!" });
            }
            return Json(new { status = true, Message = "The item was updated!" });
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
