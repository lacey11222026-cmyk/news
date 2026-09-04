using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIZ.Entity;
using DATA;

namespace WebMVC4.Models
{
    public class DocumentModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }
        public int pageSize { get; set; }
        public List<DOCUMENT_FULL> listdata { get; set; }
        public int pageIndex { get; set; }
    }
    public class AgentModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }
        public int pageSize { get; set; }
        public List<Shop> listdata { get; set; }
        public int pageIndex { get; set; }
    }
}