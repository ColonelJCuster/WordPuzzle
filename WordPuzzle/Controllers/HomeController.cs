using System.Web.Mvc;
using WordPuzzle.Models;
using WordPuzzle.ViewModels;

namespace WordPuzzle.Controllers
{
    public class HomeController : Controller
    {
        static private PuzzleViewModel Puzzle = new PuzzleViewModel();

        [HttpGet]
        public ActionResult Index()
        {
            // set default values
            Puzzle.PuzzleSize = 0;
            Puzzle.PuzzleWordList = "";

            return View(Puzzle);
        }

        [HttpGet]
        public ActionResult BuildPuzzle(int puzzleSize, string puzzleWords)
        {
            var model = new PuzzleModel(puzzleSize, puzzleWords);

            return PartialView("_puzzle", model);
        }
    }
}