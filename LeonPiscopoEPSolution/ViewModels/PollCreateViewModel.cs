using System.ComponentModel.DataAnnotations;

namespace LeonPiscopoEPSolution.ViewModels
{
    public class PollCreateViewModel
    {
        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Option1 { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Option2 { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Option3 { get; set; } = string.Empty;
    }
}

