using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BIZ;
using BIZ.Entity;
using DATA;
using UTILS;

namespace CMS.Post
{
    /// <summary>
    /// Summary description for CommentService
    /// </summary>
    public class CommentService : IHttpHandler, IReadOnlySessionState
    {


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            ResponseMsg responseMsg = new ResponseMsg();
            CommentBO CommentBo = new CommentBO();
            try
            {
                string method = context.Request["__m"];
                int return_val;

                switch (method.ToLower())
                {
                    case "save":

                        var CommentId = context.Request.Form["id"];
                        var message = HttpUtility.UrlDecode(context.Request.Form["message"]);

                        var itemId = context.Request.Form["itemId"];
                        var type = context.Request.Form["type"];

                        var itemname = HttpUtility.UrlDecode(context.Request.Form["itemname"]);
                        var mail = HttpUtility.UrlDecode(context.Request.Form["mail"]);
                        var username = HttpUtility.UrlDecode(context.Request.Form["username"]);

                        var published = context.Request.Form["published"];
                       
                        if (!Utils.IsNumber(CommentId) || !Utils.IsNumber(published))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình lấy thông tin";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }
                        if (!HttpContext.Current.User.IsInRole("Administrator") && !HttpContext.Current.User.IsInRole("Comment") && (published != "0"))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Không có quyền";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }
                        var Comment = new Comment();

                        if (Convert.ToInt32(CommentId) > 0)
                        {
                            Comment = CommentBo.GetComment(Convert.ToInt32(CommentId));
                        }
                        else
                        {
                            Comment.CreatedTime = DateTime.Now;

                            Comment.ItemName = itemname;
                            Comment.Email = mail;
                            Comment.Type = Convert.ToInt32(type);
                            if (Convert.ToInt64(itemId) != 0)
                            {
                                Comment.ItemId = Convert.ToInt64(itemId);

                            }
                        }
                        Comment.UserName = username;
                        Comment.Id = Convert.ToInt32(CommentId);
                        Comment.Message = message;

                        //Comment.Published = Convert.ToByte(published);
                        Comment.Published = 0;
                        if (HttpContext.Current.Session["Comment"] == null)
                        {
                            HttpContext.Current.Session["Comment"] = "1";
                        }
                        var countsession = Convert.ToInt32(HttpContext.Current.Session["Comment"].ToString());
                        HttpContext.Current.Session["Comment"] = (countsession + 1).ToString();
                        if (countsession <= 20)
                        {
                            return_val = CommentBo.CreateUpdateComment(Comment);


                        }
                        else
                        {
                            return_val = 1;
                        }
                        if (return_val != -1)
                        {



                            responseMsg.Success = true;
                            responseMsg.Value = Convert.ToString(return_val);

                            responseMsg.Text = "Cảm ơn bạn đã gửi bình luận đến Báo Giao Thông. Nội dung có thể được biên tập trước khi xuất bản. ";
                            if (Comment.Id > 0)
                                responseMsg.Text = "Lưu thông tin thành công";
                        }
                        else
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình xử lý";
                        }

                        context.Response.Write(responseMsg.ToJsonString());
                        return;

                    case "delete":
                        if (!HttpContext.Current.User.IsInRole("Administrator") && !HttpContext.Current.User.IsInRole("Comment"))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Không có quyền";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }
                        var arrCommentId = context.Request.Form.GetValues("id[]");
                        if (arrCommentId == null || arrCommentId.Count() == 0)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Bạn phải chọn ít nhất 1 bình luận để xóa";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        if (arrCommentId.Count() == 1)
                        {
                            return_val = CommentBo.DeleteComment(Convert.ToInt32(arrCommentId[0]));

                        }
                        else
                        {
                            string joinId = string.Empty;
                            foreach (var id in arrCommentId)
                            {
                                if (Utils.IsNumber(id))
                                    joinId += "," + id;
                            }

                            if (string.IsNullOrEmpty(joinId))
                            {
                                responseMsg.Success = false;
                                responseMsg.Text = "Bạn phải chọn ít nhất 1 Comment để xóa";
                                context.Response.Write(responseMsg.ToJsonString());
                                return;
                            }

                            joinId = joinId.TrimStart(',');
                            return_val = CommentBo.DeleteComments(joinId);

                        }

                        if (return_val != -1)
                        {
                            responseMsg.Success = true;
                            responseMsg.Text = "Xóa bình luận thành công";
                        }
                        else
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình xử lý : Có thể bạn chưa xóa hết các phần liên quan";
                        }

                        context.Response.Write(responseMsg.ToJsonString());
                        return;

                    case "published":
                        if (!HttpContext.Current.User.IsInRole("Administrator") && !HttpContext.Current.User.IsInRole("Comment"))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Không có quyền";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }
                        var arrCommentIds = context.Request.Form.GetValues("id[]");
                        if (arrCommentIds == null || arrCommentIds.Count() == 0)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Bạn phải chọn ít nhất 1 bình luận để duyệt";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }


                        var joinIds = string.Empty;
                        foreach (var id in arrCommentIds)
                        {
                            if (Utils.IsNumber(id))
                                joinIds += "," + id;
                        }

                        if (string.IsNullOrEmpty(joinIds))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Bạn phải chọn ít nhất 1 Comment để xóa";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        joinIds = joinIds.TrimStart(',');
                        return_val = CommentBo.PublishedComments(joinIds);



                        if (return_val != -1)
                        {
                            responseMsg.Success = true;
                            responseMsg.Text = "Duyệt bình luận thành công";
                        }
                        else
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình xử lý : Có thể bạn chưa Duyệt hết các phần liên quan";
                        }

                        context.Response.Write(responseMsg.ToJsonString());
                        return;

                }
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "CommentServicePost", "CommentService");
                responseMsg.Success = false;
                responseMsg.Text = "Có lỗi trong quá trình xử lý - execption";
                context.Response.Write(responseMsg.ToJsonString());
                return;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }




    }
}