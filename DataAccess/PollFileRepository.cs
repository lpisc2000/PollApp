using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PollFileRepository : IPollRepository
    {
        private readonly string _filePath;
        private List<Poll> _polls;
        // In-memory record of votes cast (not persisted to file)
        private readonly HashSet<(int PollId, int UserId)> _votes = new HashSet<(int, int)>();

        public PollFileRepository(string filePath = "polls.json")
        {
            _filePath = filePath;
            _polls = File.Exists(_filePath)
                ? JsonSerializer.Deserialize<List<Poll>>(File.ReadAllText(_filePath)) ?? new List<Poll>()
                : new List<Poll>();
        }

        private void SaveToFile()
        {
            // Write the current _polls list to the JSON file
            var json = JsonSerializer.Serialize(_polls, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public IEnumerable<Poll> GetAllPollsSortedByDate()
        {
            return _polls.OrderByDescending(p => p.Date).ToList();
        }

        public Poll? GetPollById(int id)
        {
            return _polls.FirstOrDefault(p => p.Id == id);
        }

        public void AddPoll(Poll poll)
        {
            // Assign a new ID (simple incremental ID based on current list)
            int newId = _polls.Any() ? _polls.Max(p => p.Id) + 1 : 1;
            poll.Id = newId;
            poll.Date = DateTime.Now;
            _polls.Add(poll);
            SaveToFile();
        }

        public void DeletePoll(int id)
        {
            var poll = _polls.FirstOrDefault(p => p.Id == id);
            if (poll != null)
            {
                _polls.Remove(poll);
                _votes.RemoveWhere(v => v.PollId == id);
                SaveToFile();
            }
        }


        public void VotePoll(int pollId, int userId, int optionNumber)
        {
            var poll = _polls.FirstOrDefault(p => p.Id == pollId);
            if (poll == null) return;
            // If user already voted on this poll (according to in-memory record), do nothing
            if (_votes.Contains((pollId, userId))) return;

            // Increment vote count
            switch (optionNumber)
            {
                case 1: poll.Votes1++; break;
                case 2: poll.Votes2++; break;
                case 3: poll.Votes3++; break;
            }
            _votes.Add((pollId, userId));
            SaveToFile();
        }

        public bool HasUserVoted(int pollId, int userId)
        {
            return _votes.Contains((pollId, userId));
        }

        public User? ValidateUser(string username, string password)
        {
            // For simplicity, file-based repository does not handle user credentials
            return null; // Not supported in this repository
        }
    }
}
