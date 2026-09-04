using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BIZ;
using Constants = UTILS.Constants;
using BIZ.Entity;
using System.Web.Routing;
using DATA;
using Newtonsoft.Json;
using WebMVC4.Models;
using UTILS;
using DATA.SMS;
using TestArchiveTeam = DATA.SMS.TestArchiveTeam;

namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,Competition")]
    public class AdminTestController : Controller
    {
        //
        // GET: /AdminTest/


        #region"registor"
        public ActionResult TestRegistor()
        {

            return View();
        }
        public ActionResult ListTestRegistor(string keyword, int? status, int? currentPage, int? pageSize)
        {

            int Status = status == null ? -1 : (int)status;
            int TotalRecord = 0;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 20 : (int)pageSize;

            var data = new TestRegistorBO().GetAll(keyword, Status, CurrPage, RecordPerPage, ref TotalRecord);
            if (data.Count > 0)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;

            return PartialView(data);
        }
        public ActionResult TestRegistorDetail(int Id = 0)
        {
            var model = new TestRegistor { Id = 0, StartTime = DateTime.Now, EndTime = DateTime.Now, Status = 1, NumberQuestion = 10, TestTime = 10 };
            if (Id > 0)
            {
                model = new TestRegistorBO().GetById(Id);
                if (model == null)
                    return RedirectToAction("TestRegistor");
                ViewBag.Title = "Cập nhật đợt thi";
            }
            else
            {
                ViewBag.Title = "Thêm mới đợt thi";
            }
            return View(model);
        }
        public ActionResult TestRegistorResult(int registorId = 0)
        {
            var listQuestion = new TestQuestionBO().GetByRegistorId(registorId);
            listQuestion.Reverse();
            return View(listQuestion);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult TestRegistorSaveData(TestRegistor obj, string sStartTime, string sEndTime)
        {
            var ReturnData = new ReturnData();
            try
            {
                IFormatProvider culture = new CultureInfo("en-US", true);
                obj.StartTime = DateTime.ParseExact(sStartTime, "dd/MM/yyyy HH:mm", culture);
                obj.EndTime = DateTime.ParseExact(sEndTime, "dd/MM/yyyy HH:mm", culture);
                var result = new TestRegistorBO().InsertUpdate(obj);
                ReturnData.ResponseCode = result;
                if (result >= 0)
                {

                    var lognewsobj = new ContentLog
                    {
                        UserName = HttpContext.User.Identity.Name,
                        ItemtType = (int)Constants.CategoryType.TestRegistor,
                        ItemId = obj.Id,
                        ItemName = obj.Title,
                        Note = "Xóa đợt thi",
                        Type = 1

                    };
                    if (obj.Id > 0)
                    {
                        ReturnData.Description = "Cập nhật Thành Công";
                        lognewsobj.Note = "Update đợt thi";
                    }

                    else
                    {
                        ReturnData.Description = "Thêm mới Thành Công";
                        lognewsobj.Note = "Tạo mới đợt thi";
                    }

                    //Ghi log
                    Action<ContentLog> send = InsertContentLog;
                    var asynSend = send.BeginInvoke(lognewsobj, null, null);
                }
                else switch (result)
                    {
                        case -51: ReturnData.Description = "Đã có bài viết này"; break;
                        case -600: ReturnData.Description = "Tham số truyền vào không hợp lệ"; break;
                        default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                    }
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
        [ValidateAntiForgeryToken]
        public ActionResult TestRegistorUpdateStatus(string _id, string Title)
        {
            int Id = int.Parse(Utils.Base64Decode(_id));
            var ReturnData = new ReturnData();
            try
            {
                if (Id > 0)
                {
                    var obj = new TestRegistorBO().GetById(Id);
                    if (obj != null)
                    {
                        var lognewsobj = new ContentLog
                        {
                            UserName = HttpContext.User.Identity.Name,
                            ItemtType = (int)Constants.CategoryType.TestRegistor,
                            ItemId = Id,
                            ItemName = Title,
                            Note = "Duyệt đợt thi",
                            Type = 1

                        };
                        if (obj.Status == 0)
                        {
                            obj.Status = 1;
                        }
                        else
                        {
                            obj.Status = 0;
                            lognewsobj.Note = "Khóa đợt thi";
                        }
                        new TestRegistorBO().InsertUpdate(obj);
                        //Ghi log
                        Action<ContentLog> send = InsertContentLog;
                        var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Cập nhật đợt thi Thành Công";
                    }
                    else
                    {
                        ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                    }
                    return Json(ReturnData);
                }
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Không xác định văn bản cần thao tác";
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
        [ValidateAntiForgeryToken]
        public ActionResult TestRegistorDelete(string _id, string Title)
        {
            int Id = int.Parse(Utils.Base64Decode(_id));
            var ReturnData = new ReturnData();
            try
            {
                if (Id > 0)
                {
                    var obj = new TestRegistorBO().GetById(Id);
                    if (obj != null)
                    {
                        var lognewsobj = new ContentLog
                        {
                            UserName = HttpContext.User.Identity.Name,
                            ItemtType = (int)Constants.CategoryType.TestRegistor,
                            ItemId = Id,
                            ItemName = Title,
                            Note = "Xóa đợt thi",
                            Type = 1

                        };
                        if (obj.Status == 1)
                        {
                            ReturnData.Description = "Không được phép xóa đợt thi đang diễn ra";
                            return Json(ReturnData);
                        }
                        else
                        {
                            new TestRegistorBO().Delete(obj.Id);
                            //Ghi log
                            Action<ContentLog> send = InsertContentLog;
                            var asynSend = send.BeginInvoke(lognewsobj, null, null);

                            ReturnData.Description = "Xóa đợt thi Thành Công";
                        }

                    }
                    else
                    {
                        ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                    }
                    return Json(ReturnData);
                }
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Không xác định văn bản cần thao tác";
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
        #endregion
        #region "Question"
        public ActionResult TestQuestion(int registorId = -1)
        {
            ViewBag.registorId = registorId;

            var lstCate = new TestRegistorBO().GetAll();

            lstCate.Insert(0, new TestRegistor { Id = -1, Title = "--Đợt thi--" });
            ViewBag.RegistorList = lstCate;
            return View();
        }
        public ActionResult ListTestQuestion(int registorId, int? status, int? currentPage, int? pageSize)
        {

            int Status = status == null ? -1 : (int)status;
            int TotalRecord = 0;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 50 : (int)pageSize;

            var data = new TestQuestionBO().GetByRegistorId(registorId, Status, CurrPage, RecordPerPage, ref TotalRecord);
            data.Reverse();
            if (data.Count > 0)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;

            return PartialView(data);
        }

        public ActionResult TestQuestionDetail(int Id = 0, int registorId = -1)
        {
            var model = new TestQuestion { Id = 0, Status = 1, Type = 1, Mark = 1 };
            if (Id > 0)
            {
                model = new TestQuestionBO().GetById(Id);

                if (model == null)
                    return RedirectToAction("TestQuestion");
                ViewBag.Title = "Cập nhật câu hỏi";
                ViewBag.registorId = model.RegistorId;
            }
            else
            {
                ViewBag.Title = "Thêm mới câu hỏi";
                ViewBag.registorId = registorId;
            }
            var lstCate = new TestRegistorBO().GetAll();
            lstCate.Insert(0, new TestRegistor { Id = -1, Title = "--Đợt thi--" });
            ViewBag.RegistorList = lstCate;

            var data = new TestQuestion_Full()
            {
                Id = model.Id,
                Title = model.Title,
                RegistorId = model.RegistorId,
                Mark = model.Mark,
                Explain = model.Explain,
                Answers = model.Answers,
                Type = model.Type,
                Contents = model.Contents,
                Status = model.Status,

            };
            data.AnswersInfo = new List<AnswerInfo>();
            if (!string.IsNullOrEmpty(data.Answers))
            {
                data.AnswersInfo = JsonConvert.DeserializeObject<List<AnswerInfo>>(data.Answers);
            }

            return View(data);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult TestQuestionSaveData(TestQuestion obj)
        {
            var ReturnData = new ReturnData();
            try
            {

                var result = new TestQuestionBO().InsertUpdate(obj);
                ReturnData.ResponseCode = result;
                if (result >= 0)
                {

                    var lognewsobj = new ContentLog
                    {
                        UserName = HttpContext.User.Identity.Name,
                        ItemtType = (int)Constants.CategoryType.TestQuestion,
                        ItemId = obj.Id,
                        ItemName = obj.Title,
                        Note = "Xóa đợt thi",
                        Type = 1

                    };
                    if (obj.Id > 0)
                    {
                        ReturnData.Description = "Cập nhật Thành Công";
                        lognewsobj.Note = "Update câu hỏi";
                    }

                    else
                    {
                        ReturnData.Description = "Thêm mới Thành Công";
                        lognewsobj.Note = "Tạo mới câu hỏi";
                    }

                    //Ghi log
                    Action<ContentLog> send = InsertContentLog;
                    var asynSend = send.BeginInvoke(lognewsobj, null, null);
                }
                else switch (result)
                    {
                        case -51: ReturnData.Description = "Đã có bài viết này"; break;
                        case -600: ReturnData.Description = "Tham số truyền vào không hợp lệ"; break;
                        default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                    }
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
        [ValidateAntiForgeryToken]
        public ActionResult TestQuestionUpdateStatus(string _id, string Title)
        {
            int Id = int.Parse(Utils.Base64Decode(_id));
            var ReturnData = new ReturnData();
            try
            {
                if (Id > 0)
                {
                    var obj = new TestQuestionBO().GetById(Id);
                    if (obj != null)
                    {
                        var lognewsobj = new ContentLog
                        {
                            UserName = HttpContext.User.Identity.Name,
                            ItemtType = (int)Constants.CategoryType.TestQuestion,
                            ItemId = Id,
                            ItemName = Title,
                            Note = "Duyệt câu hỏi",
                            Type = 1

                        };
                        if (obj.Status == 0)
                        {
                            obj.Status = 1;
                        }
                        else
                        {
                            obj.Status = 0;
                            lognewsobj.Note = "Khóa câu hỏi";
                        }
                        new TestQuestionBO().InsertUpdate(obj);
                        //Ghi log
                        Action<ContentLog> send = InsertContentLog;
                        var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Cập nhật câu hỏi Thành Công";
                    }
                    else
                    {
                        ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                    }
                    return Json(ReturnData);
                }
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Không xác định văn bản cần thao tác";
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

        #endregion

        #region "Archive"
        public ActionResult TopTestArchive()
        {
            var data = new TestArchiveBO().SelecTop();


            return PartialView(data);
        }
        public ActionResult TestArchive(int registorId = -1)
        {
            ViewBag.registorId = registorId;

            var lstCate = new TestRegistorBO().GetAll();
            lstCate.Insert(0, new TestRegistor { Id = -1, Title = "--Tất cả--" });
            ViewBag.RegistorList = lstCate;
            return View();
        }
        public ActionResult TestArchiveReport(int registorId = -1)
        {
            ViewBag.registorId = registorId;

            var lstCate = new TestRegistorBO().GetAll();
            lstCate.Insert(0, new TestRegistor { Id = -1, Title = "--Tất cả--" });
            ViewBag.RegistorList = lstCate;
            var data = new TestArchiveBO().Report(registorId);
            if (data != null)
            {
                int total = 0;
                int totalMobile = 0;
                var dataLine = string.Empty;
                var dataLine2 = string.Empty;
                var datacategory = string.Empty;
                foreach (var item in data)
                {
                    total += int.Parse(item.CountTest);
                    totalMobile += int.Parse(item.CountMobile);
                    datacategory += String.Format("'{0}',", item.DateTest);
                    dataLine += String.Format("{0},", item.CountTest);
                    dataLine2 += String.Format("{0},", item.CountMobile);
                }
                ViewBag.DataLine = String.Format("[{0}]", dataLine);
                ViewBag.DataLine2 = String.Format("[{0}]", dataLine2);
                ViewBag.datacategory = String.Format("[{0}]", datacategory);
                ViewBag.total = total;
                ViewBag.totalMobile = totalMobile;
            }

            return View(data);
        }
        public ActionResult ListTestArchiveReport(int registorId)
        {


            var data = new TestArchiveBO().Report(registorId);


            return PartialView(data);
        }
        public ActionResult ListTestArchive(string keyword, int registorId, int? orderType, int? currentPage, int? pageSize)
        {

            int TotalRecord = 0;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 30 : (int)pageSize;
            int OrderType = orderType == null ? 0 : (int)orderType;
            var data = new TestArchiveBO().GetByRegistorId(registorId, keyword, CurrPage, RecordPerPage, ref TotalRecord, OrderType);
            if (data.Count > 0)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;

            return PartialView(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TestArchiveDelete(string _id, string Title)
        {
            int Id = int.Parse(Utils.Base64Decode(_id));
            var ReturnData = new ReturnData();
            try
            {
                if (Id > 0)
                {
                    var lognewsobj = new ContentLog
                    {
                        UserName = HttpContext.User.Identity.Name,
                        ItemtType = (int)Constants.CategoryType.TestRegistor,
                        ItemId = Id,
                        ItemName = Title,
                        Note = "Xóa bài thi",
                        Type = 1

                    };
                    new TestArchiveBO().Delete(Id);
                    //Ghi log
                    Action<ContentLog> send = InsertContentLog;
                    var asynSend = send.BeginInvoke(lognewsobj, null, null);

                    ReturnData.Description = "Xóa bài thi Thành Công";
                    return Json(ReturnData);
                }
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Không xác định văn bản cần thao tác";
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
        public ActionResult PopupArchive(int id)
        {
            var obj = new TestArchiveBO().GetById(id);
            if (obj == null)
                return PartialView(null);
            if (obj.Status != 1)
                return PartialView(null);
            ViewBag.Name = obj.Mobile + " - " + obj.FulName;
            var listQuestion = new TestQuestionBO().GetByRegistorId(obj.RegistorId.GetValueOrDefault());

            var model = new List<TestQuestionArchive>();
            var arrQuestion = obj.Questions.Split('|');
            var arrAnswer = obj.Archive.Split('|');
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
                            Status = 0

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
            return PartialView(model);
        }
        #endregion

        #region "TeamArchive"
        public ActionResult Team(int registorId=-1)
        {
            var lstCate = new TestRegistorBO().GetAll().Where(x => x.Type == 2).ToList();
            lstCate.Insert(0, new TestRegistor { Id = -1, Title = "--Đợt thi--" });
            ViewBag.RegistorList = lstCate;
            ViewBag.registorId = registorId;
            return View();
        }
        public ActionResult ListTeam(int registorId = -1)
        {
            var data = new TestArchiveTeamDAL().GetList(registorId);
            
            return PartialView(data);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult TeamAdd(TestArchiveTeam doc)
        {
            var ReturnData = new ReturnData();
            try
            {
                var listQuestion = new TestQuestionBO().GetByRegistorId(doc.RegistorId);
                listQuestion.Reverse();
                foreach (var itemq in listQuestion)
                {
                    doc.Questions += itemq.Id + "|";
                }
                doc.Archive = ",A,|,A,|,A,|,A,|,A,|";
                var result = new TestArchiveTeamDAL().InsertUpdate(doc);
                ReturnData.ResponseCode = result;
                if (result >= 0)
                {
                    ReturnData.Description = "Cập nhật Thành Công";
                }
                else switch (result)
                    {
                        case -51: ReturnData.Description = "Đã có bài viết này"; break;
                        case -600: ReturnData.Description = "Tham số truyền vào không hợp lệ"; break;
                        default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                    }
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
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult TeamUpdate(TestArchiveTeam doc)
        {
            var ReturnData = new ReturnData();
            try
            {
                var newobj = new TestArchiveTeamDAL().Get(doc.Id);
                newobj.Archive = doc.Archive;

                var arrQuestion = newobj.Questions.Split('|');
                var arrAnswer = newobj.Archive.Split('|');
                var listQuestion = new TestQuestionBO().GetByRegistorId(newobj.RegistorId);
                foreach (var itemq in arrQuestion.Select((value, i) => new { i, value }))
                {
                    if (!string.IsNullOrEmpty(itemq.value))
                    {
                        var question = listQuestion.FirstOrDefault(x => x.Id == int.Parse(itemq.value));
                        if (question != null)
                        {
                            question.Result.Replace('1', 'A').Replace('2', 'B').Replace('3', 'C').Replace('4', 'D');
                            if (question.Result == arrAnswer[itemq.i])
                                newobj.Mark++;
                        }
                    }
                }

                var result = new TestArchiveTeamDAL().InsertUpdate(newobj);
                ReturnData.ResponseCode = result;
                if (result >= 0)
                {
                    ReturnData.Description = "Cập nhật Thành Công";
                }
                else switch (result)
                    {
                        case -51: ReturnData.Description = "Đã có bài viết này"; break;
                        case -600: ReturnData.Description = "Tham số truyền vào không hợp lệ"; break;
                        default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                    }
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
        public ActionResult PopupArchiveTeam(int id)
        {
            var obj = new TestArchiveTeamDAL().Get(id);
            if (obj == null)
                return PartialView(null);
            
            ViewBag.Name = obj.Name;
            var listQuestion = new TestQuestionBO().GetByRegistorId(obj.RegistorId);

            var model = new List<TestQuestionArchive>();
            var arrQuestion = obj.Questions.Split('|');
            var arrAnswer = obj.Archive.Replace('1', 'A').Replace('2', 'B').Replace('3', 'C').Replace('4', 'D').Split('|');
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
                            Status = 0

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
            return PartialView(model);
        }
        #endregion
        private void InsertContentLog(ContentLog lognewsobj)
        {
            new ContentLogBO().CreateUpdateContentLog(lognewsobj);
        }
    }
}
