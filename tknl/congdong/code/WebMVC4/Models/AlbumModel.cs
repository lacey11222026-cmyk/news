using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMVC4.Models
{
    public class AlbumModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }
        public int pageSize { get; set; }
        public List<Album_FULL> listdata { get; set; }
        public int pageIndex { get; set; }
    }
    public class QAModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }
        public int pageSize { get; set; }
        public List<DATA.QA> listdata { get; set; }
        public int pageIndex { get; set; }
    }
    public class ShopModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }
        public int pageSize { get; set; }
        public List<DATA.Shop> listdata { get; set; }
        public int pageIndex { get; set; }
    }
    public class ExpertModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }
        public int pageSize { get; set; }
        public List<DATA.Expert> listdata { get; set; }
        public int pageIndex { get; set; }
    }
    public class OrganModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }
        public int pageSize { get; set; }
        public List<DATA.Organ> listdata { get; set; }
        public int pageIndex { get; set; }
    }
}