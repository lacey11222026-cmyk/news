using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using BIZ;
using BIZ.Entity;
using UTILS;
using DATA;

using System.Configuration;

namespace WebMVC4.Post
{
    /// <summary>
    /// Summary description for ContactService
    /// </summary>
    public class ContactService : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            ResponseMsg responseMsg = new ResponseMsg();
            //StringBuilder stringBuilder;
            ContactBO ContactBo = new ContactBO();
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

                        var contactId = context.Request.Form["id"];
                        var name = HttpUtility.UrlDecode(context.Request.Form["name"]);
                        var categoryId = context.Request.Form["categoryid"];
                        var yahoo = context.Request.Form["yahoo"];
                        var categoryPathway = context.Request.Form["categoryPathway"];
                        var mail = context.Request.Form["mail"];

                        var mobile = context.Request.Form["mobile"];
                        var published = context.Request.Form["published"];



                        if (!Utils.IsNumber(contactId) || !Utils.IsNumber(categoryId) || !Utils.IsNumber(published))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình lấy thông tin";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        var Contact = new Contact();

                        if (Convert.ToInt32(contactId) > 0)
                        {
                            Contact = ContactBo.GetContact(Convert.ToInt32(contactId));
                        }

                        Contact.Id = Convert.ToInt32(contactId);

                        if (Convert.ToInt32(categoryId) != 0)
                            Contact.CategoryId = Convert.ToInt32(categoryId);
                        else
                            Contact.CategoryId = null;

                        Contact.Name = name;
                        Contact.Yahoo = yahoo;
                        Contact.CategoryPathway = categoryPathway;
                        Contact.Mail = mail;

                        Contact.Mobile = mobile;
                        Contact.Published = Convert.ToByte(published);


                        return_val = ContactBo.CreateUpdateContact(Contact);

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
                        var arrcontactId = context.Request.Form.GetValues("id[]");
                        if (arrcontactId == null || arrcontactId.Count() == 0)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Bạn phải chọn ít nhất 1 sản phẩm";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        if (arrcontactId.Count() == 1)
                        {
                            return_val = ContactBo.DeleteContact(Convert.ToInt32(arrcontactId[0]));
                        }
                        else
                        {
                            string joinId = string.Empty;
                            foreach (var id in arrcontactId)
                            {
                                if (Utils.IsNumber(id))
                                    joinId += "," + id;
                            }

                            if (string.IsNullOrEmpty(joinId))
                            {
                                responseMsg.Success = false;
                                responseMsg.Text = "Bạn phải chọn ít nhất 1 sản phẩm";
                                context.Response.Write(responseMsg.ToJsonString());
                                return;
                            }

                            joinId = joinId.TrimStart(',');
                            return_val = ContactBo.DeleteContacts(joinId);
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


                    case "send_listsms":
                        var content = HttpUtility.UrlDecode(context.Request.Form["content"]);
                        var arrphone = context.Request.Form.GetValues("phone[]");
                        if (arrphone == null || arrphone.Count() == 0)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Bạn phải chọn ít nhất 1 danh bạ";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }


                        string phonenumbers = string.Empty;
                        foreach (var id in arrphone)
                        {
                            if (Utils.IsNumber(id))
                                phonenumbers += "," + id;
                        }

                        if (string.IsNullOrEmpty(phonenumbers))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Bạn phải chọn ít nhất 1 danh bạn";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        phonenumbers = phonenumbers.TrimStart(',');
                        //return_val = new SMSBO().MultiSend(phonenumbers, content, ConfigurationManager.AppSettings["SMSCODE"]);

                        return_val = 1;
                        if (return_val == -1)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình xử lý";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;

                        }

                        responseMsg.Success = true;
                        responseMsg.Text = "Gửi tin nhắn thành công";
                        context.Response.Write(responseMsg.ToJsonString());
                        return;


                }
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "ContactServiceGet", "ContactService");
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