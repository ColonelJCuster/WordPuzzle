using System.ComponentModel.DataAnnotations;

namespace WordPuzzle.Models
{
    public enum WordDirection
    {
        Horizontal,
        Vertical,
        DiagonalUp,
        DiagonalDown
    }

    public class PuzzleWordModel
    {
        [RegularExpression("[A-Za-z)")]
        [Required]
        public string Word { get; set; }

        public WordDirection Direction { get; set; }

        public bool IsReversed { get; set; }

        public int StartingRow { get; set; }

        public int StartingColumn { get; set; }
    }
}