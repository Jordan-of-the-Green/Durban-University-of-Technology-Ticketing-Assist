// Controllers/AuthController.cs


/***************************************************************************************
 *    Title: <Microsoft Account external login setup with ASP.NET Core>
 *    Author: <Microsoft>
 *    Date Published: <06 March 2022>
 *    Date Retrieved: <22 October 2023>
 *    Code version: <1.0.0>
 *    Availability: <https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/microsoft-logins?view=aspnetcore-7.0>
 *
 ***************************************************************************************/

/***************************************************************************************
 *    Title: <Introduction to Identity on ASP.NET Core>
 *    Author: <Microsoft>
 *    Date Published: <12 January 2022>
 *    Date Retrieved: <22 October 2023>
 *    Code version: <1.0.0>
 *    Availability: <https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-7.0&tabs=visual-studio>
 *
 ***************************************************************************************/

/***************************************************************************************
 *    Title: <Overview of the Microsoft Authentication Library (MSAL)>
 *    Author: <Microsoft>
 *    Date Published: <10 December 2022>
 *    Date Retrieved: <22 October 2023>
 *    Code version: <1.0.0>
 *    Availability: <https://learn.microsoft.com/en-us/azure/active-directory/develop/msal-overview>
 *
 ***************************************************************************************/

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Helpful_Hackers._XBCAD7319._POE.Models;
using Microsoft.AspNetCore.Authorization;

namespace Helpful_Hackers._XBCAD7319._POE.Controllers
{
    [Authorize]
    public class AuthController : Controller
    {
        public IActionResult SignIn()
        {
            // Trigger Microsoft authentication
            return Challenge(new AuthenticationProperties { RedirectUri = Url.Action("UserProfile", "Auth") }, MicrosoftAccountDefaults.AuthenticationScheme);
        }

        public IActionResult SignInAdmin()
        {
            // Trigger Microsoft authentication
            return Challenge(new AuthenticationProperties { RedirectUri = Url.Action("UserProfileAdmin", "Auth") }, MicrosoftAccountDefaults.AuthenticationScheme);
        }

        public IActionResult UserProfile()
        {
            // Retrieve the user's name from the Microsoft authentication data
            var userName = User.Identity.Name; // The user's name is stored in the identity.

            // Create a UserModel with the user's name
            var userModel = new UserModel
            {
                UserName = userName
            };

            return View(userModel);
        }

        public IActionResult UserProfileAdmin()
        {
            // Retrieve the user's name from the Microsoft authentication data
            var userName = User.Identity.Name; // The user's name is stored in the identity.

            // Create a UserModel with the user's name
            var userModel = new UserModel
            {
                UserName = userName
            };

            return View(userModel);
        }


        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait(); // Sign out the user

            return RedirectToAction("Index", "Home");
        }


    }
}
