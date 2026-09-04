using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using BIZ;
using UTILS;

namespace Local.Get
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
                        string url = context.Request["_url"];
                        strBuilder = new StringBuilder();
                        // divided 1000000 files in folder               
                        strBuilder.Append(context.Request.PhysicalApplicationPath).Append(ConfigurationManager.AppSettings["UploadPath"]).Append(url).Append("\\");
                        upload_path = strBuilder.ToString();

                        // if folder not exist => create folder follow rule
                        if (!Directory.Exists(upload_path))
                        {
                            context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
                            return;
                        }
                        // get all file in avarta of club 
                        DirectoryInfo di = new DirectoryInfo(upload_path);
                        FileSystemInfo[] files = di.GetFileSystemInfos();
                        var orderedFiles = files.OrderByDescending(f => f.CreationTime).Where(x => x.Name.Contains(".jpg")).Select(x=>x.Name).Take(30).ToList();
                        
                        //fileList = (from file in Directory.GetFiles(upload_path) select file.Replace(upload_path, "").Trim()).ToList();
                        //fileList = fileList.Where(x => x.Contains(".jpg")).ToList() ;  
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(orderedFiles, string.Empty) + ")");
                        return;
                    case "article":

                        strBuilder = new StringBuilder ();
                        // divided 1000000 files in folder               
                        strBuilder.Append(context.Request.PhysicalApplicationPath).Append(ConfigurationManager.AppSettings["UploadPath"]).Append("Article").Append("\\").Append(Convert.ToInt32(id) / 100000).Append("\\").Append(Convert.ToInt32(id) / 100).Append("\\").Append(id).Append("\\");
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
                        strBuilder.Append ( context.Request.PhysicalApplicationPath ).Append ( ConfigurationManager.AppSettings ["UploadPath"] ).Append ( "Product" ).Append ( "\\" ).Append ( Convert.ToInt32 ( id ) / 100000 ).Append ( "\\" ).Append ( Convert.ToInt32 ( id ) / 100 ).Append ( "\\" ).Append ( id ).Append ( "\\" );

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
                        strBuilder.Append ( context.Request.PhysicalApplicationPath ).Append ( ConfigurationManager.AppSettings ["UploadPath"] ).Append ( "Category" ).Append ( "\\" ).Append ( Convert.ToInt32 ( id ) / 100000 ).Append ( "\\" ).Append ( Convert.ToInt32 ( id ) / 100 ).Append ( "\\" ).Append ( id ).Append ( "\\" );

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
                    case "manufactory":

                        strBuilder = new StringBuilder ();
                        // divided 1000000 files in folder               
                        strBuilder.Append ( context.Request.PhysicalApplicationPath ).Append ( ConfigurationManager.AppSettings ["UploadPath"] ).Append ( "Manufactory" ).Append ( "\\" ).Append ( Convert.ToInt32 ( id ) / 100000 ).Append ( "\\" ).Append ( Convert.ToInt32 ( id ) / 100 ).Append ( "\\" ).Append ( id ).Append ( "\\" );

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