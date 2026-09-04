using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEN.Models
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