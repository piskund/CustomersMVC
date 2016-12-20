using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Customers.Web.DAL;
using Customers.Web.Models;
using System.Collections.Generic;

namespace Customers.Web.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CustomerContext _db = new CustomerContext();

        // GET: Customers
        [Authorize(Roles = RoleNames.AllowedToRead)]
        public ActionResult Index(string sortOrder, 
            string currentFilter,
            string searchString,
            int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["LoginSortParm"] = sortOrder == "login_desc" ? "login" : "login_desc";
            ViewData["NameSortParm"] =  sortOrder == "name_desc" ? "name" : "name_desc";
            ViewData["EmailSortParm"] = sortOrder == "email_desc" ? "email" : "email_desc";
            ViewData["PhoneSortParm"] = sortOrder == "phone_desc" ? "phone" : "phone_desc";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            IEnumerable<Customer> customers = _db.Customers.Select(c => c).AsEnumerable();
            if (!string.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(c => c.FullName.Contains(searchString));
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

                default:
                    break;
            }

            var pageSizeFromConfig = ConfigurationManager.AppSettings["GridPageSize"];
            int pageSize;
            if (!int.TryParse(pageSizeFromConfig, out pageSize))
            {
                // Just give it some default vaue if the config is corrupted.
                pageSize = 5;
            }

            var model = PagedList<Customer>.Create(customers, page ?? 1, pageSize);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_CustomersPartial", model);
            }

            return View(model);
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        [Authorize(Roles = RoleNames.AllowedToModify)]
        public async Task<ActionResult> Delete(int? id)
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

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.AllowedToModify)]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Customer customer = _db.Customers.Find(id);
            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
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
