using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Customers.Web.Models;

namespace Customers.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl, string errorMessage)
        {
            if (!RoleNames.GetRoleNamesWithAcccessToSite().Any(r =>  Roles.GetRolesForUser(model.Login).Contains(r) ))
            {
                ModelState.AddModelError("", "Your current role(s) has no access to this site.");
                FormsAuthentication.SignOut();
                return View(model);
            }

            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.Login, model.Password))
                {
                    var membershipUser = Membership.GetUser(model.Login);
                    
                    if (membershipUser != null && membershipUser.IsApproved)
                    {
                        FormsAuthentication.SetAuthCookie(model.Login, true);

                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Customers");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The user is disabled. Please contact to your system administrator.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Customers");
        }
    }
}