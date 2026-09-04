using CuteWebUI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using UTILS;

namespace WebMVC4.Post
{
    public class UploadHandler : MvcHandler
    {
        
        public override UploaderValidateOption GetValidateOption()
        {
            var option = new UploaderValidateOption
            {
                MaxSizeKB = 200 * 1024,
                AllowedFileExtensions = "*.jpg,*.png,*.gif,*.doc,*.docx,*.rar,*.zip,*.xls,*.xlsx,*.ppt,*.swf,*.flv,*.mp3,*.avi,*.mp4,*.pdf"
            };
            return option;
        }

        /// <summary>
        /// Create      : Thai.Tran
        /// Date        : 23/11/2011
        /// </summary>
        /// <param name="file"></param>
        public override void OnFileUploaded(MvcUploadFile file)
        {
            if (string.Equals(Path.GetExtension(file.FileName), ".bmp", StringComparison.OrdinalIgnoreCase))
            {
                file.Delete();
                throw (new Exception("Không Upload ảnh định dạng .bmp"));
            }

            SetServerData("this value will pass to javascript api as item.ServerData");



           try
            {
               
                var strUploadPath = HttpContext.Current.Request.PhysicalApplicationPath + ConfigurationManager.AppSettings["UploadPath"] + "User\\" + HttpContext.Current.User.Identity.Name + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString() + "\\";
                if (!Directory.Exists(strUploadPath)) { Directory.CreateDirectory(strUploadPath); }
                var filename = Path.GetFileNameWithoutExtension(file.FileName);
                filename = Utils.ReplaceVietnameseChar(filename).Replace(" ", "_");
                file.MoveTo(strUploadPath + file.FileName.Replace(Path.GetFileNameWithoutExtension(file.FileName), filename));
                //file.MoveTo(strUploadPath + file.FileName.Replace(Path.GetFileNameWithoutExtension(file.FileName), DateTime.Now.ToString("yyMMddhhmmss")));
                


            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "UploadHandler", "OnFileUploaded , file=" + file.FileName);

            }
        }

       
        public override void OnUploaderInit(MvcUploader uploader)
        {
           
        }


    }
}