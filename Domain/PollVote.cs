using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class PollVote
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public int UserId { get; set; }

        // Navigation properties (optional for EF Core)
        public Poll? Poll { get; set; }
        public User? User { get; set; }
    }
}
