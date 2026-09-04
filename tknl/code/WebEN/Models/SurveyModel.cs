using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DATA;

namespace WebEN.Models
{
    public class SurveyModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }
        public int pageSize { get; set; }
        public List<Survey> listdata { get; set; }
        public int pageIndex { get; set; }
    }
}