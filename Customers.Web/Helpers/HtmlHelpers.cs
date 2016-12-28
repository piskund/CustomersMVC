using System.Web.Mvc;

namespace Customers.Web.Helpers
{
    public static class HtmlHelpers
    {
        public static string IsActive(this HtmlHelper htmlHelper, string controller, string action)
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            var routeAction = routeData.Values["action"].ToString();
            var routeController = routeData.Values["controller"].ToString();

            var returnActive = (controller == routeController) && (action == routeAction);

            return returnActive ? "active" : "";
        }
    }
}