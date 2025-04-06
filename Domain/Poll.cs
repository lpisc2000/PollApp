namespace Domain
{
    public class Poll
    {
        public int Id { get; set; }

        // Poll question/title
        public string Title { get; set; } = string.Empty;

        // Three options for the poll
        public string Option1 { get; set; } = string.Empty;
        public string Option2 { get; set; } = string.Empty;
        public string Option3 { get; set; } = string.Empty;

        // Vote counts for each option
        public int Votes1 { get; set; } = 0;
        public int Votes2 { get; set; } = 0;
        public int Votes3 { get; set; } = 0;

        // Date poll was created
        public DateTime Date { get; set; }
    }
}
