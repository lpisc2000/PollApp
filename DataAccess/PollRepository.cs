using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PollRepository : IPollRepository
    {
        private readonly PollDbContext _context;
        public PollRepository(PollDbContext context)
        {
            _context = context; // DbContext is injected (constructor injection)
        }

        public IEnumerable<Poll> GetAllPollsSortedByDate()
        {
            // Fetch all polls ordered by date descending
            return _context.Polls
                .OrderByDescending(p => p.Date)
                .AsNoTracking()
                .ToList();
        }

        public Poll? GetPollById(int id)
        {
            // Include is not needed for vote counts because they are in Poll,
            // PollVotes could be included if needed to check voted users
            return _context.Polls.Find(id);
        }

        public void AddPoll(Poll poll)
        {
            _context.Polls.Add(poll);
            _context.SaveChanges();
        }

        public void DeletePoll(int id)
        {
            var poll = _context.Polls.Find(id);
            if (poll != null)
            {
                _context.Polls.Remove(poll);

                // Also delete votes linked to this poll
                var votes = _context.PollVotes.Where(v => v.PollId == id);
                _context.PollVotes.RemoveRange(votes);

                _context.SaveChanges();
            }
        }

        public void VotePoll(int pollId, int userId, int optionNumber)
        {
            var poll = _context.Polls.Find(pollId);
            if (poll == null) return;
            // Increment the selected option's vote count
            switch (optionNumber)
            {
                case 1: poll.Votes1++; break;
                case 2: poll.Votes2++; break;
                case 3: poll.Votes3++; break;
            }
            // Record the vote in PollVotes table
            var voteRecord = new PollVote { PollId = pollId, UserId = userId };
            _context.PollVotes.Add(voteRecord);
            _context.SaveChanges();
        }

        public bool HasUserVoted(int pollId, int userId)
        {
            // Check if a PollVote entry exists for this user and poll
            return _context.PollVotes.Any(pv => pv.PollId == pollId && pv.UserId == userId);
        }

        public User? ValidateUser(string username, string password)
        {
            // Verify user credentials (plaintext password for simplicity)
            return _context.Users
                .SingleOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
