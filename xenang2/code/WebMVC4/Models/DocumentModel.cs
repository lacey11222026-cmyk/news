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
    public class ProductModel
    {
        public int total { get; set; }
        public int CategoryId { get; set; }

        public int ManufactoryId { get; set; }
        public CATEGORY_FULL Cate { get; set; }

        public CATEGORY_FULL CateParrent { get; set; }

        public int pageSize { get; set; }
        public List<Product> listdata { get; set; }

        public List<CATEGORY_FULL> listcate { get; set; }
        public List<CategoryManufactory> listmanu { get; set; }
        public int pageIndex { get; set; }
    }
    public class ProductDetailModel
    {
        
        public CATEGORY_FULL Cate { get; set; }

        public CATEGORY_FULL CateParrent { get; set; }
        public Product_Full Detail { get; set; }

        public Manufactory Manu { get; set; }

    }
}