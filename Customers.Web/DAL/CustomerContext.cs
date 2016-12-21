using System.Data.Entity;
using System.Linq;
using Customers.Web.Models;

namespace Customers.Web.DAL
{
    public class CustomerContext : DbContext
    {
        public CustomerContext() : base("CustomerConnection")
        {
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<CustomerInRole> CustomersInRoles { get; set; }

        public Customer FindCustomerByLoginOrEmail(string username)
        {
            Customer result = null;

            if (this.Customers != null)
            {
                result = this.Customers.FirstOrDefault(c => (c.Login == username) || (c.Email == username));
            }

            return result;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}