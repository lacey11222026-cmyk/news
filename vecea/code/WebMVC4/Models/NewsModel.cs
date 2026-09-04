using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIZ.Entity;

namespace WebMVC4.Models
{
    public class NewsModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }
        public int pageSize { get; set; }
        public List<CONTENT_FULL> listdata { get; set; }

        public List<CONTENT_FULL> hotnews { get; set; }
        public int pageIndex { get; set; }

        public int Id { get; set; }
    }
    public class LastestNewModel
    {
        public List<CONTENT_FULL> lstdata { get; set; }
        public int CategoryId { get; set; }
        public string HeaderTitle { get; set; }
        public string Url { get; set; }
        public string Css { get; set; }
    }
}