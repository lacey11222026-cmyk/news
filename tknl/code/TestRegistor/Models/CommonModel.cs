using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestRegistor.Models
{
    public class CommonModel
    {
    }
    public class ReturnData
    {
        public int ResponseCode { get; set; }
        public string Description { get; set; }
        public string Extended { get; set; }
    }
    public class CONTENT_APIFULL
    {
        public string Title
        {
            get;
            set;
        }
        public string Contents
        {
            get;
            set;
        }
        public string MainImage
        {
            get;
            set;
        }
        
    }
}