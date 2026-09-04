using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DATA;

namespace CMS.Models
{
    public class SurveyItemModel
    {
        public List<SurveyItem> listdata { get; set; }
        public Survey obj { get; set; }
        public string  data { get; set; }
    }
}