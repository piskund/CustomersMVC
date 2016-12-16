using System.Collections.Generic;
using System.Data.Entity;
using CustomersMVC.Models;

namespace CustomersMVC.DAL
{
    internal class CustomersInitializer : DropCreateDatabaseIfModelChanges<CustomerContext>
        //DropCreateDatabaseAlways
    {
        protected override void Seed(CustomerContext context)
        {
            var customerEntities = new List<Customer>
            {
                new Customer {FirstName = "John", LastName = "Smith", Email = "john.smith@fake.com"},
                new Customer {FirstName = "Jane", LastName = "Wesson", Email = "jane.wesson@fake.com"},
                new Customer {FirstName = "John", LastName = "Doe", Email = "john.doe@fake.com"},
                new Customer {FirstName = "Jane", LastName = "Doe", Email = "jane.doe@fake.com"},
                new Customer {FirstName = "Jack", LastName = "Black", Email = "jack.black@fake.com"},
                new Customer {FirstName = "Peggy", LastName = "Pink", Email = "peggy.black@fake.com"},
                new Customer {FirstName = "Laura", LastName = "White", Email = "laura.white@fake.com"},
                new Customer {FirstName = "Dorian", LastName = "Grey", Email = "dorian.grey@fake.com"},
                new Customer {FirstName = "Yan", LastName = "Brown", Email = "yan.brown@fake.com"},
                new Customer {FirstName = "Ned", LastName = "Red", Email = "ned.red@fake.com"}
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
