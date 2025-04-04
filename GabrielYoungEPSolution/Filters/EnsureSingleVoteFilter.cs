using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using DataAccess.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Presentation.Filters
{
    public class EnsureSingleVoteFilter : ActionFilterAttribute
    {
        private readonly IPollRepository _pollRepository;

        public EnsureSingleVoteFilter(IPollRepository pollRepository)
        {
            _pollRepository = pollRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
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

            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (poll.VoterIds.Contains(userId))
            {
                // Set TempData via ITempDataDictionaryFactory
                var tempDataFactory = context.HttpContext.RequestServices.GetService(typeof(ITempDataDictionaryFactory)) as ITempDataDictionaryFactory;
                var tempData = tempDataFactory?.GetTempData(context.HttpContext);
                tempData["Alert"] = "You have already voted in this poll.";

                context.Result = new RedirectToActionResult("Details", "Poll", new { id = pollId });
            }
        }
    }

}