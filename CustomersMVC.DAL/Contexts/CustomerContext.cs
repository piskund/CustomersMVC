using System.Data.Entity;
using Customers.Core.Entities;

namespace Customers.DAL.Contexts
{
    public class CustomerContext : DbContext
    {
        public CustomerContext() : base(nameof(CustomerContext))
        {
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<CustomerRole> CustomerRoles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}