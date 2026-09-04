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
        //công suất
        public string Wattage
        {
            get;
            set;
        }
        //dung tích
        public string Volumn
        {
            get;
            set;
        }
        //điện áp
        public string V
        {
            get;
            set;
        }
        //hiệu suất
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
        public string Madeby
        {
            get;
            set;
        }
        //tu lanh
        
        //Điện năng tiêu thụ năm
        public string Electric
        {
            get;
            set;
        }
        
        //binh nuoc nong

        //den led
       
        public string HZ
        {
            get;
            set;
        }
        public string LM
        {
            get;
            set;
        }
        public string K
        {
            get;
            set;
        }
        public string Age
        {
            get;
            set;
        }
        public string Color
        {
            get;
            set;
        }
        public string CRI
        {
            get;
            set;
        }
        public string BH
        {
            get;
            set;
        }
        public string W1000
        {
            get;
            set;
        }
        public string Other
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
