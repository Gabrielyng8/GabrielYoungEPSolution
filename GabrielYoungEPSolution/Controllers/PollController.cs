using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;
using System.Security.Claims;

namespace Presentation.Controllers
{
    public class PollController : Controller
    {
        [HttpGet]
        public IActionResult Index([FromServices] IPollRepository pollRepository)
        {
            var polls = pollRepository.GetPolls().OrderByDescending(p => p.DateCreated);
            return View(polls);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create([FromServices] IPollRepository pollRepository)
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create([FromServices] IPollRepository pollRepository, string title, string option1Text, string option2Text, string option3Text)
        {
            var poll = new Domain.Models.Poll
            {
                Title = title,
                Option1Text = option1Text,
                Option2Text = option2Text,
                Option3Text = option3Text,
                Option1VotesCount = 0,
                Option2VotesCount = 0,
                Option3VotesCount = 0,
                DateCreated = DateTime.Now
            };

            pollRepository.CreatePoll(poll);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details([FromServices] IPollRepository pollRepository, int id)
        {
            var poll = pollRepository.GetPollById(id);
            if (poll == null)
            {
                return NotFound();
            }
            return View(poll);
        }

        [HttpPost]
        [Authorize]
        [ServiceFilter(typeof(EnsureSingleVoteFilter))] // applies the filter!
        public IActionResult Vote([FromServices] IPollRepository pollRepository, int pollId, int option)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                pollRepository.Vote(pollId, option, userId);
                return RedirectToAction("Details", new { id = pollId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Vote(int pollId)
        {
            return RedirectToAction("Details", new { id = pollId });
        }
    }
}