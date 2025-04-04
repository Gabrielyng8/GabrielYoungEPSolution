using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PollRepository
    {
        private PollDbContext context;

        // Constructor to initialize the context using dependency injection
        public PollRepository(PollDbContext _context) {
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
