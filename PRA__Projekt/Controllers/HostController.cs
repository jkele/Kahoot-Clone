using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRA__Projekt.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRA__Projekt.Controllers
{
    [Authorize]
    public class HostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HostController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.Quizs.Where(i => i.UserId == User.Claims.FirstOrDefault().Value).ToList();
            return View(data);
        }

        public IActionResult Details(string id)
        {
            var data = _context.Quizs.Where(i => i.UserId == User.Claims.FirstOrDefault().Value && i.Code == id).FirstOrDefault();
            if (data == null)
            {
                return NotFound();
            }

            if (!data.Started)
            {
                data.Started = true;
                _context.SaveChanges();
            }

            var q = _context.Questions.Include(i => i.Quiz).Where(i => i.QuizId == data.Id && !i.Ended).OrderBy(i => i.Order).ThenBy(i => i.Id).FirstOrDefault();

            if (q == null)
            {
                data.Ended = true;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            var answers = _context.Answers.AsNoTracking().Where(i => i.QuestionId == q.Id).ToList();

            foreach (var a in answers)
            {
                a.UserId = _context.Users.Find(a.UserId).UserName;
            }


            var vm = new Models.HostQuestionVM()
            {
                Question = q,
                Answers = answers
            };

            return View(vm);
        }

        public IActionResult NextQuestion(string id)
        {
            var data = _context.Quizs.Where(i => i.UserId == User.Claims.FirstOrDefault().Value && i.Code == id).FirstOrDefault();
            if (data == null)
            {
                return NotFound();
            }

            var q = _context.Questions.Include(i => i.Quiz).Where(i => i.QuizId == data.Id && !i.Ended).OrderBy(i => i.Order).ThenBy(i => i.Id).FirstOrDefault();
            q.Ended = true;
            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = id });
        }

        public IActionResult Summary(string id)
        {
            var model = new Models.QuizSummaryVM();
            model.Quiz = _context.Quizs.Where(i => i.UserId == User.Claims.FirstOrDefault().Value && i.Code == id).FirstOrDefault();
            if (model.Quiz == null)
            {
                return NotFound();
            }

            model.Questions = _context.Questions.Where(i => i.QuizId == model.Quiz.Id).OrderBy(i => i.Order).ThenBy(i => i.Id).ToList();

            model.Answers = _context.Answers.Where(i => i.QuizId == model.Quiz.Id).OrderBy(i => i.QuestionId).ToList();

            var userlist = _context.Users.AsNoTracking().ToList();
            foreach (var item in model.Answers)
            {
                item.UserId = userlist.Where(i => i.Id == item.UserId).FirstOrDefault().UserName;
            }

            return View(model);
        }
    }
}
