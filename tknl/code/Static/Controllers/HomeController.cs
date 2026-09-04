using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using UTILS;

namespace Static.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Error()
        {
            var context = System.Web.HttpContext.Current;

          
            var requestpage = HttpUtility.UrlDecode(context.Request.ServerVariables["QUERY_STRING"].Replace("404;", ""));
            ViewBag.requestpage = requestpage;
            
            if (requestpage.Contains("Upload"))
            {
                requestpage = requestpage.Substring(requestpage.IndexOf("Images"));
                var arequestpage = requestpage.Split("/".ToCharArray());
                string url;
                try
                {
                    var atargetfile = arequestpage[arequestpage.Length - 1].Split(".".ToCharArray());
                    int sourceId;
                    if (!int.TryParse(arequestpage[arequestpage.Length - 2], out sourceId))
                    {
                    }
                    var w = atargetfile[atargetfile.Length - 3];
                    var h = atargetfile[atargetfile.Length - 2];
                    var f = "";
                    for (var i = 0; i < arequestpage.Length - 1; i++)
                    {
                        f += arequestpage[i] + "/";
                    }
                    for (var i = 0; i < atargetfile.Length - 3; i++)
                    {
                        f += atargetfile[i];
                        if (i < atargetfile.Length - 4) f += ".";
                    }
                    f = f.Replace("/", "\\");
                    try
                    {
                        Convert.ToInt32(w);
                        Convert.ToInt32(h);
                        if (!MvcApplication.ImageWidth.Contains("," + w + ",") || !MvcApplication.ImageHeight.Contains("," + h + ","))
                        {
                            ExHandler.Handle(new Exception(MvcApplication.ImageWidth + " Height: " + MvcApplication.ImageWidth), "404.html");
                            //Console.Write(MvcApplication.ImageWidth);
                            url = "/images/upload/no_image.jpg";
                        }
                        else
                        {
                            url = "/srv_thumb.ashx?source=" + sourceId + "&w=" + w + "&h=" + h + "&f=" + HttpUtility.UrlEncode(f);
                        }

                    }
                    catch (Exception ex)
                    {
                        ExHandler.Handle(ex, "404.html");
                        url = "/images/upload/no_image.jpg";
                    }

                }
                catch (Exception ex)
                {
                    ExHandler.Handle(ex, "404.html");
                    url = "/images/upload/no_image.jpg";
                }
                return Redirect(url);
            }
            else
            {
                if (requestpage.Contains("eepmedia"))
                {
                    requestpage = requestpage.Substring(requestpage.IndexOf("eepmedia"));
                    var arequestpage = requestpage.Split("/".ToCharArray());
                    string url;
                    try
                    {
                        var atargetfile = arequestpage[arequestpage.Length - 1].Split(".".ToCharArray());
                        int sourceId;
                        if (!int.TryParse(arequestpage[arequestpage.Length - 2], out sourceId))
                        {
                        }
                        var w = atargetfile[atargetfile.Length - 3];
                        var h = atargetfile[atargetfile.Length - 2];
                        var f = "";
                        for (var i = 0; i < arequestpage.Length - 1; i++)
                        {
                            f += arequestpage[i] + "/";
                        }
                        for (var i = 0; i < atargetfile.Length - 3; i++)
                        {
                            f += atargetfile[i];
                            if (i < atargetfile.Length - 4) f += ".";
                        }
                        f = f.Replace("/", "\\");
                        try
                        {
                            Convert.ToInt32(w);
                            Convert.ToInt32(h);
                            if (!MvcApplication.ImageWidth.Contains("," + w + ",") || !MvcApplication.ImageHeight.Contains("," + h + ","))
                            {
                                ExHandler.Handle(new Exception(MvcApplication.ImageWidth + " Height: " + MvcApplication.ImageWidth), "404.html");
                                //Console.Write(MvcApplication.ImageWidth);
                                url = "/images/upload/no_image.jpg";
                            }
                            else
                            {
                                url = "/srv_thumb.ashx?source=" + sourceId + "&w=" + w + "&h=" + h + "&f=" + HttpUtility.UrlEncode(f);
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            ExHandler.Handle(ex, "404.html");
                            url = "/images/upload/no_image.jpg";
                        }

                    }
                    catch (Exception ex)
                    {
                        ExHandler.Handle(ex, "404.html");
                        url = "/images/upload/no_image.jpg";
                    }
                    return Redirect(url);
                }

            }
            return View();
        }

    }
}
