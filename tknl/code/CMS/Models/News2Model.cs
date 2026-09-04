using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Models
{
    public class News2Model
    {
        public int CategoryId { get; set; }
        public List<CONTENT_FULL> articles { get; set; }
        public List<CONTENT_FULL> hotnews { get; set; }
        public List<CATEGORY_FULL> listdata { get; set; }
        public int pageIndex { get; set; }
    }
}