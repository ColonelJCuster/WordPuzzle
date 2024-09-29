using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WordPuzzle.Models
{
    public class PuzzleModel
    {
        #region Constants

        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        #endregion

        [Required]
        public int? PuzzleSize { get; set; }

        public List<PuzzleWordModel> PuzzleWords { get; set; } = new List<PuzzleWordModel>();

        public char[,] PuzzleMatrix { get; set; }

        public PuzzleModel()
        {

        }

        public PuzzleModel(int? puzzleSize, string puzzleWords)
        {
            PuzzleSize = puzzleSize;
            // TODO: RANDOMIZE PUZZLE LOCATION FIELDS
            ICollection<string> words = puzzleWords.Split('\n');
            foreach (string word in words)
            {
                PuzzleWords.Add(new PuzzleWordModel
                {
                    Word = word,
                    Direction = WordDirection.Horizontal,
                    IsReversed = false,
                    StartingRow = 0,
                    StartingColumn = 0
                });
            }
            // TODO: ACTUALLY PLOT WORDS AT RANDOM IN MATRIX; FOR NOW, JUST FILL THE BOX FOR TESTING PURPOSES
            PuzzleMatrix = new char[puzzleSize.Value, puzzleSize.Value];
            for (int x = 0; x < puzzleSize.Value; ++x)
            {
                for (int y = 0; y < puzzleSize.Value; ++y)
                {
                    PuzzleMatrix[x, y] = letters.Substring(x, 1).ToCharArray()[0];
                }
            }
        }
    }
}