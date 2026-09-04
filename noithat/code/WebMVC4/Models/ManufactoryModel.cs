using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DATA;
using BIZ.Entity;
namespace WebMVC4.Models
{
    public class CategoryManufactoryModel
    {
        public List<MANUFACTORY_FULL> Manufactory { get; set; }

        public List<CATEGORY_FULL> Category { get; set; }
        public List<CategoryManufactory> CategoryManufactory { get; set; }
    }
}