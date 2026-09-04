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
    /// Summary description for AlbumService
    /// </summary>
    public class AlbumService : IHttpHandler
    {

        private delegate string DelegateDeleteImages(HttpRequest request, int id, string entityName);

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            ResponseMsg responseMsg = new ResponseMsg();
            AlbumBO AlbumBo = new AlbumBO();
            try
            {
                string method = context.Request["__m"];
                int return_val;
                if (!HttpContext.Current.User.IsInRole("Administrator") && !HttpContext.Current.User.IsInRole("Album"))
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
                        var style = context.Request.Form["style"];
                        var categoryPathway = context.Request.Form["categorypathway"];
                        var description = HttpUtility.UrlDecode(context.Request.Form["description"]);

                        var hits = context.Request.Form["hits"];
                        var status = context.Request.Form["status"];
                        if (!Utils.IsNumber(AlbumId) || !Utils.IsNumber(categoryId) || !Utils.IsNumber(status))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình lấy thông tin";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        var Album = new Album();

                        if (Convert.ToInt32(AlbumId) > 0)
                        {
                            Album = AlbumBo.GetAlbum(Convert.ToInt32(AlbumId));
                        }
                        else
                        {
                            Album.PublishDate = DateTime.Now;
                            Album.CreatedBy = HttpContext.Current.User.Identity.Name;
                        }

                        Album.Id = Convert.ToInt32(AlbumId);
                        Album.Title = title;

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
                        Album.Status = Convert.ToByte(status);
                        Album.Style = Convert.ToInt32( style);
                        Album.Param = HttpUtility.UrlDecode(context.Request.Form["param"]);
                        var mainImage = HttpUtility.UrlDecode(context.Request.Form["mainimage"]);
                        var images = context.Request.Form.GetValues("image[]");
                        var listImages = "";


                        if (!string.IsNullOrEmpty(mainImage))
                            listImages = mainImage + ",";
                        if (images != null)
                        {
                            foreach (var image in images)
                            {
                                listImages += HttpUtility.UrlDecode(image) + ",";
                            }
                            
                        }
                        listImages = "[" + listImages.TrimEnd(',') + "]";
                        Album.Images = listImages;
                        return_val = AlbumBo.CreateUpdateAlbum(Album);

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
                            if (return_val != -1)
                            {
                                

                              
                                    DelegateDeleteImages delegateDeleteImages = Utils.DeleteFiles;
                                    delegateDeleteImages.BeginInvoke(context.Request, Convert.ToInt32(arrAlbumId[0]), "Album", null, null);
                                

                            }
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
                            if (return_val != -1)
                            {
                                DelegateDeleteImages delegateDeleteImages = Utils.DeleteFiles;
                                string[] arrJoinId = joinId.Split(',');
                                foreach (var joinid in arrJoinId)
                                {
                                    delegateDeleteImages.BeginInvoke(context.Request, Convert.ToInt32(joinid), "Album", null, null);
                                }

                            }
                        }

                        if (return_val != -1)
                        {
                            responseMsg.Success = true;
                            responseMsg.Text = "Xóa sản phẩm thành công";
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
                ExHandler.Handle(ex, "AlbumServicePost", "AlbumService");
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