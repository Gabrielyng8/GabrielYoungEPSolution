using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using DataAccess.Repositories;

namespace Presentation.Filters
{
    public class EnsureSingleVoteFilter : IActionFilter
    {
        private readonly IPollRepository _pollRepository;

        public EnsureSingleVoteFilter(IPollRepository pollRepository)
        {
            _pollRepository = pollRepository;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var pollId = (int)context.ActionArguments["pollId"];
            var userId = context.HttpContext.User.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var poll = _pollRepository.GetPollById(pollId);
            if (poll == null)
            {
                context.Result = new NotFoundResult();
                return;
            }

            // ACheck voterIDs list to see if the user has already voted
            if (poll.VoterIds.Contains(userId))
            {
                context.Result = new BadRequestObjectResult("You have already voted.");
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No action needed after execution
        }
    }
}