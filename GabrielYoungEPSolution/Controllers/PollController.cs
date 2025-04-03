using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class PollController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create([FromServices] PollRepository pollRepository)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromServices] PollRepository pollRepository, string title, string option1Text, string option2Text, string option3Text)
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
    }
}
