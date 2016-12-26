using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Customers.Web.DAL;
using Customers.Web.Models;
using System.Collections.Generic;
using System.Web.Security;

namespace Customers.Web.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CustomerContext _db = new CustomerContext();

        // GET: Customers
        public async Task<ActionResult> Index(string sortOrder, 
            string currentFilter,
            string searchString,
            int? page)
        {
            if (!RoleNames.GetRolesWithAcccessToSite().Any(r => User.IsInRole(r)))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Account");
            }

            var customersProjection = _db.Customers.Select(c => c);

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

            // Http-request (no sorting/filtering needed)
            if (!Request.IsAjaxRequest())
            {
                return View(await PagedList<Customer>.CreateAsync(customersProjection, 1, pageSize, numOfButtons));
            }

            IEnumerable<Customer> customers = customersProjection.AsEnumerable();
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
                    customers = customers.OrderBy(c => c.FullName);
                    break;
                case "name_desc":
                    customers = customers.OrderByDescending(c => c.FullName);
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

            var model = PagedList<Customer>.Create(customers, page ?? 1, pageSize, numOfButtons);
            model.CurrentFilter = searchString;

            return PartialView("_CustomersListPartial", model);
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
            Customer customer = await _db.Customers.FindAsync(id);
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
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.AllowedToModify)]
        public async Task<ActionResult> Create([Bind(Include = "Id,FirstName,LastName,Email,PhoneNumber,Login,Password,IsDisabled")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _db.Customers.Add(customer);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
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
            Customer customer = await _db.Customers.FindAsync(id);
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
        public async Task<ActionResult> Edit([Bind(Include = "Id,FirstName,LastName,Email,PhoneNumber,Login,Password,IsDisabled")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(customer).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var customers = _db.Customers.Select(c => c);
            var model = PagedList<Customer>.Create(customers, 1, 10, 5);
            model.CurrentFilter = string.Empty;

            return PartialView("_CustomersListPartial", model);
        }

        // GET: Customers/Delete/5
        [Authorize(Roles = RoleNames.AllowedToModify)]
        [AcceptVerbs(HttpVerbs.Delete)]
        public ActionResult Delete(int? id, string sortOrder)
        {
            Customer customer = _db.Customers.Find(id);
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
