using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Security;
using Customers.Web.Models;
using WebGrease.Css.Extensions;

namespace Customers.Web.DAL
{
    internal class CustomersInitializer : DropCreateDatabaseIfModelChanges<CustomerContext>
    {
        // DropCreateDatabaseIfModelChanges
        // DropCreateDatabaseAlways
        protected override void Seed(CustomerContext context)
        {
            if (context.Customers.Any())
            {
                // DB has been already seeded.
                return;
            }

            // Fill initial values of customers with fake data.
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
            for (int i = 0; i < 200; i++)
            {
                var randomString = Guid.NewGuid().ToString("N");
                var customer = new Customer()
                {
                    FirstName = randomString.Substring(0, 5),
                    LastName = randomString.Substring(5, 7),
                    Login = randomString.Substring(12, 6),
                    Email = randomString.Substring(0, 12) + "@fake.com"
                };
                customers.Add(customer);
            }

            int phoneNumber = 223322000;
            customers.ForEach(c => c.Password = $"{c.FirstName}{c.LastName}");
            customers.ForEach(c => c.PhoneNumber = phoneNumber++.ToString() );
            customers.ForEach(c => c.IsDisabled = c.Login.Contains("d"));

            // Create customer as application user.
            customers.ForEach(c => Membership.CreateUser(c.Login, c.Password, c.Email));
            // Update password to hash instead of plain value.
            context.Customers.ForEach(c => customers.Find(contextCust => contextCust.Login == c.Login).Password = c.Password);
            // Update customer conext with extended data.
            context.Customers.AddOrUpdate(contextCust => contextCust.Login, customers.ToArray());
            context.SaveChanges();

            // Fill roles table.
            RoleNames.GetAllRoles().ForEach(Roles.CreateRole);

            // Set initial fake roles to customers.
            var allCustomers = customers.Select(c => c.Login).ToArray();
            Roles.AddUsersToRole(allCustomers, RoleNames.Customer);

            var admins = customers.Select(c => c.Login).Where(l => l.Contains("a")).ToArray();
            Roles.AddUsersToRole(admins, RoleNames.Administrator);

            var managers = customers.Select(c => c.Login).Where(l => l.Contains("m")).ToArray();
            Roles.AddUsersToRole(managers, RoleNames.Manager);

            var operators = customers.Select(c => c.Login).Where(l => l.Contains("o")).ToArray();
            Roles.AddUsersToRole(operators, RoleNames.Operator);
        }
    }
}
