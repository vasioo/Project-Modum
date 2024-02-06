using Microsoft.AspNetCore.Mvc;
using Modum.Models.ViewModels;
using Modum.Services.Interfaces;
using Modum.Services.Services;
using Modum.Web.ControllerService.FooterController;
using System.Net;

namespace Modum.Web.Controllers
{
    public class FooterController : Controller
    {
        #region FieldsAndConstructor
        private readonly IFirebaseService _firebaseService;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IConfiguration _configuration;
        private readonly IFooterControllerHelper _helper;


        public FooterController(IFooterControllerHelper helper, IEmailSenderService emailSenderService,
            IFirebaseService firebaseService, IConfiguration configuration
            )
        {
            _firebaseService = firebaseService;
            _configuration = configuration;
            _helper = helper;
            _emailSenderService = emailSenderService;
        }
        #endregion

        #region Views
        public async Task<IActionResult> AboutUs()
        {
            return View("~/Views/FooterItems/AboutUs.cshtml");
        }
        public async Task<IActionResult> Ads()
        {
            return View("~/Views/FooterItems/Ads.cshtml");
        }
        public async Task<IActionResult> Campaigns()
        {
            var viewModel = await _helper.GetCampaignInformationData();
            return View("~/Views/FooterItems/Campaigns.cshtml",viewModel);
        }
        public async Task<IActionResult> ContactUs()
        {
            return View("~/Views/FooterItems/ContactUs.cshtml");
        }
        public async Task<IActionResult> MostAskedQuestions()
        {
            return View("~/Views/FooterItems/MostAskedQuestions.cshtml");
        }
        public async Task<IActionResult> OnlinePayment()
        {
            return View("~/Views/FooterItems/OnlinePayment.cshtml");
        }
        public async Task<IActionResult> Partnership()
        {
            return View("~/Views/FooterItems/Partnership.cshtml");
        }
        public async Task<IActionResult> PaymentOnDelivery()
        {
            return View("~/Views/FooterItems/PaymentOnDelivery.cshtml");
        }
        public async Task<IActionResult> Privacy()
        {
            return View("~/Views/FooterItems/Privacy.cshtml");
        }
        public async Task<IActionResult> Returning()
        {
            return View("~/Views/FooterItems/Returning.cshtml");
        }
        public async Task<IActionResult> TermsAndConditions()
        {
            return View("~/Views/FooterItems/TermsAndConditions.cshtml");
        }

        public async Task<IActionResult> OutfitPicker()
        {
            var model = new OutfitPickerViewModel();
            return View("~/Views/UserViews/OutfitPicker.cshtml", model);

        }
        #endregion

        #region Blog
        public IActionResult Blog()
        {
            var viewModel = new BlogViewModel();
            var model = _firebaseService.GetAllBlogPosts();
            viewModel.BlogPosts = model;
            return View("~/Views/FooterItems/Blog.cshtml", viewModel);
        }

        public async Task<IActionResult> BlogPost(Guid postId)
        {
            var viewModel = new BlogPostViewModel();
            viewModel.Post = await _firebaseService.GetBlogPostById(postId);
            if (viewModel.Post != null)
            {
                return View("~/Views/FooterItems/BlogPost.cshtml", viewModel);
            }
            return View("~/Views/FooterItems/Blog.cshtml");
        }

        #endregion

        #region Emails
        [HttpPost]
        public async Task<JsonResult> UserSendEmail(string email, string bodyText, string name)
        {
            try
            {
                string subject = "user-message";
                var status = _emailSenderService.ReceiveEmail(email, bodyText, subject, name);

                if (status == HttpStatusCode.OK)
                {
                    return Json(new { status = true, Message = "The email was sent successfully!" });
                }
                else
                {
                    return Json(new { status = false, Message = "Error: Email could not be sent." });
                }
            }
            catch (Exception)
            {
                return Json(new { status = false, Message = "Error Conflicted" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> PartnershipSendEmail(string email, string bodyText, string name)
        {
            try
            {
                string subject = "partnerships";
                var status = _emailSenderService.ReceiveEmail(email, bodyText, subject, name);

                if (status == HttpStatusCode.OK)
                {
                    return Json(new { status = true, Message = "The email was sent successfully!" });
                }
                else
                {
                    return Json(new { status = false, Message = "Error: Email could not be sent." });
                }
            }
            catch (Exception)
            {
                return Json(new { status = false, Message = "Error Conflicted" });
            }
        }
        #endregion
    }
}
