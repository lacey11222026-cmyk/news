using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIZ.Entity;

namespace WebEN.Models
{
    public class NewsModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }
        public int pageSize { get; set; }
        public List<CONTENT_FULL> listdata { get; set; }
        public int pageIndex { get; set; }
    }
}