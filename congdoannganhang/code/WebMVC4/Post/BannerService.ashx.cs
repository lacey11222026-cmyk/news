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
    /// Summary description for BannerService
    /// </summary>
    public class BannerService : IHttpHandler
    {


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            ResponseMsg responseMsg = new ResponseMsg();
            BannerBO BannerBo = new BannerBO();
            try
            {
                string method = context.Request["__m"];
                int return_val;
                if (!HttpContext.Current.User.IsInRole("Administrator") && !HttpContext.Current.User.IsInRole("Banner"))
                {
                    responseMsg.Success = false;
                    responseMsg.Text = "Không có quyền";
                    context.Response.Write(responseMsg.ToJsonString());
                    return;
                }
                switch (method.ToLower())
                {
                    case "save":

                        var BannerId = context.Request.Form["id"];
                        var name = HttpUtility.UrlDecode(context.Request.Form["name"]);
                        var description = HttpUtility.UrlDecode(context.Request.Form["description"]);
                        var data = HttpUtility.UrlDecode(context.Request.Form["data"]);
                        var url = HttpUtility.UrlDecode(context.Request.Form["url"]);
                        var region = context.Request.Form["region"];
                        var type = context.Request.Form["type"];

                        var order = context.Request.Form["order"];
                       

                        var status = context.Request.Form["status"];
                        if (!Utils.IsNumber(BannerId) || !Utils.IsNumber(status))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình lấy thông tin";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }
                        if (!HttpContext.Current.User.IsInRole("Administrator") && !HttpContext.Current.User.IsInRole("Banner") && (status != "0"))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Không có quyền";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }
                        var Banner = new Banner();

                        if (Convert.ToInt32(BannerId) > 0)
                        {
                            Banner = BannerBo.GetBanner(Convert.ToInt32(BannerId));
                        }
                        
                        Banner.Data = data;
                        Banner.Description = description;
                        Banner.Url = url;
                        Banner.Id = Convert.ToInt32(BannerId);
                        Banner.Order = Convert.ToInt32(order);
                        Banner.Name = name;
                        Banner.Status = Convert.ToByte(status);
                        Banner.Region = Convert.ToInt32(region);
                        Banner.Type = Convert.ToByte(type);

                        return_val = BannerBo.CreateUpdateBanner(Banner);

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
                       
                        var arrBannerId = context.Request.Form.GetValues("id[]");
                        if (arrBannerId == null || arrBannerId.Count() == 0)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Bạn phải chọn ít nhất 1 dữ liệu để xóa";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        if (arrBannerId.Count() == 1)
                        {
                            return_val = BannerBo.DeleteBanner(Convert.ToInt32(arrBannerId[0]));

                        }
                        else
                        {
                            string joinId = string.Empty;
                            foreach (var id in arrBannerId)
                            {
                                if (Utils.IsNumber(id))
                                    joinId += "," + id;
                            }

                            if (string.IsNullOrEmpty(joinId))
                            {
                                responseMsg.Success = false;
                                responseMsg.Text = "Bạn phải chọn ít nhất 1 dữ liệu để xóa";
                                context.Response.Write(responseMsg.ToJsonString());
                                return;
                            }

                            joinId = joinId.TrimStart(',');
                            return_val = BannerBo.DeleteBanners(joinId);

                        }

                        if (return_val != -1)
                        {
                            responseMsg.Success = true;
                            responseMsg.Text = "Xóa dữ liệu thành công";
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




    }
}