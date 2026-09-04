using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace BIZ
{
    public class CrawlBO
    {
        public string getfirsdom(string dom, string html)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                string result = "";
                foreach (HtmlNode selectNode in doc.DocumentNode.SelectNodes("//" + dom))
                {
                    return selectNode.OuterHtml;
                }
            }
            catch
            {

            }
            return "";
        }
        public string getbyclassout(string id, string dom, string html)
        {

            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                string result = "";

                foreach (HtmlNode selectNode in doc.DocumentNode.SelectNodes("//" + dom + "[@class='" + id + "']"))
                {
                    return selectNode.OuterHtml;
                }
            }
            catch
            {

            }

            return "";
        }
        public string getbyclass(string id, string dom, string html)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                string result = "";

                if (!string.IsNullOrEmpty(id))
                {
                    foreach (HtmlNode selectNode in doc.DocumentNode.SelectNodes("//" + dom + "[@class='" + id + "']"))
                    {
                        return selectNode.InnerHtml;
                    }
                }
                else
                {
                    foreach (HtmlNode selectNode in doc.DocumentNode.SelectNodes("//" + dom))
                    {
                        return selectNode.InnerHtml;
                    }
                }
            }
            catch
            {

            }

            return "";
        }
        public string getattrbyclass(string id, string dom, string attr, string html)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                string result = "";
                foreach (HtmlNode selectNode in doc.DocumentNode.SelectNodes("//" + dom + "[@class='" + id + "']"))
                {
                    var firstOrDefault = selectNode.Attributes.FirstOrDefault(x => x.Name == attr);
                    if (firstOrDefault != null)
                        return firstOrDefault.Value;
                }
            }
            catch
            {

            }
            return "";
        }
        public string getattr(string dom, string attr, string html)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                string result = "";
                foreach (HtmlNode selectNode in doc.DocumentNode.SelectNodes("//" + dom ))
                {
                    if (string.IsNullOrEmpty(attr))
                    {
                        return selectNode.InnerHtml;
                    }
                    else
                    {
                        var firstOrDefault = selectNode.Attributes.FirstOrDefault(x => x.Name == attr);
                        if (firstOrDefault != null)
                            return firstOrDefault.Value;
                    }
                  
                }
            }
            catch
            {

            }
            return "";
        }
        public string getmetaTag(string property, string html)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                var selectNode = doc.DocumentNode.SelectSingleNode("//meta[@property='" + property + "']");
                return   selectNode.Attributes["content"].Value;
            }
            catch
            {

            }
            return "";
        }
        public string getbyId(string id, string html)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                return doc.GetElementbyId(id).InnerHtml;
            }
            catch
            {

            }
            return "";
        }
        public string GetPage(string url)

        {

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.UserAgent = "Simple crawler";
            WebResponse response = request.GetResponse();

            Stream stream = response.GetResponseStream();

            StreamReader reader = new StreamReader(stream);

            string htmlText = reader.ReadToEnd();
            return htmlText;

        }

    }
}
