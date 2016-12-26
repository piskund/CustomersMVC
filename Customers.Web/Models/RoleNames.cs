using System.Collections.Generic;
using System.Linq;
using System.Text;
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
         
        public static IEnumerable<string> GetRolesWithAcccessToSite()
        {
            yield return Administrator;
            yield return Manager;
            yield return Operator;
        }

        public static string GetAllowedToRead()
        {
            var sb = new StringBuilder();
            GetRolesWithAcccessToSite().ForEach(r => sb.Append(r + Separator));
            return sb.ToString().TrimEnd(Separator.ToCharArray());
        }

        public static IEnumerable<string> GetAllRoles()
        {
            return GetRolesWithAcccessToSite().Concat(new [] { Customer });
        }
    }
}