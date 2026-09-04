using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DATA;

namespace WebMVC4.Models
{
    public class SurveyItemModel
    {
        public List<SurveyItem> listdata { get; set; }
        public Survey obj { get; set; }
        public string  data { get; set; }
        public string cate { get; set; }
    }
}