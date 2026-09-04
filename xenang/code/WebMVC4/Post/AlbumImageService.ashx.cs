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

namespace WebMVC4.Post
{
    /// <summary>
    /// Summary description for AlbumImageService
    /// </summary>
    public class AlbumImageService : IHttpHandler
    {

       

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            ResponseMsg responseMsg = new ResponseMsg();
            AlbumImageBO AlbumBo = new AlbumImageBO();
            try
            {
                string method = context.Request["__m"];
                int return_val;
                if (!HttpContext.Current.User.IsInRole("Administrator") && !HttpContext.Current.User.IsInRole("Competition") && !HttpContext.Current.User.IsInRole("CompetitionCreate"))
                {
                    responseMsg.Success = false;
                    responseMsg.Text = "Không có quyền";
                    context.Response.Write(responseMsg.ToJsonString());
                    return;
                }
                switch (method.ToLower())
                {
                    case "save":

                        var AlbumId = context.Request.Form["id"];
                        var title = HttpUtility.UrlDecode(context.Request.Form["title"]);
                        var categoryId = context.Request.Form["categoryid"];
                        var author =  HttpUtility.UrlDecode(context.Request.Form["author"]);
                        var image = HttpUtility.UrlDecode(context.Request.Form["image"]);
                        var categoryPathway = context.Request.Form["categorypathway"];
                        var description = HttpUtility.UrlDecode(context.Request.Form["description"]);
                        var code = HttpUtility.UrlDecode(context.Request.Form["code"]);
                        var groupname = HttpUtility.UrlDecode(context.Request.Form["groupname"]);
                        var publishdate = HttpUtility.UrlDecode(context.Request.Form["publishdate"]);
                        var type = context.Request.Form["type"];
                        var status = context.Request.Form["status"];
                        if (!Utils.IsNumber(AlbumId) || !Utils.IsNumber(categoryId) || !Utils.IsNumber(status))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình lấy thông tin";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        var Album = new AlbumImage_FULL();

                        var isCreate = false;
                        if (Convert.ToInt32(AlbumId) > 0)
                        {
                            Album = AlbumBo.GetAlbum(Convert.ToInt32(AlbumId));
                        }
                        else
                        {
                            Album.PublishDate = DateTime.Now;
                            Album.Param = HttpContext.Current.User.Identity.Name;
                            isCreate = true;
                            Album.Code = " ";
                        }
                        if (String.IsNullOrEmpty(publishdate))
                            Album.PublishDate = DateTime.Now;
                        else
                        {
                            Album.PublishDate = Utils.ConvertToDate(publishdate, "dd-MM-yyyy hh:mm");

                        }
                        Album.Id = Convert.ToInt32(AlbumId);
                        Album.Name = title;

                        if (Convert.ToInt32(categoryId) != 0)
                        {
                            Album.CategoryId = Convert.ToInt32(categoryId);
                            Album.CategoryPathway = categoryPathway;
                        }
                        else
                        {
                            Album.CategoryId = 0;

                        }
                        Album.Description = description;
                        Album.Code = code;
                        Album.GroupName = groupname;
                        Album.Author = author;
                        Album.Status = Convert.ToInt32(status);
                        Album.Type = Convert.ToInt32(type);
                       // Album.Param = HttpUtility.UrlDecode(context.Request.Form["param"]);
                        

                        Album.Image = image;
                        return_val = AlbumBo.CreateUpdateAlbum(Album);

                        if (return_val != -1)
                        {


                            //update 
                            if (isCreate)
                            {
                                Album.Id = return_val;
                                Album.Code = String.Format("{0}{1}", "MS", return_val);
                                AlbumBo.CreateUpdateAlbum(Album);
                            }
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
                        var arrAlbumId = context.Request.Form.GetValues("id[]");
                        if (arrAlbumId == null || arrAlbumId.Count() == 0)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Bạn phải chọn ít nhất 1 album để xóa";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        if (arrAlbumId.Count() == 1)
                        {
                            return_val = AlbumBo.DeleteAlbum(Convert.ToInt32(arrAlbumId[0]));
                           
                        }
                        else
                        {
                            string joinId = string.Empty;
                            foreach (var id in arrAlbumId)
                            {
                                if (Utils.IsNumber(id))
                                    joinId += "," + id;
                            }

                            if (string.IsNullOrEmpty(joinId))
                            {
                                responseMsg.Success = false;
                                responseMsg.Text = "Bạn phải chọn ít nhất 1 album để xóa";
                                context.Response.Write(responseMsg.ToJsonString());
                                return;
                            }

                            joinId = joinId.TrimStart(',');
                            return_val = AlbumBo.DeleteAlbums(joinId);
                            
                        }

                        if (return_val != -1)
                        {
                            responseMsg.Success = true;
                            responseMsg.Text = "Xóa ảnhthành công";
                        }
                        else
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình xử lý : Có thể bạn chưa xóa hết các phần liên quan";
                        }

                        context.Response.Write(responseMsg.ToJsonString());
                        return;


                }
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "AlbumImageServicePost", "AlbumImageService");
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