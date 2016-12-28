using System;
using System.Configuration.Provider;
using System.Linq;
using System.Web.Security;
using Customers.Web.DAL;
using Customers.Web.Models;

namespace Customers.Web.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        public override bool IsUserInRole(string username, string roleName)
        {
            return GetRolesForUser(username).Any(r => r == roleName);
        }

        public override string[] GetRolesForUser(string username)
        {
            string[] roles = { };
            using (var db = new CustomerContext())
            {
                // Get application user (customer)
                var user = db.Customers.FirstOrDefault(c => c.Login == username || c.Email == username);
                if (user != null)
                {
                    // Get role
                    var roleIds = db.CustomersInRoles.Where(r => r.CustomerId == user.Id).Select(r => r.RoleId);
                    if (roleIds.Any())
                    {
                        roles = db.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.Name).ToArray();
                    }
                }
            }
            return roles;
        }

        public override void CreateRole(string roleName)
        {
            var newRole = new RoleEntity() { Name = roleName };
            using (var db = new CustomerContext())
            {
                db.Roles.Add(newRole);
                db.SaveChanges();
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new System.NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            return GetAllRoles().Contains(roleName);
        }

        public override void AddUsersToRoles(string[] userNames, string[] roleNames)
        {
            foreach (var rolename in roleNames)
            {
                if (string.IsNullOrEmpty(rolename))
                    throw new ProviderException("Role name cannot be empty or null.");
                if (!RoleExists(rolename))
                    throw new ProviderException("Role name not found.");
            }

            foreach (var username in userNames)
            {
                if (string.IsNullOrEmpty(username))
                    throw new ProviderException("User name cannot be empty or null.");

                foreach (var rolename in roleNames)
                {
                    if (IsUserInRole(username, rolename))
                        throw new ProviderException($"User {username} is already in role {rolename}.");
                }
            }

            using (var db = new CustomerContext())
            {
                foreach (var username in userNames)
                {
                    foreach (var rolename in roleNames)
                    {
                        var user = db.Customers.FirstOrDefault(c => c.Login == username || c.Email == username);
                        var role = db.Roles.FirstOrDefault(r => r.Name == rolename);
                        if (user != null && role != null)
                        {
                            db.CustomersInRoles.Add(new CustomerInRole() { CustomerId = user.Id, RoleId = role.Id });
                        }
                    }
                }
                db.SaveChanges();
            }
        }

        public override void RemoveUsersFromRoles(string[] userNames, string[] roleNames)
        {
            using (var db = new CustomerContext())
            {
                foreach (var username in userNames)
                {
                    foreach (var rolename in roleNames)
                    {
                        var user = db.Customers.FirstOrDefault(c => c.Login == username || c.Email == username);
                        var role = db.Roles.FirstOrDefault(r => r.Name == rolename);
                        if (user != null && role != null)
                        {
                            var customerInRole = db.CustomersInRoles.FirstOrDefault(cir => cir.CustomerId == user.Id && cir.RoleId == role.Id);
                            db.CustomersInRoles.Remove(customerInRole);
                        }
                    }
                }
                db.SaveChanges();
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new System.NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            string[] roles = {};
            using (var db = new CustomerContext())
            {
                roles = db.Roles.Select(r => r.Name).ToArray();
            }

            return roles;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new System.NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}