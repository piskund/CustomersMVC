using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Customers.Web.DAL;
using Customers.Web.Models;

namespace Customers.Web.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CustomerContext _db = new CustomerContext();

        // GET: Customers
        [Authorize(Roles = RoleNames.AllowedToRead)]
        public async Task<ActionResult> Index(string sortOrder, 
            string currentFilter,
            string searchString,
            int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["LastNameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "lastname_desc" : string.Empty;
            ViewData["FirstNameSortParm"] = sortOrder == "firstname_desc" ? "firstname" : "firstname_desc";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var customers = _db.Customers.Select(c => c);

            if (!string.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(c => c.LastName.Contains(searchString)
                                       || c.FirstName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "lastname":
                    customers = customers.OrderBy(s => s.LastName);
                    break;
                case "lastname_desc":
                    customers = customers.OrderByDescending(s => s.LastName);
                    break;
                case "firstname":
                    customers = customers.OrderBy(s => s.FirstName);
                    break;
                case "firstname_desc":
                    customers = customers.OrderByDescending(s => s.FirstName);
                    break;
                default:
                    customers = customers.OrderBy(s => s.LastName);
                    break;
            }

            var pageSizeFromConfig = ConfigurationManager.AppSettings["GridPageSize"];
            int pageSize;
            if (!int.TryParse(pageSizeFromConfig, out pageSize))
            {
                // Just give it some default vaue if the config is corrupted.
                pageSize = 5;
            }
            return View(await PagedList<Customer>.CreateAsync(customers.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Customers/Details/5
        [Authorize(Roles = RoleNames.AllowedToRead)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = _db.Customers.Find(id);
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
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email,PhoneNumber,Login,Password,IsDisabled")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _db.Customers.Add(customer);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        [Authorize(Roles = RoleNames.AllowedToModify)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = _db.Customers.Find(id);
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
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,PhoneNumber,Login,Password,IsDisabled")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(customer).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        [Authorize(Roles = RoleNames.AllowedToModify)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = _db.Customers.Find(id);
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
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = _db.Customers.Find(id);
            _db.Customers.Remove(customer);
            _db.SaveChanges();
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
