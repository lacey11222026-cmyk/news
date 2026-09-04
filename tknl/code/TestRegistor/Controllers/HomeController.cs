using System;
using System.Collections.Generic;
using System.Linq;
using BIZ;
using DATA;
using System.Configuration;

using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BIZ.Entity;
using TestRegistor.Helper;
using TestRegistor.Models;
using UTILS;
using Newtonsoft.Json;
using DATA.DAL;

namespace TestRegistor.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /TestRegistor/
        public string GetAccountTest()
        {
            var strAccounttest = ConfigurationManager.AppSettings["AccountTest"] ?? "test";
            return strAccounttest.ToLower();
        }

        [OutputCache(Duration = 30)]
        public ActionResult TopTestRegister()
        {

            var listRegistor = new TestRegistorBO().GetTestRegistor();
            listRegistor = listRegistor.Where(x => x.Status == 1 && x.Type == 1).ToList();
            listRegistor.Reverse();
            return PartialView(listRegistor);
        }
        public ActionResult TestRegisterDetail(TestRegister_Full obj)
        {


            return PartialView(obj);
        }
        public ActionResult Index()
        {
            ViewBag.Page = "home";
            if (Request.Cookies["testnameinfo2024v2"] != null)
            {
                var testname = Utils.Base64Decode(Request.Cookies["testnameinfo2024v2"].Value.ToString());
                var testnamearr = testname.Split('_');
                Session["Mobile"] = testnamearr[0];
                Session["Fullname"] = testnamearr[1];
                Session["Location"] = testnamearr[2];
                Session["Role"] = testnamearr[3];
                //            Session["Location"] = testnamearr[2];
                //Session["Role"] = testnamearr[3];
                //Session["Note"] = testnamearr[4];
                if (String.IsNullOrEmpty(Session["Fullname"].ToString()))
                {
                    return RedirectToAction("LogOn");
                }

                if (Session["Test"] != null)
                {
                    //NLogLogger.DebugMessage("Home " +Session["Test"].ToString());
                    ViewBag.Test = Session["Test"].ToString();
                }
                return View();
            }
            return View();

        }
        public ActionResult ResultExam(int id)
        {
            if (Request.Cookies["testnameinfo2024v2"] != null)
            {
                var testname = Utils.Base64Decode(Request.Cookies["testnameinfo2024v2"].Value.ToString());
                var testnamearr = testname.Split('_');
                Session["Mobile"] = testnamearr[0];
                Session["Fullname"] = testnamearr[1];
                Session["Location"] = testnamearr[2];
                Session["Role"] = testnamearr[3];
                //if (Session["TestId"] == null)
                //    return RedirectToAction("Index");
                //int id = int.Parse(Session["TestId"].ToString());

                var listarchiveInfo = new TestArchiveBO().GetByMobile(id, Session["Mobile"].ToString());
                var registorInfo = new TestRegistorBO().GetFullById(id);
                if (registorInfo == null || registorInfo.Status == 0)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Name = registorInfo.Title;
                return View(listarchiveInfo);
            }
            else
            {
                return RedirectToAction("LogOn");
            }

        }
        [HttpPost]

        public ActionResult GotoArchive(int id)
        {
            var ReturnData = new ReturnData();
            try
            {
                Session["ArchiveId"] = id;
                return Json(ReturnData);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(ReturnData);
            }

        }
        [HttpPost]

        public ActionResult Start(int id)
        {
            var ReturnData = new ReturnData();
            try
            {
                if (Session["Mobile"] == null || Session["Fullname"] == null)
                {
                    Session["Test"] = id.ToString();
                    //NLogLogger.DebugMessage("Start "+Session["Test"].ToString());
                    ReturnData.ResponseCode = -101;
                    ReturnData.Description = "Chưa đăng nhập";
                    return Json(ReturnData);
                }
                if (String.IsNullOrEmpty(Session["Mobile"].ToString()))
                {
                    Session["Test"] = id.ToString();
                    //NLogLogger.DebugMessage("Start "+Session["Test"].ToString());
                    ReturnData.ResponseCode = -101;
                    ReturnData.Description = "Chưa đăng nhập";
                    return Json(ReturnData);
                }
                Session["Test"] = null;
                var isAccountTest = GetAccountTest().Contains(Session["Mobile"].ToString());
                var registorInfo = new TestRegistorBO().GetFullById(id);

                if (registorInfo == null)
                {
                    ReturnData.ResponseCode = -99;
                    ReturnData.Description = "Cuộc thi đã đóng";
                    return Json(ReturnData);
                }
                if (!isAccountTest)
                {
                    if (registorInfo.Status == 0)
                    {
                        ReturnData.ResponseCode = -99;
                        ReturnData.Description = "Cuộc thi đã đóng";
                        return Json(ReturnData);
                    }
                }
                if (registorInfo.EndTime <= DateTime.Now)
                {
                    ReturnData.ResponseCode = -99;
                    ReturnData.Description = "Cuộc thi đã đóng";
                    return Json(ReturnData);
                }
                TestArchive archiveInfo;
                var listarchiveInfo = new TestArchiveBO().GetByMobile(id, Session["Mobile"].ToString());

                //các trước hợp
                //cho chưa thi thì cho vào thi
                if (listarchiveInfo == null || listarchiveInfo.Count() < 5)
                {


                    if (listarchiveInfo != null && listarchiveInfo.Count() > 0)

                    {
                        archiveInfo = listarchiveInfo.FirstOrDefault();
                        if (archiveInfo.Status == 0)
                        {

                            //đang thi
                            if (DateTime.Now <=
                                archiveInfo.StartTime.GetValueOrDefault()
                                    .AddSeconds(registorInfo.TestTime.GetValueOrDefault()))
                            {
                                ReturnData.ResponseCode = archiveInfo.Id;
                                Session["ArchiveId"] = ReturnData.ResponseCode;
                                NLogLogger.DebugMessage(ReturnData.ResponseCode);
                                return Json(ReturnData);
                            }

                        }
                    }
                    archiveInfo = new TestArchive
                    {
                        FulName = Session["Fullname"].ToString(),
                        Mobile = Session["Mobile"].ToString(),
                        Location = Session["Location"].ToString(),
                        //Location = "",
                        Note = 0,
                        Role = Session["Role"].ToString(),
                        Status = 0,
                        RegistorId = id,
                        Questions = ""

                    };
                    //trộn đề
                    var listQuestion = new TestQuestionBO().GetByRegistorId(id);
                    if (listQuestion == null || listQuestion.Count < registorInfo.NumberQuestion.GetValueOrDefault())
                    {
                        ReturnData.ResponseCode = -99;
                        ReturnData.Description = "Lỗi đề thi, vui lòng quay lại sau";
                        return Json(ReturnData);
                    }
                    var questions = new List<TestQuestion_Full>();
                    if (listQuestion.Exists(x => x.Mark >= 2))
                    {
                        var random = new Random();
                        questions.AddRange(listQuestion.Where(x => x.Mark == 1).OrderBy(x => random.NextDouble())
                                 .Take(8)
                                 .ToList());
                        questions.AddRange(listQuestion.Where(x => x.Mark == 2).OrderBy(x => random.NextDouble())
                                .Take(2)
                                .ToList());
                        //questions.AddRange(listQuestion.Where(x => x.Mark == 3).OrderBy(x => random.NextDouble())
                        //       .Take(3)
                        //       .ToList());
                        //questions.AddRange(listQuestion.Where(x => x.Mark == 4).OrderBy(x => random.NextDouble())
                        //      .Take(4)
                        //      .ToList());
                        questions = questions.OrderBy(x => random.NextDouble()).ToList();
                    }
                    else
                    {
                        var random = new Random();

                        questions =
                           listQuestion.OrderBy(x => random.NextDouble())
                               .Take(registorInfo.NumberQuestion.GetValueOrDefault())
                               .ToList();

                    }
                    foreach (var itemq in questions)
                    {
                        archiveInfo.Questions += itemq.Id + "|";
                    }

                    ReturnData.ResponseCode = new TestArchiveBO().InsertUpdate(archiveInfo);
                    Session["ArchiveId"] = ReturnData.ResponseCode;
                    return Json(ReturnData);
                }
                else
                {
                    archiveInfo = listarchiveInfo.FirstOrDefault();
                    if (archiveInfo.Status == 0)
                    {

                        //đang thi
                        if (DateTime.Now <=
                            archiveInfo.StartTime.GetValueOrDefault()
                                .AddSeconds(registorInfo.TestTime.GetValueOrDefault()))
                        {
                            ReturnData.ResponseCode = archiveInfo.Id;
                            Session["ArchiveId"] = ReturnData.ResponseCode;
                            NLogLogger.DebugMessage(ReturnData.ResponseCode);
                            return Json(ReturnData);
                        }
                        else
                        {
                            ReturnData.ResponseCode = -99;
                            ReturnData.Description =
                                "Thời gian thi của bạn đã kết thúc, vui lòng liên hệ với quản trị để thi lại";
                            return Json(ReturnData);
                        }
                    }
                    else
                    {
                        ReturnData.ResponseCode = -102;
                        Session["TestId"] = id;
                        ReturnData.Description = "Bạn đã hoàn thành cuộc thi";
                        return Json(ReturnData);
                    }
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(ReturnData);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult End(string result)
        {
            var ReturnData = new ReturnData();
            try
            {


                if (Session["Mobile"] == null || Session["ArchiveId"] == null)
                {
                    ReturnData.ResponseCode = -101;
                    ReturnData.Description = "Chưa đăng nhập";
                    return Json(ReturnData);
                }

                var id = int.Parse(Session["ArchiveId"].ToString());
                var archiveInfo = new TestArchiveBO().GetById(id);
                if (archiveInfo == null || archiveInfo.Status == 1)
                {
                    ReturnData.ResponseCode = -101;
                    ReturnData.Description = "Chưa đăng nhập";
                    return Json(ReturnData);
                }
                archiveInfo.Archive = result;
                archiveInfo.Status = 1;
                archiveInfo.Mark = 0;
                var arrQuestion = archiveInfo.Questions.Split('|');
                var arrAnswer = archiveInfo.Archive.Split('|');
                var listQuestion = new TestQuestionBO().GetByRegistorId(archiveInfo.RegistorId.GetValueOrDefault());
                foreach (var itemq in arrQuestion.Select((value, i) => new { i, value }))
                {
                    if (!string.IsNullOrEmpty(itemq.value))
                    {
                        var question = listQuestion.FirstOrDefault(x => x.Id == int.Parse(itemq.value));
                        if (question != null)
                        {
                            if (question.Result == arrAnswer[itemq.i])
                                archiveInfo.Mark++;
                        }


                    }

                }
                var registorInfo = new TestRegistorBO().GetFullById(archiveInfo.RegistorId.GetValueOrDefault());
                if (registorInfo == null || registorInfo.Status == 0)
                {
                    return RedirectToAction("Index");
                }
                archiveInfo.EndTime = DateTime.Now;
                TimeSpan span = (archiveInfo.EndTime.GetValueOrDefault() - archiveInfo.StartTime.GetValueOrDefault());
                if (span.TotalSeconds > registorInfo.TestTime)
                {
                    //NLogLogger.DebugMessage(JsonConvert.SerializeObject(archiveInfo));
                    archiveInfo.EndTime = archiveInfo.StartTime.GetValueOrDefault().AddSeconds(registorInfo.TestTime.GetValueOrDefault());
                }
                //Random rnd = new Random();
                //var mintime= rnd.Next(8, 11);
                //if (span.TotalSeconds < mintime)
                //{
                //    NLogLogger.DebugMessage(JsonConvert.SerializeObject(archiveInfo));
                //    NLogLogger.DebugMessage(span.TotalSeconds.ToString());
                //    archiveInfo.EndTime = archiveInfo.StartTime.GetValueOrDefault().AddSeconds(mintime);
                //}
                new TestArchiveBO().InsertUpdate(archiveInfo);
                ReturnData.ResponseCode = 1;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
            }
            return Json(ReturnData);

        }


        public ActionResult Result()
        {
            if (Session["Mobile"] == null || Session["ArchiveId"] == null)
            {

                return RedirectToAction("Index");
            }
            var id = int.Parse(Session["ArchiveId"].ToString());
            var archiveInfo = new TestArchiveBO().GetById(id);
            if (archiveInfo == null || archiveInfo.Status == 0)
            {
                return RedirectToAction("Index");
            }
            var registorInfo = new TestRegistorBO().GetFullById(archiveInfo.RegistorId.GetValueOrDefault());
            if (registorInfo == null || registorInfo.Status == 0)
            {
                return RedirectToAction("Index");
            }
            var listQuestion = new TestQuestionBO().GetByRegistorId(archiveInfo.RegistorId.GetValueOrDefault());
            var arrQuestion = archiveInfo.Questions.Split('|');
            var arrAnswer = archiveInfo.Archive.Split('|');
            var model = new List<TestQuestionArchive>();
            foreach (var itemq in arrQuestion.Select((value, i) => new { i, value }))
            {
                if (!string.IsNullOrEmpty(itemq.value))
                {
                    var question = listQuestion.FirstOrDefault(x => x.Id == int.Parse(itemq.value));
                    if (question != null)
                    {
                        var itemBase = new TestQuestionArchive
                        {
                            Id = question.Id,
                            Contents = question.Contents,
                            Type = question.Type,
                            Status = 0,

                        };
                        var lstAnswer = new List<AnswerArchiveInfo>();
                        foreach (var itema in question.AnswersInfo.Select((value, i) => new { i, value }))
                        {
                            var itemBaseA = new AnswerArchiveInfo
                            {
                                Order = itema.value.Order,
                                Name = itema.value.Name,
                                IsCheck = itema.value.IsCheck,
                                IsUserCheck = arrAnswer[itemq.i].Contains("," + itema.value.Order + ",")
                            };
                            lstAnswer.Add(itemBaseA);
                        }
                        itemBase.AnswerArchiveInfo = lstAnswer;
                        if (question.Result == arrAnswer[itemq.i])
                        {
                            itemBase.Status = 1;
                        }
                        model.Add(itemBase);
                    }

                }
            }
            ViewBag.Note = archiveInfo.Note.GetValueOrDefault();
            ViewBag.Name = registorInfo.Title;
            ViewBag.Mark = archiveInfo.Mark;
            ViewBag.TotalMark = registorInfo.NumberQuestion;
            ViewBag.Time = archiveInfo.TestTime;
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ExamUpdate(int Note)
        {
            try
            {
                if (Session["Mobile"] == null || Session["ArchiveId"] == null)
                {
                    return Json(new { success = false, statusCode = -98, msg = "" });
                }
                var id = int.Parse(Session["ArchiveId"].ToString());
                var archiveInfo = new TestArchiveBO().GetById(id);
                if (archiveInfo == null )
                {
                    return Json(new { success = false, statusCode = -97, msg = "" });
                }
                archiveInfo.Note = Note;
                new TestArchiveBO().InsertUpdate(archiveInfo);
                return Json(new { success = true, statusCode = 1, msg = "" });
            }

            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { success = false, statusCode = -99, msg = "Hệ thống bận vui lòng quay lại sau" });
            }
        }
        public ActionResult Exam()
        {
            if (Session["Mobile"] == null || Session["ArchiveId"] == null)
            {
                return RedirectToAction("Index");
            }
            var id = int.Parse(Session["ArchiveId"].ToString());
            var archiveInfo = new TestArchiveBO().GetById(id);
            if (archiveInfo == null || archiveInfo.Status == 1)
            {
                return RedirectToAction("Index");
            }
            var registorInfo = new TestRegistorBO().GetFullById(archiveInfo.RegistorId.GetValueOrDefault());
            if (registorInfo == null || registorInfo.Status == 0)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Name = registorInfo.Title;
            ViewBag.NumberQuestion = registorInfo.NumberQuestion;
            if (DateTime.Now >
                           archiveInfo.StartTime.GetValueOrDefault()
                               .AddSeconds(registorInfo.TestTime.GetValueOrDefault()))
            {
                return RedirectToAction("Index");
            }



            var listQuestion = new TestQuestionBO().GetByRegistorId(archiveInfo.RegistorId.GetValueOrDefault());
            var arrQuestion = archiveInfo.Questions.Split('|');
            var model = new List<TestQuestionArchive>();
            foreach (var itemq in arrQuestion.Select((value, i) => new { i, value }))
            {
                if (!string.IsNullOrEmpty(itemq.value))
                {
                    var question = listQuestion.FirstOrDefault(x => x.Id == int.Parse(itemq.value));
                    var itemBase = new TestQuestionArchive
                    {
                        Id = question.Id,
                        Contents = question.Contents,
                        Type = question.Type,
                        Status = 1,
                        Mark = question.Mark,

                    };
                    var lstAnswer = new List<AnswerArchiveInfo>();
                    foreach (var itema in question.AnswersInfo.Select((value, i) => new { i, value }))
                    {
                        var itemBaseA = new AnswerArchiveInfo
                        {
                            Order = itema.value.Order,
                            Name = itema.value.Name,
                            IsCheck = itema.value.IsCheck,


                        };
                        lstAnswer.Add(itemBaseA);
                    }
                    itemBase.AnswerArchiveInfo = lstAnswer;
                    model.Add(itemBase);
                }
            }
            //set lại time thi
            if (archiveInfo.CreatedDate == archiveInfo.StartTime)
            {
                archiveInfo.StartTime = DateTime.Now;
                new TestArchiveBO().UpdateContentCache(archiveInfo);
            }
            TimeSpan span = (archiveInfo.StartTime.GetValueOrDefault().AddSeconds(registorInfo.TestTime.GetValueOrDefault()) - DateTime.Now);
            ViewBag.Time = archiveInfo.StartTime.GetValueOrDefault().AddSeconds(registorInfo.TestTime.GetValueOrDefault());
            ViewBag.m = span.Minutes + span.Hours * 60;
            ViewBag.s = span.Seconds;
            return View(model);
        }

        public ActionResult LoadExam(int index)
        {
            ViewBag.Index = index;
            if (Session["Mobile"] == null || Session["ArchiveId"] == null)
            {
                return PartialView(null);
            }
            var id = int.Parse(Session["ArchiveId"].ToString());
            var archiveInfo = new TestArchiveBO().GetById(id);
            if (archiveInfo == null || archiveInfo.Status == 1)
            {
                return PartialView(null);
            }
            var registorInfo = new TestRegistorBO().GetFullById(archiveInfo.RegistorId.GetValueOrDefault());
            if (registorInfo == null || registorInfo.Status == 0)
            {
                return PartialView(null);
            }
            //NLogLogger.DebugMessage(Session["Mobile"] + "-" + index.ToString() + "-" + DateTime.Now.ToString("HH:mm:ss.fff"));
            ViewBag.Name = registorInfo.Title;
            ViewBag.NumberQuestion = registorInfo.NumberQuestion;
            if (DateTime.Now >
                           archiveInfo.StartTime.GetValueOrDefault()
                               .AddSeconds(registorInfo.TestTime.GetValueOrDefault()))
            {
                return PartialView(null);
            }



            var listQuestion = new TestQuestionBO().GetByRegistorId(archiveInfo.RegistorId.GetValueOrDefault());
            var arrQuestion = archiveInfo.Questions.Split('|');
            var model = new List<TestQuestionArchive>();
            foreach (var itemq in arrQuestion.Select((value, i) => new { i, value }))
            {
                if (!string.IsNullOrEmpty(itemq.value))
                {
                    var question = listQuestion.FirstOrDefault(x => x.Id == int.Parse(itemq.value));
                    var itemBase = new TestQuestionArchive
                    {
                        Id = question.Id,
                        Contents = question.Contents,
                        Type = question.Type,
                        Status = 1,
                        Mark = question.Mark,
                    };
                    var lstAnswer = new List<AnswerArchiveInfo>();
                    foreach (var itema in question.AnswersInfo.Select((value, i) => new { i, value }))
                    {
                        var itemBaseA = new AnswerArchiveInfo
                        {
                            Order = itema.value.Order,
                            Name = itema.value.Name,
                            IsCheck = itema.value.IsCheck,

                        };
                        lstAnswer.Add(itemBaseA);
                    }
                    itemBase.AnswerArchiveInfo = lstAnswer;
                    model.Add(itemBase);
                }
            }
            //set lại time thi
            if (archiveInfo.CreatedDate == archiveInfo.StartTime)
            {
                archiveInfo.StartTime = DateTime.Now;
                new TestArchiveBO().UpdateContentCache(archiveInfo);
            }
            TimeSpan span = (archiveInfo.StartTime.GetValueOrDefault().AddSeconds(registorInfo.TestTime.GetValueOrDefault()) - DateTime.Now);
            ViewBag.Time = archiveInfo.StartTime.GetValueOrDefault().AddSeconds(registorInfo.TestTime.GetValueOrDefault());
            ViewBag.m = span.Minutes + span.Hours * 60;
            ViewBag.s = span.Seconds;
            return PartialView(model);
        }
        public ActionResult Logout()
        {
            Session.RemoveAll();
            Session.Abandon();
            Session["Mobile"] = "";
            Session["Fullname"] = "";
            foreach (string cookie in HttpContext.Request.Cookies.AllKeys)
            {

                if (!cookie.ToLower().Contains("testnameinfo2024v2")) continue;

                //Logger.Info(string.Format("[Logout][RemoveCookie] name: {0}", cookie));

                HttpContext.Response.Cookies.Set(new HttpCookie(cookie) { Expires = DateTime.Now.AddMonths(-1), Path = "/" });
                //HttpContext.Current.Response.Cookies.Add(new HttpCookie(cookie) { Expires = DateTime.Now.AddMonths(-1), Path = "/" });
            }
            return RedirectToAction("Index");
        }
        public ActionResult LogOn()
        {

            Session["Mobile"] = null;
            Session["Fullname"] = null;
            List<TestLocation> allCache = new TestLocationBO().GetAllCache();

            //ViewBag.Mobile = Session["Mobile"]?.ToString() ?? "";
            //ViewBag.FulName = "";
            //ViewBag.Location = "";
            //ViewBag.Role = "";
            //ViewBag.Note = "";
            //if (Session["Mobile"] != null)
            //{
            //    var data = new TestArchiveBO().GetByMobile(-1, Session["Mobile"].ToString());
            //    if (data != null)
            //    {

            //        if (data.Count >= 1)
            //        {
            //            ViewBag.FulName = data.LastOrDefault().FulName;

            //        }

            //    }
            //}
            //var listLocation = new TestLocationBO().GetAllCache();
            //listRegistor = listRegistor.Where(x => x.Status == 1).ToList();

            return View(allCache);
            //return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult LogOn(string Mobile, string FullName, string Location, string Role)
        {
            try
            {
                if (string.IsNullOrEmpty(Mobile))
                    return Json(new { success = false, statusCode = -100, msg = "Dữ liệu không được bỏ trống" });
                if (Utils.GetTelCo(Mobile) <= 0)
                {
                    return Json(new { success = false, statusCode = -100, msg = "Số điện thoại không hợp lệ" });
                }

                if (string.IsNullOrEmpty(FullName))
                {
                    //Session["Mobile"] = Mobile;
                    return Json(new { success = false, statusCode = -1, msg = "Họ tên không được bỏ trống" });
                }
                if (FullName.Length < 6)
                {
                    //Session["Mobile"] = Mobile;
                    return Json(new { success = false, statusCode = -1, msg = "Họ tên không hợp lệ" });
                }
                Session["Mobile"] = Mobile;
                Session["Fullname"] = FullName;
                Session["Location"] = Location;
                Session["Role"] = Role;
                var testname = Utils.Base64Encode($"{Mobile}_{FullName}_{Location}_{Role}");
                HttpCookie ck = new HttpCookie("testnameinfo2024v2", testname) { HttpOnly = true, Path = "/" };
                ck.Expires = DateTime.Now.AddMinutes(144000);
                HttpContext.Request.Cookies.Add(ck);
                HttpContext.Response.Cookies.Add(ck);
                return Json(new { success = true, statusCode = 1, msg = "Tên đăng nhập hoặc mật khẩu không đúng" });
            }

            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { success = false, statusCode = -99, msg = "Hệ thống bận vui lòng quay lại sau" });
            }
        }

        public ActionResult Intro()
        {
            //var intro = new ContentBO().GetContentFull(Id);
            if (Request.Cookies["testnameinfo2024v2"] != null)
            {
                var testname = Utils.Base64Decode(Request.Cookies["testnameinfo2024v2"].Value.ToString());
                var testnamearr = testname.Split('_');
                Session["Mobile"] = testnamearr[0];
                Session["Fullname"] = testnamearr[1];
                //            Session["Location"] = testnamearr[2];
                //Session["Role"] = testnamearr[3];
                //Session["Note"] = testnamearr[4];
                if (String.IsNullOrEmpty(Session["Fullname"].ToString()))
                {
                    return RedirectToAction("LogOn");
                }
                ViewBag.Test = "";
                if (Session["Test"] != null)
                {
                    //NLogLogger.DebugMessage("Home " +Session["Test"].ToString());
                    ViewBag.Test = Session["Test"].ToString();
                }
                return View();
            }
            else
            {
                return RedirectToAction("LogOn");
                //return View();
            }

        }
        [OutputCache(Duration = 60, VaryByParam = "*")]
        public ActionResult LastestNews(int CategoryId, int MaxLastestNews)
        {

            var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);

            return PartialView(lstdata);
        }
    }

}