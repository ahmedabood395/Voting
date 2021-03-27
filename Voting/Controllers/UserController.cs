using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Voting.Models;

namespace Voting.Controllers
{
    public class UserController : Controller
    {
        DBContext db = new DBContext();
        // GET: User
        public ActionResult Index()
        {

            Question q=db.Questions.ToList().LastOrDefault();
            ViewBag.question= db.Questions.ToList().LastOrDefault();
            List<Answer> ans = db.Answers.Where(n => n.QuestionId == q.ID).ToList();

            return View(ans);
        }
        [HttpPost]
        public ActionResult Index(List<Answer> ans, int id)
        {

            if (Request.Cookies["answer"] != null)
            {

                string x = Request.Cookies["answer"].Values["id"];
                

                if (!x.Contains(id.ToString()))
                {

                    foreach (var item in ans)
                    {
                        if (item.ID != 0)
                        {
                            Answer answer = db.Answers.Where(n => n.ID == item.ID).SingleOrDefault();
                            answer.Count++;
                            db.SaveChanges();

                            Request.Cookies["answer"].Values.Add("id", answer.QuestionId.ToString());
                            Request.Cookies["answer"].Values.Add("name", answer.AnswerTitle);
                            Response.Cookies.Add(Request.Cookies["answer"]);
                        }
                        
                    }
                   Session["Error"] = "You Voted This Question";
                    return RedirectToAction("Index");
                }
                else
                {
                    Session["Error"] = "You Voted This Question"; 
                    return RedirectToAction("Index");
                }
            }
            else
            {
                foreach (var item in ans)
                {
                    if (item.ID != 0)
                    {
                        Answer answer = db.Answers.Where(n => n.ID == item.ID).SingleOrDefault();
                        answer.Count++;
                        db.SaveChanges();
                        HttpCookie co = new HttpCookie("answer");
                        co.Values.Add("id", answer.QuestionId.ToString());
                        co.Values.Add("name", answer.AnswerTitle);
                        co.Expires = DateTime.Now.AddDays(15);
                        Response.Cookies.Add(co);
                    }
                   
                }
            }
          
            Session["Error"] = "You Voted This Question";
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            string x = Request.Cookies["answer"].Values["name"];

            List<Answer> answer = db.Answers.Where(n => n.QuestionId == id).ToList();

            foreach (var item in answer)
            {
                if(x.Contains(item.AnswerTitle))
                {
                    Answer ans = db.Answers.Where(n => n.ID == item.ID).SingleOrDefault();
                    ans.Count--;
                    db.SaveChanges();
                }
            }

            Request.Cookies["answer"].Expires = DateTime.Now.AddDays(-30);
            Response.Cookies.Add(Request.Cookies["answer"]);

            return RedirectToAction("Index");
        }
    }
}