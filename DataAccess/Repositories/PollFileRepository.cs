using Domain.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAccess.Repositories
{
    public class PollFileRepository : IPollRepository
    {
        private readonly string _filePath;

        public PollFileRepository(string filePath)
        {
            _filePath = filePath;
        }

        public IQueryable<Poll> GetPolls()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Poll>().AsQueryable();
            }

            var jsonData = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Poll>>(jsonData)?.AsQueryable() ?? new List<Poll>().AsQueryable();
        }

        public Poll GetPollById(int id)
        {
            return GetPolls().FirstOrDefault(p => p.Id == id);
        }

        public void CreatePoll(Poll poll)
        {
            var polls = GetPolls().ToList();
            poll.Id = polls.Any() ? polls.Max(p => p.Id) + 1 : 1;
            polls.Add(poll);
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(polls, Formatting.Indented));
        }

        public void Vote(int pollId, int option, string userId)
        {
            var polls = GetPolls();
            var poll = polls.FirstOrDefault(p => p.Id == pollId);
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
                poll.VoterIds.Add(userId);
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(polls, Formatting.Indented));
            }
        }
    }
}
