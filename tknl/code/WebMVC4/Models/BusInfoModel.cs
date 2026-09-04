using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMVC4.Models
{
    public class BusInfoModel
    {
        public List<BusInfo> ListData { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int CityId { get; set; }
    }
}