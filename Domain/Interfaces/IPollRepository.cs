using Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Repositories
{
    public interface IPollRepository
    {
        IQueryable<Poll> GetPolls();
        Poll GetPollById(int id);
        void CreatePoll(Poll poll);
        void Vote(int pollId, int option);
    }
}