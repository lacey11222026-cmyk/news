using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIZ.Entity;

namespace Local.Models
{
    public class TopDocumentModel
    {
        public List<DOCUMENT_FULL> lstdata { get; set; }
        public int CategoryId { get; set; }
        public string HeaderTitle { get; set; }
        public string Url { get; set; }
    }
}