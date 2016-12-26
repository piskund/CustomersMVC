using System.Web.Mvc;
using Customers.Web.Models;

namespace Customers.Web.Controllers
{
    public class CustomerRolesController : Controller
    {
        // GET: CustomerRoles
        [Authorize(Roles = RoleNames.AllowedToRead)]
        public ActionResult Index()
        {
            return View();
        }
    }
}