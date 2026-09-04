using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIZ;
using BIZ.Entity;
using UTILS;
using Constants = UTILS.Constants;

namespace WebMVC4.Post
{
    /// <summary>
    /// Summary description for ArticleService
    /// </summary>
    public class ArticleService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var IsNewsPublish = false;
            if (HttpContext.Current.User.IsInRole("Administrator") || HttpContext.Current.User.IsInRole("NewsPublish"))
                IsNewsPublish = true;

            var IsNewsEdit = false;
            if (HttpContext.Current.User.IsInRole("Administrator") || HttpContext.Current.User.IsInRole("NewsEdit"))
                IsNewsEdit = true;
            context.Response.ContentType = "text/plain";
            ResponseMsg responseMsg = new ResponseMsg();
            ContentBO contentBo = new ContentBO();
            try
            {
                string method = context.Request["__m"];
                int return_val = 0;

                switch (method.ToLower())
                {

                    case "save":

                        var contentId = context.Request.Form["id"];
                        var status = context.Request.Form["status"];
                        var createdby = context.Request.Form["createdby"];
                        var alias = context.Request.Form["alias"];
                        if (string.IsNullOrEmpty(status))
                            status = "0";

                        if (!CheckPermission(Convert.ToByte(status), IsNewsPublish, IsNewsEdit, createdby))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Không có quyền";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }
                        var func = context.Request.Form["func"];

                        //setstatus
                        switch (func.ToLower())
                        {
                            case "add":
                                //nếu tạo mới chuyển sang đang biên tập
                                if (status == "0")
                                    status = "1";
                                //nếu quyền xuất bản tạo mới sang chờ xuất bản luôn
                                if (IsNewsPublish)
                                    status = "2";
                                break;
                            case "publish":
                                status = "4";
                                break;
                            case "reject":
                                status = "3";
                                break;
                            case "send":
                                status = "2";
                                break;
                            case "del":
                                return_val = contentBo.DeleteContent(Convert.ToInt32(contentId));
                                if (return_val != -1)
                                {
                                    responseMsg.Success = true;
                                    responseMsg.Text = "Xóa bài viếtthành công";
                                }
                                else
                                {
                                    responseMsg.Success = false;
                                    responseMsg.Text = "Có lỗi trong quá trình xử lý";
                                }

                                context.Response.Write(responseMsg.ToJsonString());
                                return;

                            default:
                                break;
                        }

                        var title = HttpUtility.UrlDecode(context.Request.Form["title"]);
                        var linkmedia = HttpUtility.UrlDecode(context.Request.Form["linkmedia"]);
                        var categoryId = context.Request.Form["categoryid"];
                        var categoryPathway = context.Request.Form["categoryPathway"];
                        var intro = HttpUtility.UrlDecode(context.Request.Form["intro"]);
                        var type = context.Request.Form["type"];

                        if (!Utils.IsNumber(contentId) || !Utils.IsNumber(categoryId) || !Utils.IsNumber(status) || !Utils.IsNumber(type))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình lấy thông tin";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        var content = new CONTENT_FULL();
                        if (Convert.ToInt32(contentId) > 0)
                            content = contentBo.GetContentFull(Convert.ToInt32(contentId));
                        else
                            content.CreatedDate = DateTime.Now;

                        content.Id = Convert.ToInt32(contentId);
                        content.Title = title;
                        content.Url = linkmedia;
                        content.Alias = alias;

                        if (Convert.ToInt32(categoryId) != 0)
                            content.CategoryId = Convert.ToInt32(categoryId);
                        else
                            content.CategoryId = null;
                        content.CategoryPathway = categoryPathway;
                        content.IntroText = intro;
                         
                        content.Status = Convert.ToByte(status);
                        content.Type = Convert.ToByte(type);
                        var publishdate = HttpUtility.UrlDecode   (context.Request.Form["publishdate"]);
                        if (String.IsNullOrEmpty(publishdate))
                            content.PublishDate = DateTime.Now;
                        else
                        {
                            content.PublishDate = Utils.ConvertToDate(publishdate, "dd-MM-yyyy hh:mm");

                        }
                        content.Contents = HttpUtility.UrlDecode(context.Request.Form["contents"]);
                        content.CreatedBy = HttpContext.Current.User.Identity.Name;



                        content.Params = HttpUtility.UrlDecode(context.Request.Form["params"]);

                        var listImages = string.Empty;
                        var mainImage = context.Request.Form["mainimage"];
                        var images = context.Request.Form.GetValues("image[]");
                        if (images != null)
                        {
                            if (!string.IsNullOrEmpty(mainImage))
                                listImages = mainImage + ",";

                            foreach (var image in images)
                            {
                                if (image != mainImage)
                                    listImages += image + ",";
                            }

                            listImages = listImages.TrimEnd(',');

                        }

                        content.Image = listImages;

                        return_val = contentBo.CreateUpdateContent(content);

                        if (return_val != -1)
                        {
                            responseMsg.Success = true;
                            responseMsg.Value = Convert.ToString(return_val);
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
                        var arrContentId = context.Request.Form.GetValues("id[]");
                        if (arrContentId == null || arrContentId.Count() == 0)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Bạn phải chọn ít nhất 1 bài viết để xóa";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        if (arrContentId.Count() == 1)
                        {
                            return_val = contentBo.DeleteContent(Convert.ToInt32(arrContentId[0]));
                        }
                        else
                        {
                            string joinId = string.Empty;
                            foreach (var id in arrContentId)
                            {
                                if (Utils.IsNumber(id))
                                    joinId += "," + id;
                            }

                            if (string.IsNullOrEmpty(joinId))
                            {
                                responseMsg.Success = false;
                                responseMsg.Text = "Bạn phải chọn ít nhất 1 bài viếtđể xóa";
                                context.Response.Write(responseMsg.ToJsonString());
                                return;
                            }

                            joinId = joinId.TrimStart(',');
                            return_val = contentBo.DeleteContents(joinId);
                        }

                        if (return_val != -1)
                        {
                            responseMsg.Success = true;
                            responseMsg.Text = "Xóa bài viếtthành công";
                        }
                        else
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình xử lý";
                        }

                        context.Response.Write(responseMsg.ToJsonString());
                        return;


                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
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
        public bool CheckPermission(int Status, bool IsNewsPublish, bool IsNewsEdit, string Createdby)
        {
            if (IsNewsPublish)
            {

                if (Status == 2 || Status == 4)
                {

                    return true;
                }
            }

            if (IsNewsEdit)
            {
                if (Status == 0)
                    return true;
                if (Status == 1 || Status == 3)
                {
                    if (HttpContext.Current.User.Identity.Name.ToLower() == Createdby.ToLower())
                        return true;
                }

            }
            return false;
        }
    }
}