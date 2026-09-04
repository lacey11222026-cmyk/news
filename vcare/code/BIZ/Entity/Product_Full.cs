using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIZ.Entity
{
    public class Product_Full : Product
    {
        public ImageParam ImageParam
        {
            get;
            set;
        }
        public string ManuName
        {
            get;
            set;
        }
        public ProParam ProParam
        {
            get;
            set;
        }
    }

    public class ImageParam
    {

        public string Path1 { get; set; }
        public string Path2 { get; set; }
        public string Path3 { get; set; }
        public string Path4 { get; set; }
        public string Path5 { get; set; }
        public string Path6 { get; set; }
        public string Path7 { get; set; }
        public string Path8 { get; set; }
        public string Path9 { get; set; }
    }
    public class ProParam
    {
        public string Pro1 { get; set; }
        public string Pro2 { get; set; }
        public string Pro3 { get; set; }
        public string Pro4 { get; set; }
        public string Pro5 { get; set; }
        public string Pro6 { get; set; }
        public string Pro7 { get; set; }
        public string Price1 { get; set; }
        public string Price2 { get; set; }
        public string Price3 { get; set; }
        public string Price4 { get; set; }
        public string Price5 { get; set; }
        public string Price6 { get; set; }
        public string Price7 { get; set; }
    }
}
