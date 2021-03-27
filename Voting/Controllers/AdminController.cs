using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Voting.Models;

namespace Voting.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        DBContext db = new DBContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Addquestion()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Addquestion(Question q)
        {
            Question quest = new Question()
            {
                QuestionTitle=q.QuestionTitle
            };
            db.Questions.Add(quest);
            db.SaveChanges();
            return RedirectToAction("Addanswer",new { id = quest.ID });
        }
        public ActionResult Addanswer(int id)
        {
            Question q = db.Questions.Where(n => n.ID == id).SingleOrDefault();
            ViewBag.question = q;
            ViewBag.answer = db.Answers.Where(n => n.QuestionId == id).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Addanswer(Answer ans)
        {
            Answer a = new Answer()
            {
                AnswerTitle=ans.AnswerTitle,
                QuestionId=ans.QuestionId,
                Count=0
            };
            db.Answers.Add(a);
            db.SaveChanges();
            return RedirectToAction("Addanswer",new { id=a.QuestionId});
        }
        public ActionResult ShowQuestion()
        {
            List<Question> q = db.Questions.ToList();
            return View(q);
        }
        public ActionResult ShowVote(int id)
        {
            List<Answer> ans = db.Answers.Where(n => n.QuestionId == id).ToList();
            int TotalCount = 0;
            foreach (var item in ans)
            {
                TotalCount +=(int)item.Count;
            }
            ViewBag.TotalCount = TotalCount;
            ViewBag.question = db.Questions.Where(n => n.ID == id).SingleOrDefault();
            return View(ans);
        }
    }
}