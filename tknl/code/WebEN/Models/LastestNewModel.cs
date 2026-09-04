using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIZ.Entity;

namespace WebEN.Models
{
    public class LastestNewModel
    {
        public List<CONTENT_FULL> lstdata { get; set; }
        public int CategoryId { get; set; }
        public string HeaderTitle { get; set; }
        public string Url { get; set; }
        public string Css { get; set; }
    }
}