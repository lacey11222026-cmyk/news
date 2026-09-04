using System;
using System.Web;
using System.Web.Services;
using BIZ;
using UTILS;

namespace WebMVC4.Get
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ArticleService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                // get current page and record per page
                string method = context.Request["__m"];
                var pageIndex = context.Request["_pi"];
                var pageSize = context.Request["_ps"];
                if (!Utils.IsNumber(pageIndex))
                    pageIndex = "1";
                if (!Utils.IsNumber(pageSize))
                    pageSize = "10";
                switch (method.ToLower())
                {
                    case "get_article":
                        string id = context.Request["_id"];
                        if (!Utils.IsNumber(id))
                        {
                            context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
                            return;
                        }
                        var content = new ContentBO().GetContentFull(Convert.ToInt32(id));
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(content, string.Empty) + ")");
                        return;

                    case "get_crawl":
                        string url = context.Request["url"];
                        //var domain = Utils.GetUrlRootOfLink(url);
                        var domain = "";
                        var crawlctl = new CrawlBO();
                        var webcontent = crawlctl.GetPage(url);
                        var newsdes = "";
                        var newstitle = "";
                        var newscontent = "";
                        var newstime = "";
                        var newimages = "";
                        var remove1 = "";
                        var newimageshtml = "";

                        newstitle = HttpUtility.HtmlEncode( crawlctl.getmetaTag("og:title", webcontent));
                        newsdes = crawlctl.getmetaTag("og:description", webcontent);
                        newimages = crawlctl.getmetaTag("og:image", webcontent);
                        //switch (domain)
                        //{
                        //    case "http://congnghiepcongnghecao.com.vn":
                        //    case "http://congnghiepcongnghecao.vn":
                        //    case "http://tietkiemnangluong.com.vn":
                        //        newsdes = crawlctl.getbyId("introNews", webcontent);
                        //        newstitle = crawlctl.getbyId("newsTitle", webcontent);
                        //        newscontent = crawlctl.getbyId("NewsContent", webcontent);
                        //        newscontent = newscontent.Replace("../../..", domain);
                        //        newimages = crawlctl.getbyId("igmNews", webcontent);
                        //        newstime = crawlctl.getbyId("newstime", webcontent);
                        //        break;
                        //    case "http://nscl.vn":
                        //        newstitle = crawlctl.getbyclass("post-head", "h1", webcontent);
                        //        newscontent = crawlctl.getbyclass("text-detail", "div", webcontent);
                        //        newimages = crawlctl.getattrbyclass("img-thumbnail thumb-0 wp-post-image", "img", "src", webcontent);
                        //        newsdes = crawlctl.getfirsdom("strong", newscontent).Replace("<strong>", "").Replace("</strong>", "");
                        //        newscontent = newscontent.Replace(newsdes, "");

                        //        newimageshtml = "<div style='text-align:center;'>" + crawlctl.getbyclass("wp-caption featured", "div", webcontent) + "</div>";
                        //        newscontent = newimageshtml + newscontent;

                        //        newstime = crawlctl.getbyclass("txt", "span", webcontent);
                        //        newstime = newstime.Replace("Ngày đăng: ", "");
                        //        break;
                        //    case "http://tapchicongthuong.vn":
                        //        newstitle = crawlctl.getmetaTag("og:title", webcontent);
                        //        newscontent = crawlctl.getbyclass("left-bodydetail", "div", webcontent);
                        //        newsdes = crawlctl.getmetaTag("og:description", webcontent);
                        //        newimages = crawlctl.getmetaTag("og:image", webcontent);
                        //        remove1 = new CrawlBO().getbyclassout("tukhoa", "div", newscontent);
                        //        if (!string.IsNullOrEmpty(remove1))
                        //            newscontent = newscontent.Replace(remove1, "");

                        //        newstime = crawlctl.getbyclass("date-detail", "span", webcontent).Split(',')[1].TrimStart();
                        //        newstime = newstime.Substring(0, 10);
                        //        break;
                        //    case "http://baocongthuong.com.vn":
                        //        newstitle = crawlctl.getmetaTag("og:title", webcontent).Split('|')[0];
                        //        newscontent = crawlctl.getbyclass("content", "div", webcontent);
                        //        newsdes = crawlctl.getmetaTag("og:description", webcontent);
                        //        newimages = crawlctl.getmetaTag("og:image", webcontent);

                        //        remove1 = crawlctl.getbyclassout("__MB_ARTICLE_A", "table", newscontent);
                        //        if (!string.IsNullOrEmpty(remove1))
                        //            newscontent = newscontent.Replace(remove1, "");

                        //        newstime = crawlctl.getbyclass("", "time", webcontent);
                        //        newstime = newstime.Split('|')[1].TrimStart();
                        //        break;
                        //    case "http://eprotech.vn":
                        //        newstitle = crawlctl.getmetaTag("og:title", webcontent);
                        //        newscontent = crawlctl.getbyclass("content", "div", webcontent);

                        //        newimageshtml = "<div style='text-align:center;'>" + crawlctl.getbyclass("avatar", "div", webcontent) + "</div>";
                        //        newscontent = newimageshtml + newscontent;

                        //        newsdes = crawlctl.getbyclass("teaser", "div", webcontent).Replace("</p>", "").Replace("<p style=\"text-align: justify;\">", "").TrimStart();
                        //        newsdes = HttpUtility.HtmlDecode(newsdes);

                        //        newimages = crawlctl.getmetaTag("og:image", webcontent);
                        //        newimages = newimages.Replace("http://eprotech.vnhttp://eprotech.vn",
                        //            "http://eprotech.vn");

                        //        newstime = crawlctl.getbyclass("xdate", "span", webcontent).Split(':')[1].TrimStart();
                        //        newstime = newstime.Substring(0, 10);
                        //        break;
                        //    case "http://support.gov.vn":

                        //        var titlehtml= crawlctl.getbyclass("newdetail_title", "div", webcontent);
                        //        newstitle = crawlctl.getattr("a", "title", titlehtml);

                        //        var deshtml = crawlctl.getbyclass("newdetailtomtat", "div", webcontent);
                        //        webcontent = webcontent.Replace(deshtml, "");
                        //        //newsdes = crawlctl.getbyclass("content-inner", "div", deshtml).Replace("<p><b>","").Replace("</b></p>", "");
                        //        newsdes =  crawlctl.getattr("b", "", deshtml);
                        //        newimages = domain+crawlctl.getattr("img", "src", deshtml);
                        //        newscontent = crawlctl.getbyclass("content-inner", "div", webcontent);
                        //        newscontent = newscontent.Replace("/images", domain+"/images");

                        //        var timehtml = crawlctl.getbyclass("solandoctin", "div", webcontent);
                        //        newstime = crawlctl.getattr("span", "", timehtml).Replace("-","/");
                        //        break;
                        //    case "https://www.most.gov.vn":
                        //        newstitle = crawlctl.getbyclass("News_Detail_Title", "h1", webcontent).TrimStart(); ;
                        //        newscontent = crawlctl.getbyId("divArticleDescription2", webcontent);
                        //        //newimages = crawlctl.getattrbyclass("img-thumbnail thumb-0 wp-post-image", "img", "src", webcontent);
                        //        newsdes = crawlctl.getbyId("divArticleDescription1", webcontent).TrimStart();


                        //        newimages = domain + crawlctl.getattr("img", "src", newscontent);
                        //        newscontent = newscontent.Replace("/Images/", domain+ "/Images/");

                        //        newstime = crawlctl.getbyclass("News_Time_Post", "span", webcontent);
                        //        newstime = newstime.Split(',')[1].TrimStart().Replace("&nbsp;", "").Substring(0,10);
                        //        break;
                        //    case "https://vnexpress.net":
                        //        newstitle = crawlctl.getmetaTag("og:title", webcontent);
                        //        newscontent = crawlctl.getbyclass("content_detail fck_detail width_common block_ads_connect", "article", webcontent);
                        //        newsdes = crawlctl.getmetaTag("og:description", webcontent);
                        //        newimages = crawlctl.getmetaTag("og:image", webcontent);

                        //        newstime = crawlctl.getbyclass("time left", "span", webcontent);
                        //        newstime = newstime.Split(',')[1].TrimStart().Replace("&nbsp;", "").Substring(0, 10).Replace("<","");
                        //        break;
                        //}

                        var contentcrawl = new BIZ.Entity.CONTENT_FULL();
                        contentcrawl.Title =  newstitle;
                        contentcrawl.IntroText = newsdes;
                        //contentcrawl.Contents = newscontent;
                        contentcrawl.Thumbnail = newimages;
                        //contentcrawl.Params = newstime;
                        //contentcrawl.Alias = HttpContext.Current.User.Identity.Name;
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(contentcrawl, string.Empty) + ")");
                        return;
                    case "get_article_byids":
                        string lstid = context.Request["_lstid"];
                        if (string.IsNullOrEmpty(lstid))
                        {
                            context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
                            return;
                        }
                        var lstcontent = new ContentBO().GetTopContentByIdsFulls(lstid, 0, true);
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(lstcontent, string.Empty) + ")");
                        return;
                    case "get_all_articles_paged":

                        var json = new ContentBO().GetAllContentsPaged_JSON(Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize));
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + json + ")");
                        return;
                    case "get_filter_articles":

                        string contentTitle = context.Request["tit"];
                        //string groupId = context.Request ["gid"];
                        string categoryId = context.Request["cid"];
                        string type = context.Request["tid"];

                        var createdby = String.Empty;

                        if (type == "1" || type == "3")
                        {
                            createdby = context.Request["createdby"];

                        }
                        var lang = "";
                        if (categoryId == "1001")
                        {
                            lang = "en-us";
                            categoryId = "-1";
                        }
                           
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + new ContentBO().GetFilterContentsPaged_JSON(Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), contentTitle, Convert.ToInt32(categoryId), Convert.ToInt32(type), createdby,"","",lang) + ")");
                        return;
                }
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "ArticleServiceeGet", "ArticleService");
                context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
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