using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using BIZ;
using UTILS;

namespace CMS.Get
{
    /// <summary>
    /// Summary description for ImageService
    /// </summary>
    public class ImageService: IHttpHandler
    {

        public void ProcessRequest ( HttpContext context )
        {
            context.Response.ContentType = "text/plain";
            try
            {
                // get current page and record per page
                string method = context.Request ["__m"];
                StringBuilder strBuilder;
                string upload_path;
                List<string> fileList;
                string id;
                id = context.Request ["_id"];
                if ( !Utils.IsNumber ( id ) )
                {
                    context.Response.Write ( context.Request.Params ["jsoncallback"] + "(null)" );
                    return;
                }

                switch ( method.ToLower () )
                {
                    case "tinycme":
                        string fromdate = context.Request["_fromdate"];
                        string todate = context.Request["_todate"];
                        string author = context.Request["_author"];
                        string title = context.Request["_title"];
                        //fileList = (from file in Directory.GetFiles(upload_path) select file.Replace(upload_path, "").Trim()).ToList();
                        //fileList = fileList.Where(x => x.Contains(".jpg")).ToList() ;  
                        var lstImgFiles = new FileUserBO().GetFileUsersByFilter(100, title, ".jpg", author, fromdate, todate);
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(lstImgFiles, string.Empty) + ")");
                        return;
                    case "article":

                        strBuilder = new StringBuilder ();
                        // divided 1000000 files in folder               
                        strBuilder.Append(ConfigurationManager.AppSettings["UploadPath"]).Append("Article").Append("\\").Append(Convert.ToInt32(id) / 100000).Append("\\").Append(Convert.ToInt32(id) / 100).Append("\\").Append(id).Append("\\");
                        upload_path = strBuilder.ToString ();

                        // if folder not exist => create folder follow rule
                        if ( !Directory.Exists ( upload_path ) )
                        {
                            context.Response.Write ( context.Request.Params ["jsoncallback"] + "(null)" );
                            return;
                        }
                        // get all file in avarta of club 
                        fileList = ( from file in Directory.GetFiles ( upload_path ) select file.Replace ( upload_path, "" ).Trim () ).ToList ();
                        fileList = fileList.Where(x => x.Split('.').Length == 2).ToList(); 
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + Utils.ConvertToJson ( fileList, string.Empty ) + ")" );
                        return;

                    case "product":

                        strBuilder = new StringBuilder ();
                        // divided 1000000 files in folder               
                        strBuilder.Append ( ConfigurationManager.AppSettings ["UploadPath"] ).Append ( "Product" ).Append ( "\\" ).Append ( Convert.ToInt32 ( id ) / 100000 ).Append ( "\\" ).Append ( Convert.ToInt32 ( id ) / 100 ).Append ( "\\" ).Append ( id ).Append ( "\\" );

                        upload_path = strBuilder.ToString ();

                        // if folder not exist => create folder follow rule
                        if ( !Directory.Exists ( upload_path ) )
                        {
                            context.Response.Write ( context.Request.Params ["jsoncallback"] + "(null)" );
                            return;
                        }
                        // get all file in avarta of club 
                        fileList = ( from file in Directory.GetFiles ( upload_path ) select file.Replace ( upload_path, "" ).Trim () ).ToList ();
                        fileList = fileList.Where(x => x.Split('.').Length == 2).ToList(); 
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + Utils.ConvertToJson ( fileList, string.Empty ) + ")" );
                        return;

                    case "album":

                        strBuilder = new StringBuilder();
                        // divided 1000000 files in folder               
                        strBuilder.Append(context.Request.PhysicalApplicationPath).Append(ConfigurationManager.AppSettings["UploadPath"]).Append("Album").Append("\\").Append(Convert.ToInt32(id) / 100000).Append("\\").Append(Convert.ToInt32(id) / 100).Append("\\").Append(id).Append("\\");

                        upload_path = strBuilder.ToString();

                        // if folder not exist => create folder follow rule
                        if (!Directory.Exists(upload_path))
                        {
                            context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
                            return;
                        }
                        // get all file in avarta of club 
                        fileList = (from file in Directory.GetFiles(upload_path) select file.Replace(upload_path, "").Trim()).ToList();
                        fileList = fileList.Where(x => x.Split('.').Length ==2).ToList() ; 
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(fileList, string.Empty) + ")");
                        return;
                    case "category":

                        strBuilder = new StringBuilder ();
                        // divided 1000000 files in folder               
                        strBuilder.Append ( ConfigurationManager.AppSettings ["UploadPath"] ).Append ( "Category" ).Append ( "\\" ).Append ( Convert.ToInt32 ( id ) / 100000 ).Append ( "\\" ).Append ( Convert.ToInt32 ( id ) / 100 ).Append ( "\\" ).Append ( id ).Append ( "\\" );

                        upload_path = strBuilder.ToString ();

                        // if folder not exist => create folder follow rule
                        if ( !Directory.Exists ( upload_path ) )
                        {
                            context.Response.Write ( context.Request.Params ["jsoncallback"] + "(null)" );
                            return;
                        }
                        // get all file in avarta of club 
                        fileList = ( from file in Directory.GetFiles ( upload_path ) select file.Replace ( upload_path, "" ).Trim () ).ToList ();
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + Utils.ConvertToJson ( fileList, string.Empty ) + ")" );
                        return;
                    case "channel":

                        strBuilder = new StringBuilder ();
                        // divided 1000000 files in folder               
                        strBuilder.Append(ConfigurationManager.AppSettings["UploadPath"]).Append("Channel").Append("\\").Append(Convert.ToInt32(id) / 100000).Append("\\").Append(Convert.ToInt32(id) / 100).Append("\\").Append(id).Append("\\");

                        upload_path = strBuilder.ToString ();

                        // if folder not exist => create folder follow rule
                        if ( !Directory.Exists ( upload_path ) )
                        {
                            context.Response.Write ( context.Request.Params ["jsoncallback"] + "(null)" );
                            return;
                        }
                        // get all file in avarta of club 
                        // get all file in avarta of club 
                        fileList = (from file in Directory.GetFiles(upload_path) select file.Replace(upload_path, "").Trim()).ToList();
                        fileList = fileList.Where(x => x.Split('.').Length == 2).ToList();
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(fileList, string.Empty) + ")");
                        return;
                }
            }
            catch ( Exception ex )
            {
                ExHandler.Handle(ex, "ImageServiceGet", "ImageService");
                context.Response.Write ( context.Request.Params ["jsoncallback"] + "(null)" );
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