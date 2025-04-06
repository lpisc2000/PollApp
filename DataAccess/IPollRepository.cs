using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IPollRepository
    {
        IEnumerable<Poll> GetAllPollsSortedByDate();
        Poll? GetPollById(int id);
        void AddPoll(Poll poll);
        void VotePoll(int pollId, int userId, int optionNumber);
        void DeletePoll(int id);
        bool HasUserVoted(int pollId, int userId);
        User? ValidateUser(string username, string password);
    }
}
