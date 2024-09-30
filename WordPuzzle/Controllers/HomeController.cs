using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using WordPuzzle.Models;
using WordPuzzle.ViewModels;

namespace WordPuzzle.Controllers
{
    public class HomeController : Controller
    {
        static private PuzzleModel puzzleModel = new PuzzleModel();
        static private PuzzleViewModel puzzleViewModel = new PuzzleViewModel();

        [HttpGet]
        public ActionResult Index()
        {
            // set default values
            puzzleViewModel.PuzzleSize = 0;
            puzzleViewModel.PuzzleWordList = "";

            return View(puzzleViewModel);
        }

        [HttpPost]
        public ActionResult BuildPuzzle(PuzzleViewModel puzzleViewModel)
        {
            var model = new PuzzleModel(puzzleViewModel.PuzzleSize, puzzleViewModel.PuzzleWordList);

            if (model.PuzzleWords.Count > model.PuzzleSize.Value)
            {
                ModelState.AddModelError(string.Empty, "You have entered too many words. The number of words must be less than the puzzle size.");
            }

            foreach (string word in model.PuzzleWords)
            {
                if (word.Length > model.PuzzleSize.Value)
                {
                    ModelState.AddModelError(string.Empty, "One or more words is too long. All words must be less than or equal to the puzzle size.");
                    break;
                }

                Regex r = new Regex(@"^[a-zA-Z]+$");
                if (!r.IsMatch(word))
                {
                    ModelState.AddModelError(string.Empty, "Invalid characters. Only letters and newlines allowed.");
                    break;
                }
            }

            if (!ModelState.IsValid)
            {
                string errorString = "";
                foreach (var error in ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors))
                {
                    if (string.IsNullOrEmpty(errorString))
                        errorString += error.ErrorMessage;
                    else
                        errorString += "<br>" + error.ErrorMessage;
                }
                
                var errorModel = new { errors = errorString };
                return new JsonHttpStatusResult(errorModel, HttpStatusCode.BadRequest);
            }
            else
            {
                model.PlotWords();
                model.FillEmptySpacesWithRandomLetters();

                puzzleModel = model;
                return PartialView("_puzzle", model);
            }            
        }

        [HttpGet]
        public ActionResult ToggleAnswers()
        {
            if (puzzleModel.ShowAnswers)
                puzzleModel.ShowAnswers = false;
            else
                puzzleModel.ShowAnswers = true;

            return PartialView("_puzzle", puzzleModel);
        }
    }

    public class JsonHttpStatusResult : JsonResult
    {
        private readonly HttpStatusCode _httpStatus;

        public JsonHttpStatusResult(object data, HttpStatusCode httpStatus)
        {
            Data = data;
            _httpStatus = httpStatus;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.RequestContext.HttpContext.Response.StatusCode = (int)_httpStatus;
            base.ExecuteResult(context);
        }
    }
}