using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DATA;

namespace WebMVC4.Models
{
    public class GoNewsModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }
        public int pageSize { get; set; }
        public List<GoNew> listdata { get; set; }
        public int pageIndex { get; set; }
    }
}