using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    namespace AspNetCoreTodo.Controllers
    {
        [Authorize]
        [Route("[controller]/[action]")]
        public class AccountController : Controller
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly ILogger _logger;

            public AccountController(UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                ILogger<AccountController> logger)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _logger = logger;
            }

            [TempData]
            public string ErrorMessage { get; set; }

            [HttpGet]
            [AllowAnonymous]
            public async Task<IActionResult> Login(string returnUrl = null)
            {
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Logout()
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out.");
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public IActionResult ExternalLogin(string provider, string returnUrl = null)
            {
                // Request a redirect to the external login provider.
                var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
                var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
                return Challenge(properties, provider);
            }

            [HttpGet]
            [AllowAnonymous]
            public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
            {
                if (remoteError != null)
                {
                    ErrorMessage = $"Error from external provider: {remoteError}";
                    return RedirectToAction(nameof(Login));
                }

                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null) return RedirectToAction(nameof(Login));

                // Sign in the user with this external login provider if the user already has a login.
                var result =
                    await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                    return RedirectToLocal(returnUrl);
                }

                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }

            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model,
                string returnUrl = null)
            {
                if (ModelState.IsValid)
                {
                    // Get the information about the user from the external login provider
                    var info = await _signInManager.GetExternalLoginInfoAsync();
                    if (info == null)
                        throw new ApplicationException("Error loading external login information during confirmation.");
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var result = await _userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddLoginAsync(user, info);
                        if (result.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, false);
                            _logger.LogInformation("User created an account using {Name} provider.",
                                info.LoginProvider);
                            return RedirectToLocal(returnUrl);
                        }
                    }

                    AddErrors(result);
                }

                ViewData["ReturnUrl"] = returnUrl;
                return View(nameof(ExternalLogin), model);
            }

            #region Helpers

            private void AddErrors(IdentityResult result)
            {
                foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            }

            private IActionResult RedirectToLocal(string returnUrl)
            {
                if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            #endregion
        }
    }
}