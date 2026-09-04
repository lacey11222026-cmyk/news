using System;
using System.Collections.Generic;
using DATA;
using UTILS;

namespace BIZ.Entity
{
    [Serializable]
    public class CATEGORY_FULL : DATA.Category
    {
        //public List<PRODUCT_FULL> TopListProducts
        //{
        //    get
        //    {

        //        return new ProductBO().GetTopProductFullsByCategory(4, Id);
        //    }
        //}
        public CategoryParam Param
        {
            get;
            set;
        }

        public int NodeLevel
        {
            get { return new CategoryBO().GetNodeLevel(Pathway); }
        }
        
        public string Url
        {
            get
            {
                return Utils.FormatUrlRewriteByType(Id, Name, Type.Value, Link);
                                                    
            }
        }
        public Category ConvertToBase()
        {
            Category category = new Category();
            category.Id = Id;
            category.ParentId = ParentId;
            category.Pathway = Pathway;
            
            category.Name = Name;
            category.Link = Link;
            category.Description = Description;
            category.Contents = Contents;
            category.CreateDate = CreateDate;
            category.ModifiedDate = ModifiedDate;
            category.Published = Published;
            category.Ordering = Ordering;
            category.Language = Language; 
            category.Params = Params;
            category.Type = Type;

            return category;
        }
    }




}
