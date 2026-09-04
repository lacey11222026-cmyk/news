using CuteWebUI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using UTILS;

namespace Local.Post
{
    /// <summary>
    /// Summary description for AlbumUploadContent
    /// </summary>
    public class AlbumUploadHandler : MvcHandler
    {
        public int id = 0;
        public override UploaderValidateOption GetValidateOption()
        {
            var option = new UploaderValidateOption
            {
                MaxSizeKB = 200 * 1024,
                AllowedFileExtensions = "*.jpg"
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
                if (id > 0)
                {
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.Append(HttpContext.Current.Request.PhysicalApplicationPath).Append(ConfigurationManager.AppSettings["UploadPath"]).Append("Album").Append("\\").Append(id / 100000).Append("\\").Append(id / 100).Append("\\").Append(id).Append("\\");
                    var strUploadPath = strBuilder.ToString();
                    if (!Directory.Exists(strUploadPath)) { Directory.CreateDirectory(strUploadPath); }
                    var filename = Path.GetFileNameWithoutExtension(file.FileName);
                    filename = Utils.ReplaceVietnameseChar(filename).Replace(" ", "_");
                    file.MoveTo(strUploadPath + file.FileName.Replace(Path.GetFileNameWithoutExtension(file.FileName), filename));

                }



            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);

            }
        }

        /// <summary>
        /// Create      : Thai.Tran
        /// Date        : 23/11/2011
        /// </summary>
        /// <param name="uploader"></param>
        public override void OnUploaderInit(MvcUploader uploader)
        {
            try
            {
                string sid = (uploader.Context.ApplicationInstance).Request.QueryString["id"];
                id = int.Parse(sid);
            }
            catch
            {

                id = 0;
            }
        }


    }
}