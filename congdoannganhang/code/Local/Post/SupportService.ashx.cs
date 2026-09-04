using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using BIZ;
using BIZ.Entity;
using UTILS;

namespace Local.Post
{
    /// <summary>
    /// Summary description for SupportService
    /// </summary>
    public class SupportService : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            ResponseMsg responseMsg = new ResponseMsg();
            //StringBuilder stringBuilder;
            SupportBO supportBo = new SupportBO();
            try
            {

                if (!HttpContext.Current.User.IsInRole("Administrator") && !HttpContext.Current.User.IsInRole("Category"))
                {
                    responseMsg.Success = false;
                    responseMsg.Text = "Không có quyền";
                    context.Response.Write(responseMsg.ToJsonString());
                    return;
                }
                string method = context.Request["__m"];
                int return_val;
                switch (method.ToLower())
                {
                    case "save":

                        var supportId = context.Request.Form["id"];
                        var supporter = HttpUtility.UrlDecode(context.Request.Form["Contacter"]);
                        var categoryId = context.Request.Form["categoryid"];
                        var yahoo = context.Request.Form["yahoo"];
                        var skype = context.Request.Form["skype"];
                        var mail = context.Request.Form["mail"];
                        var phone = context.Request.Form["phone"];
                        var mobile = context.Request.Form["mobile"];
                        var published = context.Request.Form["published"];
                        var ordering = context.Request.Form["ordering"];
                        //var _params = context.Request.Form.GetValues("params[]");

                        if (!Utils.IsNumber(supportId) || !Utils.IsNumber(categoryId) || !Utils.IsNumber(published) || !Utils.IsNumber(ordering))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình lấy thông tin";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        var support = new SUPPORT_FULL();

                        if (Convert.ToInt32(supportId) > 0)
                        {
                            support = supportBo.GetSupportFull(Convert.ToInt32(supportId));
                        }

                        support.Id = Convert.ToInt32(supportId);

                        if (Convert.ToInt32(categoryId) != 0)
                            support.CategoryId = Convert.ToInt32(categoryId);
                        else
                            support.CategoryId = null;

                        support.Supporter = supporter;
                        support.Yahoo = yahoo;
                        support.Skype = skype;
                        support.Mail = mail;
                        support.Phone = phone;
                        support.Mobile = mobile;
                        support.Published = Convert.ToByte(published);
                        support.Ordering = Convert.ToByte(ordering);

                        return_val = supportBo.CreateUpdateSupport(support);

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
                        var arrSupportId = context.Request.Form.GetValues("id[]");
                        if (arrSupportId == null || arrSupportId.Count() == 0)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Bạn phải chọn ít nhất 1 người hỗ trợ";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        if (arrSupportId.Count() == 1)
                        {
                            return_val = supportBo.DeleteSupport(Convert.ToInt32(arrSupportId[0]));
                        }
                        else
                        {
                            string joinId = string.Empty;
                            foreach (var id in arrSupportId)
                            {
                                if (Utils.IsNumber(id))
                                    joinId += "," + id;
                            }

                            if (string.IsNullOrEmpty(joinId))
                            {
                                responseMsg.Success = false;
                                responseMsg.Text = "Bạn phải chọn ít nhất 1 người hỗ trợ";
                                context.Response.Write(responseMsg.ToJsonString());
                                return;
                            }

                            joinId = joinId.TrimStart(',');
                            return_val = supportBo.DeleteSupports(joinId);
                        }

                        if (return_val == -1)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình xử lý";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;

                        }

                        responseMsg.Success = true;
                        responseMsg.Text = "Xóa người hỗ trợ thành công";
                        context.Response.Write(responseMsg.ToJsonString());
                        return;


                }
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "SupportService", "SupportService");
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