using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WebApplication1.Interfaces;
using Microsoft.WebApplication1.Identity;
using Microsoft.WebApplication1.Extentions;
using Microsoft.WebApplication1.Services;
using Microsoft.WebApplication1.ViewModels.Manage;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.WebApplication1.ViewModels;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Microsoft.WebApplication1.Controllers
{

    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize] // Controllers that require Authorization still use Controller/View; other pages use Pages
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly Interfaces.IEmailSender _emailSender;
        private readonly IAppLogger<ManageController> _logger;
        private readonly UrlEncoder _urlEncoder;

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        public ManageController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            Interfaces.IEmailSender emailSender,
            IAppLogger<ManageController> logger,
            UrlEncoder urlEncoder)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._emailSender = emailSender;
            this._logger = logger;
            this._urlEncoder = urlEncoder;
        }
        [TempData] /* stores statusMessage in tempData dictionary */
        public string StatusMessage { get; set; }
       
        [HttpGet]
        public async Task<IActionResult> MyAccount()
        {
            var user = await _userManager.GetUserAsync(User); //user object contains db user data corresponding to authentication principal data
            if(user == null)
            {
                throw new ApplicationException($"Unable to load user with ID `{_userManager.GetUserId(User)}`.");

            }
            var model = new IndexViewModel /* transfer user attributes to IndexViewModel to display in view*/
            {
                /* user is inherited from IdentityUser class(from entityFramework lib)*/
                Username = user.UserName,
                IsEmailConfirmed = user.EmailConfirmed,
                Email = user.Email,
                phoneNumber = user.PhoneNumber,
                statusMessage = StatusMessage
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        { /* this is the Index POST handler method, redirect to Index GET */
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                throw new ApplicationException($"Unable to load user with ID `{_userManager.GetUserId(User)}`");
            }
            var email = user.Email; // var represents UTF-16 encoded text
            if(model.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                //set identityUser's email to indexViewModel user's email
                if(!setEmailResult.Succeeded)
                {
                    throw new ApplicationException($"Unable to set email for user with ID `{user.Id}`");
                }
            }
            var phoneNumber = user.PhoneNumber;
            if(model.phoneNumber != phoneNumber) /* if indexviewmodel's phone num != identityUser's phone num => set identityUser's phone num to indexviewmodel's phone num*/
            {
                var setPhoneNumber = await _userManager.SetPhoneNumberAsync(user, model.phoneNumber);
                if(!setPhoneNumber.Succeeded)
                {
                    throw new ApplicationException($"Unable to set phone number for user with ID `{user.Id}`");
                }
            }
            StatusMessage = "Your profile has been updated";
            return RedirectToAction(nameof(Index)); /* nameof returns the variable, type, or member of an object in string format*/
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendVerificationEmail(IndexViewModel model)
        {
            /* post method*/
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            /* User is the HTTP request body sent from the view */
            var user = await _userManager.GetUserAsync(User); /* user == IdentityUser*/
            if(user == null)
            {
                throw new ApplicationException($"Unable to load user with id `{_userManager.GetUserId(User)}`");
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme); // this var contains the email confimation link to be included in the confirmation email 
            var email = user.Email;
            
            await _emailSender.SendEmailConfirmationAsync(email, callbackUrl);
            //IEmailSender.SendEmailConfirmationAsync() is a extention method defined in Extentions/EmailConfirmationExtentions.cs
            StatusMessage = "An email verification token has been sent to your email. Please verify it.";
            return RedirectToAction(nameof(Index));// redirect to HTTP GET /
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async void LinkLogin(string provider)
        {
            /* implement temp void method, uncomment after implements external login features*/
           /* await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);*/

        /*    var redirectUrl = Url.Action(nameof(LinkLoginCallback));
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return new ChallengeResult(provider, properties);*/
        }

        [HttpGet]
        public async void LinkLoginCallback()
        {
            /* change return type to Task<IActionResult> after method definition*/
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async void RemoveLogin(RemoveLoginViewModel model)
        {
            /* change return type to Task<IActionResult> after method definition*/
        }

        [HttpGet]
        public async Task<IActionResult> EnableAuthenticator()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                throw new ApplicationException($"Unable to load user with ID {_userManager.GetUserId(User)}");
            }
            //contains authenticator key for authenticated user
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);

            if (string.IsNullOrEmpty(unformattedKey))
            {
                // if authenticator key null reset key then try to get key again
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            var model = new EnableAuthenticatorViewModel // this is inistantiates empty object then uses setters to assign values to fields
            {
                SharedKey = FormatKey(unformattedKey),
                AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey)
            };
            return View(model);
        }

  /*      [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with Id `{_userManager.GetUserId(User)}`");
            }
            var hasPassword = await _userManager.HasPasswordAsync(user);
            if(!hasPassword)
            {
                return RedirectToAction(nameof(Index));
            }
            var model = new 

        }*/

        private void AddErrors(IdentityResult result)
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public string FormatKey(string unformattedKey)
        {
            // adds space every 4 characters and converts all chars to lowercase before return new key
            var result = new StringBuilder();
            int currentPosition = 0;
            while(currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
            }
            if(currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }
            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat, 
                _urlEncoder.Encode("WebApplication1"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }
    }
}
