using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Customers.Web.DAL;
using Customers.Web.Models;
using System.Web.Security;
using Customers.Web.Providers;

namespace Customers.Web.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CustomerContext _db = new CustomerContext();

        // GET: Customers
        public ActionResult Index(string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            if (!RoleNames.GetRoleNamesWithAcccessToSite().Any(r => User.IsInRole(r)))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Account");
            }

            var customers = _db.Customers.Select(c => c).AsEnumerable();

            // Parse page size parameter from config.
            var pageSizeFromConfig = ConfigurationManager.AppSettings["PagedListPageSize"];
            int pageSize;
            if (!int.TryParse(pageSizeFromConfig, out pageSize))
            {
                // Just give it some default vaue if the config is corrupted.
                pageSize = 10;
            }
            var numOfBtnsFromConfig = ConfigurationManager.AppSettings["NumberOfButtons"];
            int numOfButtons;
            if (!int.TryParse(numOfBtnsFromConfig, out numOfButtons))
            {
                // Just give it some default vaue if the config is corrupted.
                numOfButtons = 5;
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["LoginSortParm"] = sortOrder == "login" ? "login_desc" : "login";
            ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["EmailSortParm"] = sortOrder == "email" ? "email_desc" : "email";
            ViewData["PhoneSortParm"] = sortOrder == "phone" ? "phone_desc" : "phone";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            // Ajax-request 
            if (Request.IsAjaxRequest())
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    customers = customers.Where(c => c.FirstName.Contains(searchString) ||
                                                     c.LastName.Contains(searchString) ||
                                                     c.Login.Contains(searchString) ||
                                                     c.Email.Contains(searchString) ||
                                                     c.PhoneNumber.Contains(searchString) ||
                                                     Roles.GetRolesForUser(c.Login).Any(r => r.Contains(searchString)));
                }

                switch (sortOrder)
                {
                    case "name":
                        customers = customers.OrderBy(c => c.FirstName + c.LastName);
                        break;
                    case "name_desc":
                        customers = customers.OrderByDescending(c => c.FirstName + c.LastName);
                        break;
                    case "login":
                        customers = customers.OrderBy(c => c.Login);
                        break;
                    case "login_desc":
                        customers = customers.OrderByDescending(c => c.Login);
                        break;
                    case "email":
                        customers = customers.OrderBy(c => c.Email);
                        break;
                    case "email_desc":
                        customers = customers.OrderByDescending(c => c.Email);
                        break;
                    case "phone":
                        customers = customers.OrderBy(c => c.PhoneNumber);
                        break;
                    case "phone_desc":
                        customers = customers.OrderByDescending(c => c.PhoneNumber);
                        break;
                }
            }

            var model = PagedList<CustomerViewModel>.Create(customers.Select(c => (CustomerViewModel)c), page ?? 1, pageSize, numOfButtons);
            model.CurrentFilter = searchString;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_CustomersListPartial", model);
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Stats(int? currentStats)
        {
            return PartialView("_StatsPartial", currentStats ?? 0);
        }

        // GET: Customers/Details/5
        [Authorize(Roles = RoleNames.AllowedToRead)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = (CustomerViewModel)await _db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        [Authorize(Roles = RoleNames.AllowedToModify)]
        public ActionResult Create()
        {
            return View(new CustomerCreateUpdateViewModel());
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.AllowedToModify)]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email,PhoneNumber,Login,Password,ConfirmPassword,IsDisabled")] CustomerCreateUpdateViewModel customer,
             int[] roleId)
        {
            if (roleId == null || roleId.Length < 1)
            {
                ModelState.AddModelError("RolesList","At least one role must be selected.");
            }
            if (ModelState.IsValid)
            {
                MembershipCreateStatus status;
                // Create customer as application user.
                ((CustomMembershipProvider)Membership.Provider).CreateUser((CustomerEntity)customer, out status);

                if (status != MembershipCreateStatus.Success)
                {
                    ModelState.AddModelError(string.Empty, status.ToString());
                }
                else
                {
                    // Creating roles
                    var selectedRoleNames = roleId.Select(id => _db.Roles.Find(id).Name).ToArray();
                    Roles.AddUserToRoles(customer.Login, selectedRoleNames);

                    return RedirectToAction("Index");
                }
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        [Authorize(Roles = RoleNames.AllowedToModify)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = (CustomerCreateUpdateViewModel)await _db.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.AllowedToModify)]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FirstName,LastName,Email,PhoneNumber,Login,Password,ConfirmPassword,OldHashedPassword,IsActive,InititallySelectedRoles")] CustomerCreateUpdateViewModel customer, 
           int[] roleId)
        {
            if (ModelState.IsValid)
            {
                var customerEntity = (CustomerEntity)customer;
                _db.Entry(customerEntity).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                // Updating roles
                var selectedRoleNames = roleId.Select(id => _db.Roles.Find(id).Name);

                var notSelectedNamesInRole = RoleNames.GetAllRoleNames().Except(selectedRoleNames).Where(roleName => Roles.IsUserInRole(customer.Login, roleName)).ToArray();
                if (notSelectedNamesInRole.Length > 0)
                {
                    Roles.RemoveUserFromRoles(customer.Login, notSelectedNamesInRole);
                }

                var selectedNamesNotInRole = selectedRoleNames.Where(roleName => !Roles.IsUserInRole(customer.Login, roleName)).ToArray();
                if (selectedNamesNotInRole.Length > 0)
                {
                    Roles.AddUserToRoles(customer.Login, selectedNamesNotInRole);
                }

                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Delete/5
        [Authorize(Roles = RoleNames.AllowedToModify)]
        [AcceptVerbs(HttpVerbs.Delete)]
        public ActionResult Delete(int? id, string sortOrder)
        {
            var customer = _db.Customers.Find(id);
            _db.Customers.Remove(customer);
            _db.SaveChanges();

            return RedirectToAction("Index", new {sortOrder});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
