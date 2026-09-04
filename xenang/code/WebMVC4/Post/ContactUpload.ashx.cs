using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using BIZ;
using CuteWebUI;
using DATA;
using UTILS;

namespace WebMVC4.Post
{
    /// <summary>
    /// Summary description for ContactUpload
    /// </summary>
    public class ContactUpload : MvcHandler
    {
        public int id = 0;
        //public string accountname = "xxx";
        public override UploaderValidateOption GetValidateOption()
        {
            var option = new UploaderValidateOption
            {
                MaxSizeKB = 200 * 1024,
                AllowedFileExtensions = "*.xlsx,*.txt"
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
                    var strUploadPath = HttpContext.Current.Request.PhysicalApplicationPath + ConfigurationManager.AppSettings["UploadPath"] + "User\\" + HttpContext.Current.User.Identity.Name + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString() + "\\";
                    if (Path.GetExtension(file.FileName).ToLower().Equals(".xlsx"))
                    {

                        if (!Directory.Exists(strUploadPath)) { Directory.CreateDirectory(strUploadPath); }
                        var nameupload = DateTime.Now.ToString("yyMMddHHmmss");
                        file.MoveTo(strUploadPath + file.FileName.Replace(Path.GetFileNameWithoutExtension(file.FileName), nameupload));

                        string soure = strUploadPath + nameupload + ".xlsx";
                        string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + soure + ";Extended Properties=Excel 12.0";
                        // Create the connection object
                        OleDbConnection oledbConn = new OleDbConnection(connString);
                        try
                        {
                            // Open connection
                            oledbConn.Open();

                            // Create OleDbCommand object and select data from worksheet Sheet1
                            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn);

                            // Create new OleDbDataAdapter
                            OleDbDataAdapter oleda = new OleDbDataAdapter();

                            oleda.SelectCommand = cmd;

                            // Create a DataSet which will hold the data extracted from the worksheet.
                            DataSet ds = new DataSet();

                            // Fill the DataSet from the data extracted from the worksheet.
                            oleda.Fill(ds, "Code");
                            var objCate = new CategoryBO().GetCategoryFull(id);
                            var obj = new Contact { CategoryId = id, CategoryPathway = objCate.Pathway,Published = 1};
                            var listdata = ds.Tables[0].Rows;
                            foreach (DataRow row in listdata)
                            {
                                try
                                {
                                   if(row[2].ToString().Contains("@"))
                                   {
                                        obj.Name = row[0].ToString().Trim();
                                        obj.Role = row[1].ToString().Trim();
                                        obj.Mail = row[2].ToString().Trim();
                                        obj.Mobile = row[3].ToString().Trim();
                                       if(obj.Mobile[0]!='0')
                                       {
                                            obj.Mobile="0"+obj.Mobile;
                                       }
                                       obj.Mobile = obj.Mobile.Replace(" ", "");

                                    new ContactBO().CreateUpdateContact(obj);

                                   }
                                   
                                }
                                catch
                                {

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            ExHandler.Handle(ex, "GiftCodeUpload", "OnFileUploaded , file=" + file.FileName);
                        }
                        finally
                        {
                            // Close connection
                            oledbConn.Close();
                        }
                        file.Delete();
                    }
                   
                }

            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "GiftCodeUpload", "OnFileUploaded , file=" + file.FileName);

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
                //accountname = (uploader.Context.ApplicationInstance).Request.QueryString["accountname"];
                id = int.Parse(sid);
            }
            catch
            {

                id = 0;
            }
        }


    }
}