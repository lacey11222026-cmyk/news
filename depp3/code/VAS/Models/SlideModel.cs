using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIZ.Entity;

namespace VAS.Models
{
    public class SlideModel
    {
        public List<CONTENT_FULL> LstHotNews { get; set; }
        public List<CONTENT_FULL> LstLastestNews { get; set; }
        public List<CONTENT_FULL> LstTopViewNews { get; set; }

        public List<CONTENT_FULL> LstTopViewNews2 { get; set; }
    }
}