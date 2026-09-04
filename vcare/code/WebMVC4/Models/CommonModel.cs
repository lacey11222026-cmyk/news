using LibGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DATA;
using BIZ.Entity;

namespace WebMVC4.Models
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
    public class PriceReturnData
    {
        
        public string PriceBilling { get; set; }
        public string PriceShipping { get; set; }
    }
    public class SearchResult
    {
        public List<OriginalPart> original_parts { get; set; }
        public List<ReplacementPart> replacement_parts { get; set; }

        public Part part { get; set; }
    }
    public class MenuModel
    {
        public List<CATEGORY_FULL> ListCate { get; set; }
        public List<CATEGORY_FULL> ListMainCate { get; set; }
        public List<MANUFACTORY_FULL> ListManu { get; set; }

        public List<CarSize> ListSize { get; set; }
    }
}