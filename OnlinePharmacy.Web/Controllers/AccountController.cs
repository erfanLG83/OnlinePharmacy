using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlinePharmacy.Domain.Auth;
using OnlinePharmacy.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePharmacy.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SiteSetting _siteSetting;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(SiteSetting siteSetting, SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _siteSetting = siteSetting;
        }
        [Route("account/login")]
        [Route("login")]
        public IActionResult Login(string redirectUrl = null)
        {
            ViewBag.RedirectUrl = redirectUrl;
            return View();
        }
        public IActionResult ExternalLogin(string externalProvider,string redirectUrl=null)
        {
            if (externalProvider is null)
                return NotFound();
            if (externalProvider == "Google")
            {
                string redirectUri = $"https://localhost:44300/signin-google";
                var properties = _signInManager.ConfigureExternalAuthenticationProperties(externalProvider, redirectUri);
                return Challenge(properties, externalProvider);
            }
            return BadRequest();
        }
        [Route("signin-google")]
        public IActionResult SignInGoogle(string redirectUrl = null, string remoteError = null)
        {
            return null;
        }
    }
}
