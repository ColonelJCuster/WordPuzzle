using System.ComponentModel.DataAnnotations;

namespace WordPuzzle.ViewModels
{
    public class PuzzleViewModel
    {
        [Display(Name = "Puzzle Size")]
        [Required(ErrorMessage = "Puzzle size is required")]
        [Range(0, 99, ErrorMessage = "Puzzle size should not contain characters")]
        public int? PuzzleSize { get; set; }

        public string PuzzleWordList { get; set; }
    }
}