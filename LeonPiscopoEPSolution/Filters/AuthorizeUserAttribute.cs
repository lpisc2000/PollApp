using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LeonPiscopoEPSolution.Filters
{
    // Custom authorization filter: ensures user is logged in (session contains UserId)
    public class AuthorizeUserAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                // Not logged in, redirect to login page
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}
