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
    /// Summary description for DocumentService
    /// </summary>
    public class DocumentService : IHttpHandler
    {



        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            ResponseMsg responseMsg = new ResponseMsg();
            DocumentBO DocumentBo = new DocumentBO();
            try
            {
                string method = context.Request["__m"];
                int return_val;
                if (!HttpContext.Current.User.IsInRole("Administrator") && !HttpContext.Current.User.IsInRole("Document"))
                {
                    responseMsg.Success = false;
                    responseMsg.Text = "Không có quyền";
                    context.Response.Write(responseMsg.ToJsonString());
                    return;
                }
                switch (method.ToLower())
                {
                    case "save":

                        var DocumentId = context.Request.Form["id"];
                        var name = HttpUtility.UrlDecode(context.Request.Form["name"]);
                        var categoryId = context.Request.Form["categoryid"];
                        var filepath = context.Request.Form["filepath"];
                        var code = context.Request.Form["code"];
                        var expirydate = context.Request.Form["expirydate"];
                        var effectivedate = context.Request.Form["effectivedate"];
                        var publishdate = context.Request.Form["publishdate"];
                        var signedby = HttpUtility.UrlDecode(context.Request.Form["signedby"]);
                        var signedbydesc = HttpUtility.UrlDecode(context.Request.Form["signedbydesc"]);
                        var categorypathway = context.Request.Form["categorypathway"];
                        var description = HttpUtility.UrlDecode(context.Request.Form["description"]);
                        var createdby = context.Request.Form["createdby"];
                        var hits = context.Request.Form["hits"];
                        var status = context.Request.Form["status"];
                        if (!Utils.IsNumber(DocumentId) || !Utils.IsNumber(categoryId) || !Utils.IsNumber(status))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình lấy thông tin";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        var Document = new Document();

                        if (Convert.ToInt32(DocumentId) > 0)
                        {
                            Document = DocumentBo.GetDocument(Convert.ToInt32(DocumentId));
                        }
                        else
                        {
                            Document.CreatedDate = DateTime.Now;
                            Document.CreatedBy = HttpContext.Current.User.Identity.Name;
                        }

                        Document.Id = Convert.ToInt32(DocumentId);
                        Document.Name = name;

                        if (Convert.ToInt32(categoryId) != 0)
                        {
                            Document.CategoryId = Convert.ToInt32(categoryId);
                            Document.CategoryPathway = categorypathway;
                        }
                        else
                        {
                            Document.CategoryId = 0;

                        }
                        Document.Description = description;
                        Document.Status = Convert.ToByte(status);
                        Document.SignedBy = signedby;
                        Document.SignedByDesc = signedbydesc;
                        Document.PublishDate = Utils.ConvertToDate(publishdate, "dd-MM-yyyy");
                        Document.EffectiveDate = Utils.ConvertToDate(effectivedate, "dd-MM-yyyy");
                        if (String.IsNullOrEmpty(expirydate))
                        {
                            expirydate = "01/01/9999";
                        }
                        Document.ExpiryDate = Utils.ConvertToDate(expirydate, "dd-MM-yyyy");
                        Document.Code = code;

                        Document.FilePath = filepath;
                        return_val = DocumentBo.CreateUpdateDocument(Document);

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
                        var arrDocumentId = context.Request.Form.GetValues("id[]");
                        if (arrDocumentId == null || arrDocumentId.Count() == 0)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Bạn phải chọn ít nhất 1 document để xóa";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        if (arrDocumentId.Count() == 1)
                        {
                            return_val = DocumentBo.DeleteDocument(Convert.ToInt32(arrDocumentId[0]));

                        }
                        else
                        {
                            string joinId = string.Empty;
                            foreach (var id in arrDocumentId)
                            {
                                if (Utils.IsNumber(id))
                                    joinId += "," + id;
                            }

                            if (string.IsNullOrEmpty(joinId))
                            {
                                responseMsg.Success = false;
                                responseMsg.Text = "Bạn phải chọn ít nhất 1 document để xóa";
                                context.Response.Write(responseMsg.ToJsonString());
                                return;
                            }

                            joinId = joinId.TrimStart(',');
                            return_val = DocumentBo.DeleteDocuments(joinId);

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
                ExHandler.Handle(ex, "DocumentServicePost", "DocumentService");
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