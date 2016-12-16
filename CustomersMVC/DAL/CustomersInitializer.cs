using System.Collections.Generic;
using System.Data.Entity;
using CustomersMVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CustomersMVC.DAL
{
    internal class CustomersInitializer : DropCreateDatabaseAlways<CustomerContext>
    {
        protected override async void Seed(CustomerContext context)
        {
            var customers = new List<Customer>
            {
                new Customer {FirstName = "John", LastName = "Smith", Email = "john.smith@fake.com", Login = "jsm"},
                new Customer {FirstName = "Jane", LastName = "Wesson", Email = "jane.wesson@fake.com", Login = "jws"},
                new Customer {FirstName = "John", LastName = "Doe", Email = "john.doe@fake.com", Login = "jdo"},
                new Customer {FirstName = "Jane", LastName = "Doe", Email = "jane.doe@fake.com", Login = "jado"},
                new Customer {FirstName = "Jack", LastName = "Black", Email = "jack.black@fake.com", Login = "jbk"},
                new Customer {FirstName = "Peggy", LastName = "Pink", Email = "peggy.black@fake.com", Login = "ppk"},
                new Customer {FirstName = "Laura", LastName = "White", Email = "laura.white@fake.com", Login = "lwt"},
                new Customer {FirstName = "Dorian", LastName = "Grey", Email = "dorian.grey@fake.com", Login = "dgr"},
                new Customer {FirstName = "Yan", LastName = "Brown", Email = "yan.brown@fake.com", Login = "yabro"},
                new Customer {FirstName = "Ned", LastName = "Red", Email = "ned.red@fake.com", Login = "nerd"}
            };

            var roles = new List<Role>
            {
                new Role {Name = "Administrator"},
                new Role {Name = "Customer"},
                new Role {Name = "Operator"},
                new Role {Name = "Manager"}
            };
            context.Roles.AddRange(roles);
            await context.SaveChangesAsync();
            
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            foreach (var role in roles)
            {
                var identityRole = new IdentityRole { Name = role.Name };
                roleManager.Create(identityRole);
            }
            await context.SaveChangesAsync();

            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            foreach (var customer in customers)
            {
                var appUser = new ApplicationUser() {Login = customer.Login};
                var result = userManager.Create(appUser, "12345");
                if (result.Succeeded)
                {
                    customer.Password = appUser.PasswordHash;
                    userManager.AddToRole(appUser.Id, "Customer");
                }
            }
            await context.SaveChangesAsync();

            customers.ForEach(s => context.Customers.Add(s));
            await context.SaveChangesAsync();
        }
    }
}
