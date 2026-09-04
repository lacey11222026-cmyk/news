using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Local.Controllers
{
    public class ContactController : Controller
    {
        //
        // GET: /Contact/
        [Authorize(Roles = "Administrator,Contact,Local")]
        public ActionResult Index(string myuploader,int Id=74)
        {
            ViewBag.Id = Id;
            using (var uploader = new CuteWebUI.MvcUploader(System.Web.HttpContext.Current))
            {
                // set value Uploader
                uploader.UploadUrl = Response.ApplyAppPathModifier("~/Post/ContactUpload.ashx?id=" + Id.ToString());
                uploader.Name = "myuploader";
                uploader.AllowedFileExtensions = "*.xlsx,*.txt";
                uploader.ManualStartUpload = true;
                uploader.MaxFilesLimit = 1;
                //uploader.MaxSizeKB = 102400;
                uploader.MultipleFilesUpload = true;
                uploader.InsertButtonID = "uploadbutton";
                uploader.CancelAllMsg = "Ngừng toàn bộ Upload";
                uploader.CancelUploadMsg = "Ngừng Upload";
                //prepair html code for the view
                ViewData["uploaderhtml"] = new HtmlString(uploader.Render());

                //if it's HTTP POST:
                if (!string.IsNullOrEmpty(myuploader))
                {
                    var processedfiles = (from strguid in myuploader.Split('/')
                                          select new Guid(strguid)
                                              into fileguid
                                              select uploader.GetUploadedFile(fileguid)
                                                  into file
                                                  where file != null
                                                  where Path.GetExtension(file.FileName).Equals(".xlsx,.txt")
                                                  select file.FileName).ToList();

                    if (processedfiles.Count > 0)
                    {
                        ViewData["UploadedMessage"] = string.Join(",", processedfiles.ToArray()) + " đã upload thành công";
                    }
                }

            }

            return View();
        }
        public ActionResult AddEdit(int Id = 0)
        {
            ViewBag.Id = Id;
            
            return View();
        }

    }
}
