using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIZ.Entity;

namespace WebMVC4.Areas.Image2016.Models
{
    public class AlbumImageModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }
        public int pageSize { get; set; }
        public List<AlbumImage_FULL> listdata { get; set; }
        public int pageIndex { get; set; }
    }
}