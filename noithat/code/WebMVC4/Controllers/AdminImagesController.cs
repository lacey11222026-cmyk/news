
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMVC4.Models;


namespace WebMVC4.Controllers
{
    [Authorize]
    public class AdminImagesController : Controller
    {
        //
        // GET: /Images/
        public ActionResult Index()
        {
            ViewBag.Month = DateTime.Now.Month;
            return View();
        }
        public ActionResult ManageImage()
        {
            ViewBag.Month = DateTime.Now.Month;
            return View();
        }
        public ActionResult ListImage(int? currPage, int Month = 0)
        {
            int CurrPage = 1;
            CurrPage = currPage == null ? 1 : currPage.GetValueOrDefault();
            int pagesize = 15;

            var totalRecord = 0;

            if (Month == 0)
            {
                Month = int.Parse(DateTime.Now.Month.ToString());

            }
            var folder = Request.PhysicalApplicationPath + ConfigurationManager.AppSettings["UploadPath"] + "User\\" + HttpContext.User.Identity.Name + "\\" + DateTime.Now.Year.ToString() + "\\" + Month.ToString() + "\\";

            DirectoryInfo sdir = new DirectoryInfo(folder);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            var lstImgFiles = sdir.GetFiles().OrderByDescending(x => x.CreationTime).ToList();
            totalRecord = lstImgFiles.Count();
            int PageCount = (int)(totalRecord / pagesize);
            if (totalRecord > 0)
            {
                if (totalRecord % pagesize > 0)
                    PageCount += 1;
            }
            ViewBag.PageNumber = currPage;
            ViewBag.PageCount = PageCount;
            ViewBag.Month = Month;
            ViewBag.hdfCurrentFolder = ConfigurationManager.AppSettings["UploadUrl"] + "User/" + HttpContext.User.Identity.Name + "/" + DateTime.Now.Year.ToString() + "/" + Month.ToString() + "/";

            lstImgFiles = lstImgFiles.Skip(pagesize * (CurrPage - 1)).Take(pagesize).ToList();
            return PartialView(lstImgFiles);
        }
        public ActionResult FileInfo()
        {

            return PartialView();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult FileDelete(string filename, string month)
        {
            string results = "0";
            try
            {
                var folder = Request.PhysicalApplicationPath + ConfigurationManager.AppSettings["UploadPath"] + "User\\" + HttpContext.User.Identity.Name + "\\" + DateTime.Now.Year.ToString() + "\\" + month.ToString() + "\\";
                string strfile = folder + filename;
                System.IO.File.Delete(strfile);
                results = "1";
            }
            catch
            {

                results = "0";
            }
            return Json(results);

        }
        public ActionResult SaveUploadedFile()
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                //Save file content goes here
                fName = file.FileName;
                if (file != null && file.ContentLength > 0)
                {

                    //var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\WallImages", Server.MapPath(@"\")));

                    //string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "imagepath");
                    //var pathString = System.Configuration.ConfigurationManager.AppSettings["mediaPath"];
                    //DateTime CrTime = DateTime.Now;
                    //pathString = pathString + CrTime.ToString("yyyy/MM/dd") + "/";
                    var folder = Request.PhysicalApplicationPath + ConfigurationManager.AppSettings["UploadPath"] + "User\\" + HttpContext.User.Identity.Name + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString();

                    var fileName1 = UTILS.Utils.ReplaceVietnameseChar(Path.GetFileNameWithoutExtension(file.FileName)).Replace(" ", "_");
                    //fileName1 = UTILS.Utils.SubString(fileName1, 30);
                    var extend1 = Path.GetExtension(file.FileName);
                    bool isExists = System.IO.Directory.Exists(folder);

                    if (!isExists)
                        System.IO.Directory.CreateDirectory(folder);

                    var path = string.Format("{0}\\{1}", folder, fileName1 + extend1);
                    if (System.IO.File.Exists(path))
                    {
                        path = string.Format("{0}\\{1}", folder, fileName1 + new Random().Next(10000) + extend1);
                    }
                    file.SaveAs(path);

                }

            }

            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }
    }
}
