using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DATA;

namespace BIZ.Entity
{
    public class FileUserFull : DATA.FileUser
    {
        public FileUser ConvertToBase()
        {

            FileUser content = new FileUser();
            content.Id = Id;
            content.UserName = UserName;
            content.FileName = FileName;
            content.Keyword = Keyword;
            content.CreateTime = CreateTime;
            return content;
        }
        public string FilePath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UploadUrl"] +"/User/"+ UserName + "/" + CreateTime.Year +
                       "/" + CreateTime.Month + "/" + CreateTime.Day + "/" + FileName;
            }
        }
        public string Icon
        {
            get
            {
                string imgrenurl = "";
                switch (FileType.ToString())
                {
                    case ".jpg":
                        imgrenurl = FilePath;
                        break;
                    case ".doc":
                        imgrenurl = "/Administrator/images/Icon/doc.jpg";
                        break;
                    case ".docx":
                        imgrenurl = "/Administrator/images/Icon/doc.jpg";
                        break;
                    case ".rar":
                        imgrenurl = "/Administrator/images/Icon/rar.jpg";
                        break;
                    case ".zip":
                        imgrenurl = "/Administrator/images/Icon/rar.jpg";
                        break;
                    case ".xls":
                        imgrenurl = "/Administrator/images/Icon/exel.jpg";
                        break;
                    case ".xlsx":
                        imgrenurl = "/Administrator/images/Icon/exel.jpg";
                        break;
                    case ".ppt":
                        imgrenurl = "/Administrator/images/Icon/ppt.jpg";
                        break;
                    case ".swf":
                        imgrenurl = "/Administrator/images/Icon/flv.jpg";
                        break;
                    case ".flv":
                        imgrenurl = "/Administrator/images/Icon/flv.jpg";
                        break;
                    case ".pdf":
                        imgrenurl = "/Administrator/images/Icon/pdf.jpg";
                        break;
                    case ".mp3":
                        imgrenurl = "/Administrator/images/Icon/media.jpg";
                        break;
                    case ".avi":
                        imgrenurl = "/Administrator/images/Icon/media.jpg";
                        break;
                    case ".mp4":
                        imgrenurl = "/Administrator/images/Icon/media.jpg";
                        break;
                    Default:
                        imgrenurl = "/Administrator/images/Icon/default.jpg";
                        break;
                }
                return imgrenurl;
            }
           
        }
        public string FileType
        {
            get
            {


                var lisext = FileName.Trim().Split('.');
                var ext = "." + lisext[lisext.Length - 1].ToString();
                return ext;

            }
        }
    }
}
