using System;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.SessionState;
using BIZ.Entity;
using UTILS;

namespace WebMVC4.Post
{
    /// <summary>
    /// Summary description for UploadService
    /// </summary>
    public class ImageService : IHttpHandler, IReadOnlySessionState
    {
        ResponseMsg responseMsg = new ResponseMsg();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            if (!HttpContext.Current.User.IsInRole("Administrator") && !HttpContext.Current.User.IsInRole("CompetitionCreate") && !HttpContext.Current.User.IsInRole("Competition") && !HttpContext.Current.User.IsInRole("Album") && !HttpContext.Current.User.IsInRole("NewsEdit") && !HttpContext.Current.User.IsInRole("NewsPublish"))
            {
                responseMsg.Success = false;
                responseMsg.Text = "Không có quyền";
                context.Response.Write(responseMsg.ToJsonString());
                return;
            }
            try
            {
                string method = context.Request["__m"];
                string id = context.Request.Form["_id"];
                // if id = -1 => home page banner upload
                if (!Utils.IsNumber(id))
                {
                    responseMsg.Success = false;
                    responseMsg.Text = "Có lỗi trong quá trình lấy thông tin sản phẩm";
                    context.Response.Write(responseMsg.ToJsonString());
                    return;
                }

                HttpPostedFile file;
                string fileExt;
                string fileName;
                string entityName = context.Request.Form["_en"];

                switch (method.ToLower())
                {
                    case "upl":

                        fileExt = context.Request.Form["ext"];
                        if (string.IsNullOrEmpty(fileExt))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "File của bạn không đúng định dạng cho phép";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        file = context.Request.Files[0];
                        if (file == null)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Bạn phải chọn ít nhất một file để tải lên";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }
                        if (entityName == "tinycme")
                        {
                            string url = context.Request["_url"];

                            context.Response.Write(UploadImageForEditor(context.Request, url, fileExt, entityName));
                            return;
                        }
                        context.Response.Write(UploadImage(context.Request, Convert.ToInt32(id), fileExt, entityName));
                        return;


                    case "del":
                        fileName = context.Request.Form["fna"];
                        if (string.IsNullOrEmpty(fileName))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "File của bạn không đúng định dạng cho phép";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        context.Response.Write(DeleteImage(context.Request, Convert.ToInt32(id), fileName, entityName));
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
        protected string UploadImageForEditor(HttpRequest request, string url, string fileExt, string entityName)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(Guid.NewGuid()).Append(Utils.ConvertToDateTimeStamp(DateTime.Now)).Append("-");
            string file_prefix = strBuilder.ToString();
            string uploadInfo;

            // upload
            var type = ConfigurationManager.AppSettings["EnableFTP"];
            string[] arrUploadInfo;


            uploadInfo = Utils.UploadURL(request, url, entityName, fileExt, file_prefix, false);


            arrUploadInfo = uploadInfo.Split('|');
            if (arrUploadInfo[0] == "fail")
            {
                responseMsg.Success = false;
                switch (arrUploadInfo[1])
                {
                    case "file_not_exist":
                        responseMsg.Text = "Bạn phải chọn ít nhất một file để tải lên";
                        break;
                    case "denied_content_type":
                        responseMsg.Text = "File của bạn không đúng định dạng cho phép";
                        break;
                    case "id_not_exist":
                        responseMsg.Text = "Có lỗi trong quá trình lấy thông tin sản phẩm";
                        break;
                }
                return responseMsg.ToJsonString();
            }

            responseMsg.Success = true;
            responseMsg.Text = "Tải ảnh từ máy tính thành công ";
            responseMsg.Value = arrUploadInfo[1];
            return responseMsg.ToJsonString();
        }

        protected string UploadImage(HttpRequest request, int id, string fileExt, string entityName)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(Guid.NewGuid()).Append(Utils.ConvertToDateTimeStamp(DateTime.Now)).Append("-");
            string file_prefix = strBuilder.ToString();
            string uploadInfo;

            // upload
            var type = ConfigurationManager.AppSettings["EnableFTP"];
            string[] arrUploadInfo;
            if (type == "1")
            {
                uploadInfo = new Utils().FTPUpload(request, id, entityName, fileExt, file_prefix);
                arrUploadInfo = uploadInfo.Split('|');
                if (arrUploadInfo[0] != "fail")
                {
                    uploadInfo = new Utils().FTPThumbUpload(request, id, entityName, fileExt, file_prefix);
                }
            }
            else
            {
                if (entityName == "Album" || entityName == "Article")
                {
                    uploadInfo = Utils.Upload(request, id, entityName, fileExt, file_prefix, false);

                }
                else
                {
                    uploadInfo = Utils.Upload(request, id, entityName, fileExt, file_prefix, true);

                }

            }

            arrUploadInfo = uploadInfo.Split('|');
            if (arrUploadInfo[0] == "fail")
            {
                responseMsg.Success = false;
                switch (arrUploadInfo[1])
                {
                    case "file_not_exist":
                        responseMsg.Text = "Bạn phải chọn ít nhất một file để tải lên";
                        break;
                    case "denied_content_type":
                        responseMsg.Text = "File của bạn không đúng định dạng cho phép";
                        break;
                    case "id_not_exist":
                        responseMsg.Text = "Có lỗi trong quá trình lấy thông tin sản phẩm";
                        break;
                }
                return responseMsg.ToJsonString();
            }

            responseMsg.Success = true;
            responseMsg.Text = "Tải ảnh từ máy tính thành công ";
            responseMsg.Value = arrUploadInfo[1];
            return responseMsg.ToJsonString();
        }

        protected string DeleteImage(HttpRequest request, int id, string fileName, string entityName)
        {
            // delete
            string returnVal;
            var type = ConfigurationManager.AppSettings["EnableFTP"];

            if (type == "1")
                returnVal = Utils.DeleteFileFTP(request, id, fileName, entityName);
            else
                returnVal = Utils.DeleteFile(request, id, fileName, entityName);

            if (returnVal == "fail")
            {
                responseMsg.Success = false;
                responseMsg.Text = "Xóa ảnh không thành công";
                return responseMsg.ToJsonString();
            }

            responseMsg.Success = true;
            responseMsg.Text = "Xóa ảnh từ máy tính thành công ";
            return responseMsg.ToJsonString();
        }
    }
}