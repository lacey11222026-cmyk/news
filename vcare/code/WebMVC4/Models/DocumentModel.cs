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

        public MANUFACTORY_FULL Manu { get; set; }
        public CATEGORY_FULL Cate { get; set; }

        public CATEGORY_FULL CateParrent { get; set; }

        public int pageSize { get; set; }
        public List<Product> listdata { get; set; }

        public List<CATEGORY_FULL> listcate { get; set; }

        public List<MANUFACTORY_FULL> ListManu { get; set; }

        public List<CarSize> ListSize { get; set; }

        public CarModel CarModel { get; set; }
        public int Size { get; set; }
        public int V { get; set; }

        public int OrderType { get; set; }
        public string View { get; set; }
        public string STitle { get; set; }
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