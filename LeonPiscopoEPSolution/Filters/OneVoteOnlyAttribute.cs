using DataAccess;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LeonPiscopoEPSolution.Filters
{
    // Action filter: allows execution only if the user hasn't voted on the poll yet
    public class OneVoteOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue && context.ActionArguments.TryGetValue("id", out var pollIdObj))
            {
                int pollId = (int)pollIdObj;
                // Resolve the repository service to check vote status
                var repo = context.HttpContext.RequestServices.GetService<IPollRepository>();
                if (repo != null && repo.HasUserVoted(pollId, userId.Value))
                {
                    // User already voted on this poll – redirect to results page instead of executing action
                    context.Result = new RedirectToActionResult("Results", "Poll", new { id = pollId });
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
