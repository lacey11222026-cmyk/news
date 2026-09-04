using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIZ.Entity
{
    public class Product_Full : Product
    {
        public List<ProductFileInfo> ImageParam
        {
            get;
            set;
        }
        public ProductParam ProductParam
        {
            get;
            set;
        }
        
    }
    public class ProductFileInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }

    }
    [Serializable]
    public class ProductParam
    {
        public string Model
        {
            get;
            set;
        }

        public string Number
        {
            get;
            set;
        }

        public string Manuface
        {
            get;
            set;
        }

        public string MadeIn
        {
            get;
            set;
        }

        public string GroupCert
        {
            get;
            set;
        }
        public string GroupExp
        {
            get;
            set;
        }
        public string Rule
        {
            get;
            set;
        }
        public string Wattage
        {
            get;
            set;
        }
        public string Efficiency
        {
            get;
            set;
        }
        public string Logo
        {
            get;
            set;
        }
        public string Tracking
        {
            get;
            set;
        }
        public string Year
        {
            get;
            set;
        }
    }

}
