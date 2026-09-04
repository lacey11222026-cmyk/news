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

}
