using System.Collections.Generic;

namespace Customers.Web.Models
{
    public static class RoleNames
    {
        public const string Administrator = "Administrator";
        public const string Customer = "Customer";
        public const string Operator = "Operator";
        public const string Manager = "Manager";

        public static IEnumerable<string> GetAllRoles()
        {
            yield return Administrator;
            yield return Customer;
            yield return Manager;
            yield return Operator;
        }
    }
}