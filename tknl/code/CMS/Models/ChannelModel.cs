using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DATA;

namespace CMS.Models
{
    public class ChannelModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }
        public int pageSize { get; set; }
        public List<Channel> listdata { get; set; }
        public int pageIndex { get; set; }
    }
}