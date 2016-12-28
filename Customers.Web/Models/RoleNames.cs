using System.Collections.Generic;
using System.Linq;
using System.Text;
using Customers.Web.DAL;
using WebGrease.Css.Extensions;

namespace Customers.Web.Models
{
    public static class RoleNames
    {
        public const string Administrator = "Administrator";
        public const string Customer = "Customer";
        public const string Operator = "Operator";
        public const string Manager = "Manager";

        public const string AllowedToRead = Administrator + Separator + Manager + Separator + Operator;
        public const string AllowedToModify = Administrator + Separator + Manager;

        private const string Separator = ",";
         
        public static IEnumerable<string> GetRoleNamesWithAcccessToSite()
        {
            yield return Administrator;
            yield return Manager;
            yield return Operator;
        }

        public static string GetAllowedToRead()
        {
            var sb = new StringBuilder();
            GetRoleNamesWithAcccessToSite().ForEach(r => sb.Append(r + Separator));
            return sb.ToString().TrimEnd(Separator.ToCharArray());
        }

        public static IEnumerable<string> GetAllRoleNames()
        {
            return GetRoleNamesWithAcccessToSite().Concat(new [] { Customer });
        }

        public static int[] GetAllRoleIds()
        {
            return GetAllRoleNames().Select(n => GetRoleIdByName(n).Value).ToArray();
        }

        public static int? GetRoleIdByName(string name)
        {
            int? result;

            using (var context = new CustomerContext())
            {
                var roleEntity = context.Roles.FirstOrDefault(r => r.Name == name);
                result = roleEntity?.Id;
            }

            return result;
        }
    }
}