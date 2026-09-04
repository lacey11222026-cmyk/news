using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace BIZ.Entity
{
    [Serializable]
    public class DOCUMENT_FULL : DATA.Document
    {
        //public int CategoryParrentId
        //{
        //    get
        //    {
        //        var category = new CategoryBO().GetCategoryFull(Convert.ToInt32(CategoryId));
        //        if (category == null)
        //            return 0;
        //        return category.ParentId.Value;
        //    }
        //}
        //public string Language
        //{
        //    get
        //    {
        //        var category = new CategoryBO().GetCategoryFull(Convert.ToInt32(CategoryId));
        //        if (category == null)
        //            return string.Empty;

        //        return category.Language;
        //    }
        //}
        public CateLite CateLiteObj
        {
            get
            {
                var category = new CategoryBO().GetCategoryFull(Convert.ToInt32(CategoryId));
                if (category == null)
                    return null;
                return new CateLite { Language = category.Language, ParrentId = category.ParentId.Value, Name = category.Name };
            }
        }
        public string CategoryName
        {
            get { return CateLiteObj.Name; }
        }
        public string FileName
        {
            get
            {
                if (!String.IsNullOrEmpty(FilePath))
                {
                    try
                    {
                        var listf = FilePath.Trim().Split('/');
                        return listf[listf.Length - 1].ToString();
                    }
                    catch 
                    {

                        return String.Empty;
                    }
                }
                   return String.Empty;
            }
        }
        public string SPublishDate
        {
            get;
            set;
        }
        public string SEffectiveDate
        {
            get;
            set;
        }
        public string SExpiryDate
        {
            get;
            set;
        }
        public string LinkUrl
        {
            get
            {
                if (DocType == 1)
                    return $"van-ban/{Utils.ConvertToRewriteLink(Name)}-{Id}.html";
                //if(!string.IsNullOrEmpty(SiteUrl))
                //    return SiteUrl+Utils.FormatUrlRewrite(Id, Title, "ArticleDetail");
                return $"giay-phep/{Utils.ConvertToRewriteLink(Name)}-{Id}.html";
            }
        }
        public string FileType
        {
            get
            {
               
                try
                {
                    var lisext = FileName.Trim().Split('.');
                    var ext = "."+lisext[lisext.Length - 1].ToString();
                    switch (ext.ToString())
                    {

                        case ".doc":
                            return "/Administrator/images/Icon/doc.jpg";

                        case ".docx":
                            return "/Administrator/images/Icon/doc.jpg";

                        case ".rar":
                            return "/Administrator/images/Icon/rar.jpg";

                        case ".zip":
                            return "/Administrator/images/Icon/rar.jpg";

                        case ".xls":
                            return "/Administrator/images/Icon/exel.jpg";

                        case ".xlsx":
                            return "/Administrator/images/Icon/exel.jpg";

                        case ".ppt":
                            return "/Administrator/images/Icon/ppt.jpg";

                        case ".swf":
                            return "/Administrator/images/Icon/flv.jpg";


                        case ".pdf":
                            return "/Administrator/images/Icon/pdf.jpg";



                    }
                    return "/Administrator/images/Icon/default.jpg";
                }
                catch 
                {
                    return "/Administrator/images/Icon/default.jpg";
                }
             
            }
        }

    }
}
