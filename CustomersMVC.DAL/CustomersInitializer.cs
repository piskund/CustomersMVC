using System.Collections.Generic;
using System.Data.Entity;
using Customers.Core.Entities;
using Customers.DAL.Contexts;

namespace Customers.DAL
{
    internal class CustomersInitializer : DropCreateDatabaseIfModelChanges<CustomerContext>
    {
        protected override void Seed(CustomerContext context)
        {
            var customerEntities = new List<Customer>
            {
                new Customer {FirstName = "John", LastName = "Smith"},
                new Customer {FirstName = "Jane", LastName = "Wesson"},
                new Customer {FirstName = "John", LastName = "Doe"},
                new Customer {FirstName = "Jane", LastName = "Doe"},
                new Customer {FirstName = "Jack", LastName = "Black"},
                new Customer {FirstName = "Peggy", LastName = "Pink"},
                new Customer {FirstName = "Laura", LastName = "White"},
                new Customer {FirstName = "Dorian", LastName = "Grey"},
                new Customer {FirstName = "Yan", LastName = "Brown"},
                new Customer {FirstName = "Ned", LastName = "Red"}
            };
            customerEntities.ForEach(s => context.Customers.Add(s));
            context.SaveChanges();

            var roles = new List<Role>
            {
                new Role {Name = "Administrator"},
                new Role {Name = "Cutomer"},
                new Role {Name = "Operator"},
                new Role {Name = "Manager"}
            };
            roles.ForEach(s => context.Roles.Add(s));
            context.SaveChanges();

            var customerRoles = new List<CustomerRole>
            {
                new CustomerRole {CustomerID = 1, RoleID = 1},
                new CustomerRole {CustomerID = 1, RoleID = 2},
                new CustomerRole {CustomerID = 2, RoleID = 1},
                new CustomerRole {CustomerID = 3, RoleID = 3},
                new CustomerRole {CustomerID = 4, RoleID = 4},
                new CustomerRole {CustomerID = 5, RoleID = 2},
                new CustomerRole {CustomerID = 5, RoleID = 4},
                new CustomerRole {CustomerID = 6, RoleID = 2},
                new CustomerRole {CustomerID = 7, RoleID = 1},
                new CustomerRole {CustomerID = 8, RoleID = 3},
                new CustomerRole {CustomerID = 9, RoleID = 2},
                new CustomerRole {CustomerID = 10, RoleID = 4},
            };
            customerRoles.ForEach(s => context.CustomerRoles.Add(s));
            context.SaveChanges();
        }
    }
}
