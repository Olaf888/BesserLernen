using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using BesserLernen.Web.Models;
using BesserLernen.Web.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.Data.Entity;

namespace BesserLernen.Web.Controllers
{
    [Authorize]
    public class SurveyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SurveyController(ApplicationDbContext context, 
                                UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var answers = GetCurrentUserAnswers();

            if (answers.Any())
            {
                var vm = answers.ToList();
                foreach (var v in vm) // TODO make EF load Questions automatically.
                    v.Question = _context.SurveyQuestions.First(q => q.Id == v.QuestionId);
                return View(vm);
            }

            return RedirectToAction("Take");
        }

        private IQueryable<SurveyAnswer> GetCurrentUserAnswers()
        {
            return _context.SurveyAnswers
                .Where(u => u.UserId == HttpContext.User.GetUserId());
        }

        public IActionResult Take()
        {
            if(CurrentUserHasAnswers())
                return RedirectToAction("Index");

            return View(_context.SurveyQuestions
                                .OrderBy(b=>b.SortPosition)
                                .Select(s=>new SurveyQuestionViewModel { QuestionId = s.Id, Title = s.Title})
                                .ToList());
        }

        public IActionResult Retake()
        {
            _context.SurveyAnswers.RemoveRange(GetCurrentUserAnswers().ToList());
            _context.SaveChanges();
            return RedirectToAction("Take");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(IList<SurveyQuestionViewModel> answers)
        {
            var user = await GetCurrentUserAsync();
            if(CurrentUserHasAnswers())
                return RedirectToAction("Index"); // TODO: add warning message
            
            foreach (var submittedAnswer in answers)
            {
                // TODO: what to to with non-answers?
                if (submittedAnswer.AnsweredYes == null)
                    continue; 

                // make sure we process only valid data
                var q = await _context.SurveyQuestions
                                      .FirstOrDefaultAsync(s => s.Id == submittedAnswer.QuestionId);
                if (q == null) continue;
                
                var answer = new SurveyAnswer
                {
                    Question = q, User = user,
                    AnsweredYes = submittedAnswer.AnsweredYes == true, 
                };
                _context.SurveyAnswers.Add(answer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CurrentUserHasAnswers()
        {
            return _context.SurveyAnswers.Any(u => u.UserId == HttpContext.User.GetUserId());
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
        }
    }
}
