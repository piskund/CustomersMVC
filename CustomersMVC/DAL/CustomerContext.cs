using System.Data.Entity;
using CustomersMVC.Models;

namespace CustomersMVC.DAL
{
    public class CustomerContext : DbContext
    {
        public CustomerContext() : base(nameof(CustomerContext))
        {
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Role> Roles { get; set; }

        //public DbSet<CustomerRole> CustomerRoles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}