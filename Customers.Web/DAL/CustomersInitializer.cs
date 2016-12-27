using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Security;
using Customers.Web.Models;
using WebGrease.Css.Extensions;

namespace Customers.Web.DAL
{
    internal class CustomersInitializer : DropCreateDatabaseAlways<CustomerContext>
    {
        protected override void Seed(CustomerContext context)
        {
            context.Configuration.ValidateOnSaveEnabled = false;

            // Fill initial values of customers with fake data.
            var customers = new List<CustomerEntity>
            {
                new CustomerEntity {FirstName = "John", LastName = "Smith", Email = "john.smith@fake.com", Login = "jsmi"},
                new CustomerEntity {FirstName = "Jane", LastName = "Wesson", Email = "jane.wesson@fake.com", Login = "jwes"},
                new CustomerEntity {FirstName = "John", LastName = "Doe", Email = "john.doe@fake.com", Login = "jdoe"},
                new CustomerEntity {FirstName = "Jane", LastName = "Doe", Email = "jane.doe@fake.com", Login = "jado"},
                new CustomerEntity {FirstName = "Jack", LastName = "Black", Email = "jack.black@fake.com", Login = "jbla"},
                new CustomerEntity {FirstName = "Peggy", LastName = "Pink", Email = "peggy.black@fake.com", Login = "ppin"},
                new CustomerEntity {FirstName = "Laura", LastName = "White", Email = "laura.white@fake.com", Login = "lwhi"},
                new CustomerEntity {FirstName = "Dorian", LastName = "Grey", Email = "dorian.grey@fake.com", Login = "dgre"},
                new CustomerEntity {FirstName = "Yan", LastName = "Brown", Email = "yan.brown@fake.com", Login = "yabro"},
                new CustomerEntity {FirstName = "Ned", LastName = "Red", Email = "ned.red@fake.com", Login = "nerd"}
            };
            customers.ForEach(c => c.Password = $"{c.FirstName}{c.LastName}");
            try
            {
                for (int i = 0; i < 200; i++)
                {
                    var randomString = Guid.NewGuid().ToString("N");
                    var customer = new CustomerEntity()
                    {
                        FirstName = randomString.Substring(0, 10),
                        LastName = randomString.Substring(10, 15),
                        Login = randomString.Substring(7, 12),
                        Email = randomString.Substring(5, 15) + "@fake.com",
                        Password = randomString.Substring(0,8) 
                    };
                    customers.Add(customer);
                }

                int phoneNumber = 223322000;
                customers.ForEach(c => c.Password = c.Password + "_1");
                customers.ForEach(c => c.PhoneNumber = phoneNumber++.ToString());
                customers.ForEach(c => c.IsDisabled = c.Login.Contains("d"));

                // Create customer as application user.
                customers.ForEach(c => Membership.CreateUser(c.Login, c.Password, c.Email));
                // Update password to hash instead of plain value.
                context.Customers.ForEach(
                    c => customers.Find(contextCust => contextCust.Login == c.Login).Password = c.Password);
                // Update customer conext with extended data.
                context.Customers.AddOrUpdate(contextCust => contextCust.Login, customers.ToArray());
                context.SaveChanges();
            }
            catch (DbEntityValidationException ve)
            {
                Exception raise = ve;
                foreach (var validationErrors in ve.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        var message = $"{validationErrors.Entry.Entity}:{validationError.ErrorMessage}";
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

            // Fill roles table.
            RoleNames.GetAllRoles().ForEach(Roles.CreateRole);

            // Set initial fake roles to customers.
            var allCustomers = customers.Select(c => c.Login).ToArray();
            Roles.AddUsersToRole(allCustomers, RoleNames.Customer);

            var admins = customers.Select(c => c.Login).Where(l => l.Contains("s")).ToArray();
            Roles.AddUsersToRole(admins, RoleNames.Administrator);

            var managers = customers.Select(c => c.Login).Where(l => l.Contains("m")).ToArray();
            Roles.AddUsersToRole(managers, RoleNames.Manager);

            var operators = customers.Select(c => c.Login).Where(l => l.Contains("o")).ToArray();
            Roles.AddUsersToRole(operators, RoleNames.Operator);

            context.Configuration.ValidateOnSaveEnabled = true;
        }
    }
}
