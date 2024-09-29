using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WordPuzzle.Models
{
    public class PuzzleModel
    {
        #region Constants

        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        const string placeholder = "X";

        #endregion

        #region Properties

        [Display(Name = "Puzzle Size")]
        [Required(ErrorMessage = "Puzzle size is required")]
        [Range(0, 99, ErrorMessage = "Puzzle size should not contain characters")]
        public int? PuzzleSize { get; set; }

        public List<string> PuzzleWords { get; set; } = new List<string>();

        public string[,] PuzzleMatrix { get; set; }

        #endregion

        #region Random Generating Functions

        private Random random = new Random();

        private string GetRandomLetter()
        {
            return letters[random.Next(letters.Length)].ToString();
        }

        private WordDirection GetRandomWordDirection()
        {
            var index = random.Next(4);
            return (WordDirection)index;
        }

        private bool GetRandomBool()
        {
            var index = random.Next(2);
            return index == 1;
        }

        private int GetRandomPoint()
        {
            return random.Next(PuzzleSize.Value);
        }

        #endregion

        #region Puzzle Word Plot Functions

        public bool PlotWords()
        {
            bool wordPlotted = false;

            foreach (string puzzleWord in PuzzleWords)
            {
                WordDirection startingDirection = GetRandomWordDirection();
                WordDirection currentDirection = startingDirection;
                bool isReversed = GetRandomBool();

                do
                {
                    var word = puzzleWord;
                    if (isReversed)
                        word = ReverseWord(puzzleWord);

                    // try to plot the word using the assigned random direction
                    wordPlotted = PlotWord(word, currentDirection);

                    // if that didn't work, reverse the word and try again
                    if (!wordPlotted)
                    {
                        wordPlotted = PlotWord(ReverseWord(word), currentDirection);
                    }

                    // if that didn't work, change the word direction
                    if (!wordPlotted)
                    {
                        if ((int)currentDirection != Enum.GetNames(typeof(WordDirection)).Length)
                        {
                            currentDirection = (WordDirection)((int)currentDirection + 1);
                        }
                        else
                        {
                            currentDirection = 0;
                        }
                    }
                } while (!wordPlotted && currentDirection != startingDirection);

                if (!wordPlotted)
                    break;
            }

            return wordPlotted;
        }

        public void FillEmptySpacesWithRandomLetters()
        {
            for (int x = 0; x < PuzzleSize.Value; ++x)
                for (int y = 0; y < PuzzleSize.Value; ++y)
                {
                    if (PuzzleMatrix[x, y] == placeholder)
                    {
                        PuzzleMatrix[x, y] = GetRandomLetter();
                    }
                }
        }

        private bool PlotWord(string word, WordDirection direction)
        {
            bool success = false;

            switch (direction)
            {
                case WordDirection.Horizontal:
                    success = PlotHorizontalWord(word);
                    break;
                case WordDirection.Vertical:
                    success = PlotVerticalWord(word);
                    break;
                case WordDirection.DiagonalUp:
                    success = PlotDiagonalUpWord(word);
                    break;
                case WordDirection.DiagonalDown:
                    success = PlotDiagonalDownWord(word);
                    break;
            }

            return success;
        }

        private string ReverseWord(string word)
        {
            char[] charArray = word.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private bool IsSpaceAvailable(int row, int column, string word, WordDirection direction)
        {
            bool isAvailable = true;

            if (direction == WordDirection.Horizontal)
            {
                for (int x = column; x < column + word.Length; ++x)
                {
                    if (PuzzleMatrix[row, x] != placeholder && PuzzleMatrix[row, x] != word.Substring(x - column, 1))
                    {
                        isAvailable = false;
                        break;
                    }
                }
            }
            else if (direction == WordDirection.Vertical)
            {
                for (int x = row; x < row + word.Length; ++x)
                {
                    if (PuzzleMatrix[x, column] != placeholder && PuzzleMatrix[x, column] != word.Substring(x - row, 1))
                    {
                        isAvailable = false;
                        break;
                    }
                }
            }
            else if (direction == WordDirection.DiagonalUp)
            {
                for (int x = 0; x < word.Length; ++x)
                {
                    if (PuzzleMatrix[row - x, column + x] != placeholder && PuzzleMatrix[row - x, column + x] != word.Substring(x, 1))
                    {
                        isAvailable = false;
                        break;
                    }
                }
            }
            else if (direction == WordDirection.DiagonalDown)
            {
                for (int x = 0; x < word.Length; ++x)
                {
                    if (PuzzleMatrix[row + x, column + x] != placeholder && PuzzleMatrix[row + x, column + x] != word.Substring(x, 1))
                    {
                        isAvailable = false;
                        break;
                    }
                }
            }

            return isAvailable;
        }

        private bool PlotHorizontalWord(string word)
        {
            int startingRow = random.Next(PuzzleSize.Value);
            int startingColumn = random.Next(PuzzleSize.Value - word.Length + 1);
            int currentRow = startingRow;
            int currentColumn = startingColumn;
            bool spaceAvailable = false;

            // find a row that has space available, if there is one
            do
            {
                do
                {
                    spaceAvailable = IsSpaceAvailable(currentRow, currentColumn, word, WordDirection.Horizontal);
                    if (!spaceAvailable)
                    {
                        if (currentColumn != PuzzleSize.Value - word.Length + 1)
                            ++currentColumn;
                        else
                            currentColumn = 0;
                    }
                } while (!spaceAvailable && currentColumn != startingColumn);

                if (!spaceAvailable)
                {
                    if (currentRow != PuzzleSize.Value - 1)
                        ++currentRow;
                    else
                        currentRow = 0;
                }
            } while (!spaceAvailable && currentRow != startingRow);

            // if no row has space available, return false; otherwise plot the word at the given location
            if (!spaceAvailable)
                return false;
            else
            {
                for (int x = 0; x < word.Length; ++x)
                {
                    PuzzleMatrix[currentRow, currentColumn + x] = word.Substring(x, 1);
                }

            }

            return true;
        }

        private bool PlotVerticalWord(string word)
        {
            int startingRow = random.Next(PuzzleSize.Value - word.Length + 1);
            int startingColumn = random.Next(PuzzleSize.Value);
            int currentRow = startingRow;
            int currentColumn = startingColumn;
            bool spaceAvailable = false;

            do
            {
                do
                {
                    spaceAvailable = IsSpaceAvailable(currentRow, currentColumn, word, WordDirection.Vertical);
                    if (!spaceAvailable)
                    {
                        if (currentRow != PuzzleSize.Value - word.Length + 1)
                            ++currentRow;
                        else
                            currentRow = 0;
                    }
                } while (!spaceAvailable && currentRow != startingRow);

                if (!spaceAvailable)
                {
                    if (currentColumn != PuzzleSize.Value - 1)
                        ++currentColumn;
                    else
                        currentColumn = 0;
                }
            } while (!spaceAvailable && currentColumn != startingColumn);

            // if no row has space available, return false; otherwise plot the word at the given location
            if (!spaceAvailable)
                return false;
            else
            {
                for (int x = 0; x < word.Length; ++x)
                {
                    PuzzleMatrix[currentRow + x, currentColumn] = word.Substring(x, 1);
                }

            }

            return true;
        }

        private bool PlotDiagonalUpWord(string word)
        {
            int minimumRow = word.Length - 1;
            int maximumRow = PuzzleSize.Value - 1;

            int startingRow = random.Next(minimumRow, maximumRow);
            int startingColumn = random.Next(PuzzleSize.Value - word.Length);
            int currentRow = startingRow;
            int currentColumn = startingColumn;
            bool spaceAvailable = false;

            // find a row that has space available, if there is one
            do
            {
                do
                {
                    spaceAvailable = IsSpaceAvailable(currentRow, currentColumn, word, WordDirection.DiagonalUp);
                    if (!spaceAvailable)
                    {
                        if (currentColumn != word.Length - 1)
                            ++currentColumn;
                        else
                            currentColumn = 0;
                    }
                } while (!spaceAvailable && currentColumn != startingColumn);

                if (!spaceAvailable)
                {
                    if (currentRow != PuzzleSize.Value - word.Length - 1)
                        ++currentRow;
                    else
                        currentRow = 0;
                }
            } while (!spaceAvailable && currentRow != startingRow);

            // if no row has space available, return false; otherwise plot the word at the given location
            if (!spaceAvailable)
                return false;
            else
            {
                for (int x = 0; x < word.Length; ++x)
                {
                    PuzzleMatrix[currentRow - x, currentColumn + x] = word.Substring(x, 1);
                }

            }

            return true;
        }

        private bool PlotDiagonalDownWord(string word)
        {
            int startingRow = random.Next(PuzzleSize.Value - word.Length);
            int startingColumn = random.Next(PuzzleSize.Value - word.Length);
            int currentRow = startingRow;
            int currentColumn = startingColumn;
            bool spaceAvailable = false;

            // find a row that has space available, if there is one
            do
            {
                do
                {
                    spaceAvailable = IsSpaceAvailable(currentRow, currentColumn, word, WordDirection.DiagonalDown);
                    if (!spaceAvailable)
                    {
                        if (currentColumn != PuzzleSize.Value - word.Length - 1)
                            ++currentColumn;
                        else
                            currentColumn = 0;
                    }
                } while (!spaceAvailable && currentColumn != startingColumn);

                if (!spaceAvailable)
                {
                    if (currentRow != word.Length - 1)
                        ++currentRow;
                    else
                        currentRow = 0;
                }
            } while (!spaceAvailable && currentRow != startingRow);

            // if no row has space available, return false; otherwise plot the word at the given location
            if (!spaceAvailable)
                return false;
            else
            {
                for (int x = 0; x < word.Length; ++x)
                {
                    PuzzleMatrix[currentRow + x, currentColumn + x] = word.Substring(x, 1);
                }

            }

            return true;
        }

        #endregion

        #region Constructors

        public PuzzleModel() { }

        public PuzzleModel(int? puzzleSize, string puzzleWords)
        {
            PuzzleSize = puzzleSize;

            // initialize matrix with empty strings
            PuzzleMatrix = new string[puzzleSize.Value, puzzleSize.Value];
            for (int x = 0; x < puzzleSize.Value; ++x)
            {
                for (int y = 0; y < puzzleSize.Value; ++y)
                {
                    PuzzleMatrix[x, y] = placeholder;
                }
            }

            // initialize word list
            if (!string.IsNullOrEmpty(puzzleWords))
            {
                ICollection<string> words = puzzleWords.ToUpper().Split('\n');
                foreach (string word in words)
                {
                    PuzzleWords.Add(word);
                }
            }
            
            // DEBUG LINE - WE WILL MOVE THESE LAST TWO FUNCTIONS TO THE CONTROLLER WHEN WE ADD VALIDATION
            // DEBUG LINE - FOR NOW, WE'LL TEST THESE FUNCTIONS HERE IN THE MODEL CONSTRUCTOR
            // plot puzzle words
            bool success = PlotWords();

            // fill in remaining spaces with random letters
            FillEmptySpacesWithRandomLetters();
        }

        #endregion        
    }
}