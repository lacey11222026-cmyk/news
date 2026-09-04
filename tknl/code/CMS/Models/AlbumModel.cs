using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Models
{
    public class AlbumModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }
        public int pageSize { get; set; }
        public List<Album_FULL> listdata { get; set; }
        public int pageIndex { get; set; }
    }
}