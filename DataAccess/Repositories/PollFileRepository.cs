using Domain.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace DataAccess.Repositories
{
    public class PollFileRepository
    {
        private readonly string _filePath;

        public PollFileRepository(string filePath)
        {
            _filePath = filePath;
        }

        public void CreatePoll(Poll poll)
        {
            var polls = GetPolls().ToList();
            poll.Id = polls.Any() ? polls.Max(p => p.Id) + 1 : 1;
            polls.Add(poll);
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(polls, Newtonsoft.Json.Formatting.Indented));
        }

        public IEnumerable<Poll> GetPolls()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Poll>();
            }

            var jsonData = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Poll>>(jsonData) ?? new List<Poll>();
        }
    }
}
