using BIZ;
using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Models;

namespace CMS.Controllers
{
    [Authorize(Roles = "Administrator,Comment")]
    public class AdminCommentController : Controller
    {
        //
        // GET: /AdminComment/

        public ActionResult Index(string keyword = "", int page = 1, int status = -1)
        {
            int total = 0;
            var pageSize=20;
            var lstcomment = new CommentBO().GetCommentsPaged(keyword, -1, -1, status, page, pageSize, ref  total);
            var model = new CommentModel
            {
                listdata = lstcomment,
                pageIndex = page,
                pageSize = pageSize,
                total = total

            };
            ViewBag.keyword = keyword;
            ViewBag.Title = "Duyệt bình luận";
            ViewBag.Status = status;
            ViewBag.StatusList = new List<EnumInfo> { new EnumInfo { Value = -1, Text = "--Tất cả--" }, new EnumInfo { Value = 1, Text = "Duyệt" }, new EnumInfo { Value = 0, Text = "Chưa duyệt" } };
            return View(model);
        }
        [HttpPost]
        public ActionResult Appproved(long Id, int Status)
        {
            string results;
            //var comment = new CommentBO().GetComment(Id);
            //comment.Published = byte.Parse(Status.ToString());

            new CommentBO().UpdateComments(Id,Status);
            results = "true";
            return Json(results);

        }
        [HttpPost]
        public ActionResult UpdateContent(long Id, string Contents)
        {
            string results;
            var comment = new CommentBO().GetComment(Id);
            comment.Message = Contents;

            new CommentBO().CreateUpdateComment(comment);
            results = "true";
            return Json(results);

        }
        [HttpPost]
        public ActionResult Delete(long Id)
        {
            string results;


            new CommentBO().DeleteComment(Id);
            results = "true";
            return Json(results);

        }
    }
}
