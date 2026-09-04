using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEN.Models
{
    public class ClassDetailModel
    {
        public List<Album_FULL> relatealbum { get; set; }
        public List<AlbumImage> listimgful { get; set; }
        public Album_FULL album { get; set; }
        public int CategoryId { get; set; }
    }
}