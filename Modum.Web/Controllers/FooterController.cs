﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Modum.Models.MainModel;
using Modum.Models.ViewModels;
using Modum.Services.Interfaces;
using Modum.Web.ControllerService.FooterController;
using System.Net;
using System.Net.Http.Headers;

namespace Modum.Web.Controllers
{
    public class FooterController : Controller
    {
        #region FieldsAndConstructor
        private readonly IFirebaseService _firebaseService;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IConfiguration _configuration;
        private readonly IFooterControllerHelper _helper;
        private readonly UserManager<ApplicationUser> _userManager;

        public FooterController(IFooterControllerHelper helper, IEmailSenderService emailSenderService,
            IFirebaseService firebaseService, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _firebaseService = firebaseService;
            _configuration = configuration;
            _helper = helper;
            _emailSenderService = emailSenderService;
            _userManager = userManager;
        }
        #endregion

        #region Views

        public async Task<IActionResult> AboutUs()
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);
            var userId = user != null ? user.Id : "";

            var viewModel = new CartElementsCountViewModel();
            viewModel.CartItemsForUser = await _helper.GetAmountOfCartItemsForUser(userId);

            return View("~/Views/FooterItems/AboutUs.cshtml", viewModel);
        }

        public async Task<IActionResult> Ads()
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);
            var userId = user != null ? user.Id : "";

            var viewModel = new CartElementsCountViewModel();
            viewModel.CartItemsForUser = await _helper.GetAmountOfCartItemsForUser(userId);

            return View("~/Views/FooterItems/Ads.cshtml", viewModel);
        }

        public async Task<IActionResult> Campaigns()
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);
            var userId = user != null ? user.Id : "";

            var viewModel = new CampaignsPageViewModel();
            viewModel.CartItemsForUser = await _helper.GetAmountOfCartItemsForUser(userId);
            viewModel.LTCs = await _helper.GetCampaignInformationData();

            return View("~/Views/FooterItems/Campaigns.cshtml", viewModel);
        }

        public async Task<IActionResult> ContactUs()
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);
            var userId = user != null ? user.Id : "";

            var viewModel = new CartElementsCountViewModel();
            viewModel.CartItemsForUser = await _helper.GetAmountOfCartItemsForUser(userId);

            return View("~/Views/FooterItems/ContactUs.cshtml", viewModel);
        }

        public async Task<IActionResult> MostAskedQuestions()
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);
            var userId = user != null ? user.Id : "";

            var viewModel = new CartElementsCountViewModel();
            viewModel.CartItemsForUser = await _helper.GetAmountOfCartItemsForUser(userId);

            return View("~/Views/FooterItems/MostAskedQuestions.cshtml", viewModel);
        }

        public async Task<IActionResult> OnlinePayment()
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);
            var userId = user != null ? user.Id : "";

            var viewModel = new CartElementsCountViewModel();
            viewModel.CartItemsForUser = await _helper.GetAmountOfCartItemsForUser(userId);

            return View("~/Views/FooterItems/OnlinePayment.cshtml", viewModel);
        }

        public async Task<IActionResult> Partnership()
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);
            var userId = user != null ? user.Id : "";

            var viewModel = new CartElementsCountViewModel();
            viewModel.CartItemsForUser = await _helper.GetAmountOfCartItemsForUser(userId);

            return View("~/Views/FooterItems/Partnership.cshtml", viewModel);
        }

        public async Task<IActionResult> PaymentOnDelivery()
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);
            var userId = user != null ? user.Id : "";

            var viewModel = new CartElementsCountViewModel();
            viewModel.CartItemsForUser = await _helper.GetAmountOfCartItemsForUser(userId);

            return View("~/Views/FooterItems/PaymentOnDelivery.cshtml", viewModel);
        }

        public async Task<IActionResult> Privacy()
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);
            var userId = user != null ? user.Id : "";

            var viewModel = new CartElementsCountViewModel();
            viewModel.CartItemsForUser = await _helper.GetAmountOfCartItemsForUser(userId);

            return View("~/Views/FooterItems/Privacy.cshtml", viewModel);
        }

        public async Task<IActionResult> Returning()
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);
            var userId = user != null ? user.Id : "";

            var viewModel = new CartElementsCountViewModel();
            viewModel.CartItemsForUser = await _helper.GetAmountOfCartItemsForUser(userId);

            return View("~/Views/FooterItems/Returning.cshtml", viewModel);
        }

        public async Task<IActionResult> TermsAndConditions()
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);
            var userId = user != null ? user.Id : "";

            var viewModel = new CartElementsCountViewModel();
            viewModel.CartItemsForUser = await _helper.GetAmountOfCartItemsForUser(userId);

            return View("~/Views/FooterItems/TermsAndConditions.cshtml",viewModel);
        }


        #endregion

        #region Blog
        public async Task<IActionResult> Blog()
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);
            var userId = user != null ? user.Id : "";

            var viewModel = new BlogViewModel();
            viewModel.CartItemsForUser = await _helper.GetAmountOfCartItemsForUser(userId);
            var model = _firebaseService.GetAllBlogPosts();
            viewModel.BlogPosts = model;
            return View("~/Views/FooterItems/Blog.cshtml", viewModel);
        }

        public async Task<IActionResult> BlogPost(Guid postId)
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);
            var userId = user != null ? user.Id : "";

            var viewModel = new BlogPostViewModel();
            viewModel.CartItemsForUser = await _helper.GetAmountOfCartItemsForUser(userId);
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

        #region OutfitPicker

        [Authorize]
        public async Task<IActionResult> OutfitPicker()
        {
            var username = HttpContext.User?.Identity?.Name ?? "";
            var user = await _userManager.FindByNameAsync(username);
            var userId = user != null ? user.Id : "";

            var model = new OutfitPickerViewModel();
            model.CartItemsForUser = await _helper.GetAmountOfCartItemsForUser(userId);
            return View("~/Views/UserViews/OutfitPicker.cshtml", model);

        }

        [Authorize]
        public async Task<JsonResult> OutfitPickerOnPostQuery(IFormFile personImage, IFormFile clothImage)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://virtual-try-on2.p.rapidapi.com/clothes-virtual-tryon");

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    Content = new MultipartFormDataContent
                    {
                        // Add person image
                        new StreamContent(personImage.OpenReadStream())
                        {
                            Headers =
                            {
                                ContentType = new MediaTypeHeaderValue(personImage.ContentType),
                                ContentDisposition = new ContentDispositionHeaderValue("form-data")
                                {
                                    Name = "personImage",
                                    FileName = personImage.FileName
                                }
                            }
                        },
                        new StreamContent(clothImage.OpenReadStream())
                        {
                            Headers =
                            {
                                ContentType = new MediaTypeHeaderValue(clothImage.ContentType),
                                ContentDisposition = new ContentDispositionHeaderValue("form-data")
                                {
                                    Name = "clothImage",
                                    FileName = clothImage.FileName
                                }
                            }
                        }
                    }
                };

                request.Headers.Add("X-RapidAPI-Key", _configuration.GetSection("ApiKey:ApiKeyValue").Get<string>() ?? "");
                request.Headers.Add("X-RapidAPI-Host", _configuration.GetSection("ApiKey:ApiKeyPassword").Get<string>() ?? "");

                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var responseContent = await response.Content.ReadAsStringAsync();

                    return Json(new { status = true, Message = responseContent });
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
