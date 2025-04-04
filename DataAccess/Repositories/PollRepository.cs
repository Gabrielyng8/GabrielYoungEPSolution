using DataAccess.DataContext;
using Domain.Models;
using System.Linq;

namespace DataAccess.Repositories
{
    public class PollRepository : IPollRepository
    {
        private PollDbContext context;

        public PollRepository(PollDbContext _context)
        {
            context = _context;
        }

        public IQueryable<Poll> GetPolls()
        {
            return context.Polls;
        }

        public Poll GetPollById(int id)
        {
            return context.Polls.FirstOrDefault(p => p.Id == id);
        }

        public void CreatePoll(Poll poll)
        {
            context.Polls.Add(poll);
            context.SaveChanges();
        }

        public void Vote(int pollId, int option)
        {
            var poll = context.Polls.FirstOrDefault(p => p.Id == pollId);
            if (poll != null)
            {
                switch (option)
                {
                    case 1:
                        poll.Option1VotesCount++;
                        break;
                    case 2:
                        poll.Option2VotesCount++;
                        break;
                    case 3:
                        poll.Option3VotesCount++;
                        break;
                    default:
                        throw new ArgumentException("Invalid option");
                }
                context.SaveChanges();
            }
        }
    }
}
