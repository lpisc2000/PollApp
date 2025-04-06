using DataAccess;
using Domain;
using LeonPiscopoEPSolution.Filters;
using LeonPiscopoEPSolution.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LeonPiscopoEPSolution.Controllers
{
    [AuthorizeUser] // User must be logged in for any action in this controller
    public class PollController : Controller
    {
        // GET: /Poll/Index - list all polls
        public IActionResult Index([FromServices] IPollRepository repo)
        {
            var polls = repo.GetAllPollsSortedByDate();
            return View(polls);
        }

        // GET: /Poll/Create - show form to create a new poll
        public IActionResult Create()
        {
            return View(); // returns empty form (ViewModel will be initialized in view)
        }

        // POST: /Poll/Create - handle new poll submission
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(PollCreateViewModel model, [FromServices] IPollRepository repo)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // return with validation errors if any
            }
            // Map ViewModel to Domain Poll model
            var newPoll = new Poll
            {
                Title = model.Title,
                Option1 = model.Option1,
                Option2 = model.Option2,
                Option3 = model.Option3,
                Date = DateTime.Now
            };
            repo.AddPoll(newPoll);
            return RedirectToAction("Index");
        }

        // GET: /Poll/Delete/5
        public IActionResult Delete(int id, [FromServices] IPollRepository repo)
        {
            var poll = repo.GetPollById(id);
            if (poll == null) return NotFound();
            return View(poll); // confirm delete
        }

        // POST: /Poll/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, [FromServices] IPollRepository repo)
        {
            repo.DeletePoll(id);
            return RedirectToAction("Index");
        }


        // GET: /Poll/Vote/{id} - show a poll and voting options
        public IActionResult Vote(int id, [FromServices] IPollRepository repo)
        {
            var poll = repo.GetPollById(id);
            if (poll == null) return NotFound();
            return View(poll);
        }

        // POST: /Poll/Vote/{id} - submit a vote for a poll
        [HttpPost, ValidateAntiForgeryToken, OneVoteOnly]
        public IActionResult Vote(int id, int selectedOption, [FromServices] IPollRepository repo)
        {
            // At this point, the OneVoteOnly filter has ensured the user hasn't voted yet.
            int userId = HttpContext.Session.GetInt32("UserId")!.Value;
            repo.VotePoll(id, userId, selectedOption);
            return RedirectToAction("Results", new { id });
        }

        // GET: /Poll/Results/{id} - display results for a poll
        public IActionResult Results(int id, [FromServices] IPollRepository repo)
        {
            var poll = repo.GetPollById(id);
            if (poll == null) return NotFound();
            return View(poll);
        }
    }
}
