using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRA__Projekt.Data;
using PRA__Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRA__Projekt.Controllers
{
    [Authorize]
    public class PlayController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlayController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string id)
        {
            var q = _context.Quizs.Where(i => i.Code == id).FirstOrDefault();

            if (q == null)
            {
                return RedirectToAction("QuizNotFound");
            }

            if (!q.Started)
            {
                return View("NotStarted");
            }

            var questions = _context.Questions.Where(i => i.QuizId == q.Id).ToList();
            string usersid = User.Claims.FirstOrDefault().Value;
            foreach (var question in questions)
            {
                if (_context.Answers.Where(i => i.UserId == usersid && i.QuizId == q.Id && i.QuestionId == question.Id).Any())
                {

                }
                else
                {
                    var answer = new Models.Answer()
                    {
                        QuestionId = question.Id,
                        QuizId = q.Id,
                        UserId = User.Claims.FirstOrDefault().Value
                    };
                    _context.Add(answer);
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Game", new { id = id });
        }

        public IActionResult Game(string id)
        {
            var data = _context.Quizs.Where(i =>i.Code == id).FirstOrDefault();
            if (data == null)
            {
                return NotFound();
            }


            var q = _context.Questions.Where(i => i.QuizId == data.Id && !i.Ended).OrderBy(i => i.Order).ThenBy(i => i.Id).FirstOrDefault();

            if (q == null)
            {
                return RedirectToAction(nameof(Thankyou));
            }

            var answer = _context.Answers.Where(i => i.QuestionId == q.Id && i.UserId == User.Claims.FirstOrDefault().Value).FirstOrDefault();

            ViewData["Question"] = q.QuestionText;
            ViewData["TrueAnswerId"] = answer.Id;

            return View(answer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Answer(int id, [Bind("Id,AnswerText")] Answer a)
        {
            if (id != a.Id)
            {
                return NotFound();
            }

            var answer = await _context.Answers.Include(i => i.Quiz).Where(i => i.Id == id).FirstOrDefaultAsync();
            answer.AnswerText = a.AnswerText;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Game), new { id = answer.Quiz.Code });
        }

        public IActionResult QuizNotFound()
        {
            return View();
        }

        public IActionResult NotStarted()
        {
            return View();
        }

        public IActionResult Thankyou()
        {
            return View();
        }
    }
}
