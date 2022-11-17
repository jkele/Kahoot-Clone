using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRA__Projekt.Data;
using PRA__Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRA__Projekt.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuizController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Admin(string id)
        {
            var model = new Models.QuizQuestionsVM()
            {
                Quiz = _context.Quizs.Where(i => i.Code == id && i.UserId == User.Claims.FirstOrDefault().Value).FirstOrDefault()
            };

            if (model.Quiz == null)
            {
                return RedirectToAction("QuizNotFound");
            }
            model.Questions = _context.Questions.Where(i => i.QuizId == model.Quiz.Id).ToList();

            var players = _context.Users.Where(i => _context.Answers.Where(i => i.QuizId == model.Quiz.Id).Select(u => u.UserId).Contains(i.Id)).ToList().Select(i => i.UserName);
            ViewData["players"] = players;

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                quiz.Code = Guid.NewGuid().ToString().Replace("-", "").Substring(5, 5);
                quiz.UserId = User.Claims.FirstOrDefault().Value;

                _context.Add(quiz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CreateQuestions), new { id= quiz.Id });
            }
            return View(quiz);
        }

        public IActionResult CreateQuestions(int id)
        {
            return View(new Question { QuizId = id, Quiz = _context.Quizs.Find(id) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestions([Bind("QuestionText,QuizId")] Question q)
        {
            if (ModelState.IsValid)
            {
                _context.Add(q);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CreateQuestions), new { id = q.QuizId });
            }
            return RedirectToAction(nameof(CreateQuestions), new { id = q.QuizId });
        }

        public IActionResult QuizNotFound()
        {
            return View();
        }
    }
}
