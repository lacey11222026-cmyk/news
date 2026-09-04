using System;
using System.Configuration;
using System.Text;
using DATA;
using UTILS;

namespace BIZ.Entity
{
    [Serializable]
    public class PRODUCT_FULL : DATA.Product
    {
       
        public string MainImage
        {
            get
            {
                if (!string.IsNullOrEmpty(Images))
                {
                    var arrImages = Images.Split(',');
                    return Utils.GetImageUrl(Id, EntityName.Product, true) + arrImages[0];
                }

                return Utils.GetImageUrl(0, string.Empty, false);
            }
        }


        public string CategoryName
        {
            get
            {
                var category = new CategoryBO().GetCategoryFull(Convert.ToInt32(CategoryId));
                if (category == null)
                    return string.Empty;
                return category.Name;
            }
        }

        //public string PathWay
        //{
        //    get
        //    {
        //        return new CategoryBO().GetPathway(Convert.ToInt32(CategoryId));
        //    }
        //}

        public string MFName
        {
            get
            {
                var manufactory = new ManufactoryBO().GetManufactoryFull(Convert.ToInt32(ManufactoryId));
                if (manufactory == null)
                    return string.Empty;
                return manufactory.Title;
            }
        }

        //public string MFWebsite
        //{
        //    get
        //    {
        //        var manufactory = new ManufactoryBO().GetManufactoryFull(Convert.ToInt32(ManufactoryId));
        //        if (manufactory == null)
        //            return "#";
        //        return manufactory.Website;
        //    }
        //}

        //public string MFImage
        //{
        //    get
        //    {
        //        var manufactory = new ManufactoryBO().GetManufactoryFull(Convert.ToInt32(ManufactoryId));
        //        if (manufactory == null)
        //            return string.Empty;

        //        if (string.IsNullOrEmpty(manufactory.Image))
        //            return string.Empty;

        //        var images = manufactory.Image.Split(',');

        //        return images[0];
        //    }
        //}

        //public string MFLogoLink
        //{
        //    get
        //    {
        //        StringBuilder stringBuilder = new StringBuilder();
        //        stringBuilder.Append("<div style=\"min-height:23px;\"><a href=\"" + MFWebsite + "\">").Append("<img src=\"" + MFLogoUrl + "\"  height=\"23\" />").Append("</a></div>");
        //        return stringBuilder.ToString();
        //    }
        //}

        //public string MFLogoUrl
        //{
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(MFImage))
        //            return UTILS.Utils.GetImageUrl(Convert.ToInt32(ManufactoryId), "Manufactory", false) + MFImage;

        //        return ConfigurationManager.AppSettings["NoPhotoUrl"];
        //    }
        //}

        public Product ConvertToBase()
        {
            Product product = new Product();
            product.Id = Id;
            product.CategoryId = CategoryId;
            product.Title = Title;
            product.Name = Name;
            product.Alias = Alias;
            product.ProductCode = ProductCode;
            product.IntroText = IntroText;
            product.FullText = FullText;
            product.CategoryPathway = CategoryPathway;
            product.Images = Images;
            product.Thumbnail = Thumbnail;
            product.Price = Price;
            product.PriceModifyDate = PriceModifyDate;
            product.Attributes = Attributes;
            product.CreatedBy = CreatedBy;
            product.CreatedDate = CreatedDate;
            product.ModifiedBy = ModifiedBy;
            product.ModifiedDate = ModifiedDate;
            product.Published = Published;
            product.Ordering = Ordering;
            product.Hits = Hits;
            product.Count = Count;
            product.Params = Params;
            product.ManufactoryId = ManufactoryId;
            return product;
        }
    }
}
