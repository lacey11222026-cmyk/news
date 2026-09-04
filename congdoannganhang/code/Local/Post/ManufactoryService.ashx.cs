using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using BIZ;
using BIZ.Entity;
using UTILS;

namespace Local.Post
{
    /// <summary>
    /// Summary description for ManufactoryService
    /// </summary>
    public class ManufactoryService: IHttpHandler
    {

        private delegate string DelegateDeleteImages ( HttpRequest request, int id, string entityName );

        public void ProcessRequest ( HttpContext context )
        {
            context.Response.ContentType = "text/plain";
            ResponseMsg responseMsg = new ResponseMsg ();
            ManufactoryBO manufactoryBo = new ManufactoryBO ();
            try
            {

                if (!HttpContext.Current.User.IsInRole("Administrator") && !HttpContext.Current.User.IsInRole("Sale"))
                {
                    responseMsg.Success = false;
                    responseMsg.Text = "Không có quyền";
                    context.Response.Write(responseMsg.ToJsonString());
                    return;
                }
                string method = context.Request ["__m"];
                int return_val = 0;
                switch ( method.ToLower () )
                {
                    case "save":

                        var manufactoryId = context.Request.Form ["id"];
                        var title = HttpUtility.UrlDecode ( context.Request.Form ["title"] );
                        var website = HttpUtility.UrlDecode(context.Request.Form["website"]);
                        var description = HttpUtility.UrlDecode(context.Request.Form["description"]);
                        var published = context.Request.Form ["published"];
                        //  var ordering = context.Request.Form ["ordering"];

                        if ( !Utils.IsNumber ( manufactoryId ) )
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình lấy thông tin";
                            context.Response.Write ( responseMsg.ToJsonString () );
                            return;
                        }

                        var manufactory = new MANUFACTORY_FULL ();

                        if ( Convert.ToInt32 ( manufactoryId ) > 0 )
                        {
                            manufactory = manufactoryBo.GetManufactoryFull ( Convert.ToInt32 ( manufactoryId ) );
                        }

                        manufactory.Id = Convert.ToInt32 ( manufactoryId );
                        manufactory.Title = HttpUtility.UrlDecode (title);
                        manufactory.Description = HttpUtility.UrlDecode(description);
                        manufactory.Website = HttpUtility.UrlDecode (website);
                        manufactory.Published = Convert.ToByte ( published );
                        //manufactory.Ordering = Convert.ToByte ( ordering );

                       


                        return_val = manufactoryBo.CreateUpdateManufactory ( manufactory );

                        if ( return_val != -1 )
                        {
                            responseMsg.Success = true;
                            responseMsg.Value = Convert.ToString ( return_val );
                            responseMsg.Text = "Lưu thông tin thành công";
                        }
                        else
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình xử lý";
                        }

                        context.Response.Write ( responseMsg.ToJsonString () );
                        return;

                    case "delete":
                        var arrManufactoryId = context.Request.Form.GetValues ( "id[]" );
                        if ( arrManufactoryId == null || arrManufactoryId.Count () == 0 )
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Bạn phải chọn ít nhất 1 nhà sản xuất để xóa";
                            context.Response.Write ( responseMsg.ToJsonString () );
                            return;
                        }

                        if ( arrManufactoryId.Count () == 1 )
                        {
                            return_val = manufactoryBo.DeleteManufactory ( Convert.ToInt32 ( arrManufactoryId [0] ) );
                            //if ( return_val != -1 )
                            //{
                            //      var type = ConfigurationManager.AppSettings["EnableFTP"];

                            //      if (type == "1")
                            //      {
                            //          DelegateDeleteImages delegateDeleteImages = Utils.DeleteFilesFTP;
                            //          delegateDeleteImages.BeginInvoke(context.Request, Convert.ToInt32(arrManufactoryId[0]), "Manufactory", null, null);
                            //      }
                            //      else
                            //      {
                            //          DelegateDeleteImages delegateDeleteImages = Utils.DeleteFiles;
                            //          delegateDeleteImages.BeginInvoke(context.Request, Convert.ToInt32(arrManufactoryId[0]), "Manufactory", null, null);
                            //      }
                                
                            //}
                        }
                        else
                        {
                            string joinId = string.Empty;
                            foreach ( var id in arrManufactoryId )
                            {
                                if ( Utils.IsNumber ( id ) )
                                    joinId += "," + id;
                            }

                            if ( string.IsNullOrEmpty ( joinId ) )
                            {
                                responseMsg.Success = false;
                                responseMsg.Text = "Bạn phải chọn ít nhất 1 nhà sản xuất để xóa";
                                context.Response.Write ( responseMsg.ToJsonString () );
                                return;
                            }

                            joinId = joinId.TrimStart ( ',' );
                            return_val = manufactoryBo.DeleteManufactories ( joinId );
                            if ( return_val != -1 )
                            {
                                //DelegateDeleteImages delegateDeleteImages = Utils.DeleteFiles;
                                //string [] arrJoinId = joinId.Split ( ',' );
                                //foreach ( var joinid in arrJoinId )
                                //{
                                //    delegateDeleteImages.BeginInvoke ( context.Request, Convert.ToInt32 ( joinid ), "Manufactory", null, null );
                                //}

                            }
                        }

                        if ( return_val != -1 )
                        {
                            responseMsg.Success = true;
                            responseMsg.Text = "Xóa nhà sản xuất thành công";
                        }
                        else
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình xử lý : Có thể bạn chưa xóa hết các phần liên quan";
                        }

                        context.Response.Write ( responseMsg.ToJsonString () );
                        return;


                }
            }
            catch ( Exception ex )
            {
                ExHandler.Handle(ex, "ManufactoryServicePost", "ManufactoryService");
                responseMsg.Success = false;
                responseMsg.Text = "Có lỗi trong quá trình xử lý - execption";
                context.Response.Write ( responseMsg.ToJsonString () );
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