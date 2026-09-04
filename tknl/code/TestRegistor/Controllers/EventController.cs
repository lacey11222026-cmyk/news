using BIZ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestRegistor.Controllers
{
    public class EventController : Controller
    {
        // GET: Event
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult TopTestRegister()
        {

            var listRegistor = new TestRegistorBO().GetTestRegistor();
            listRegistor = listRegistor.Where(x => x.Status == 1 && x.Type == 2).ToList();
            listRegistor.Reverse();
            return View(listRegistor);
        }
        public ActionResult Question(int Id,int Index=1)
        {

            var listQuestion = new TestQuestionBO().GetByRegistorId(Id);
            listQuestion.Reverse();
            ViewBag.Index = Index;
            ViewBag.Id = Id;
            var data = listQuestion[Index - 1];
            
            return View(data);
        }
       
        public ActionResult QuestionCorrect(int Id, int Index = 1)
        {

            var listQuestion = new TestQuestionBO().GetByRegistorId(Id);
            listQuestion.Reverse();
            ViewBag.Index = Index;
            ViewBag.Id = Id;
            var data = listQuestion[Index - 1];

            return View(data);
        }
    }
}