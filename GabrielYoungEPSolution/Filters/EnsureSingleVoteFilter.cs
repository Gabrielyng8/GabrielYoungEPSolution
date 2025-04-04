using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using DataAccess.Repositories;
using System.Security.Claims;

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
            if (!context.ActionArguments.TryGetValue("pollId", out var pollIdObj) ||
                !int.TryParse(pollIdObj?.ToString(), out var pollId))
            {
                context.Result = new BadRequestObjectResult("Invalid poll ID.");
                return;
            }

            var poll = _pollRepository.GetPollById(pollId);
            if (poll == null)
            {
                context.Result = new NotFoundResult();
                return;
            }

            if (poll.VoterIds.Contains(context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                context.Result = new BadRequestObjectResult("You have already voted.");
                return;
            }
        }
    }
}