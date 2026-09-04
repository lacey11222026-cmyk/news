using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using DATA;
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
        //public CateLite CateLiteObj
        //{
        //    get
        //    {
        //        var category = new CategoryBO().GetCategoryFull(Convert.ToInt32(CategoryId));
        //        if (category == null)
        //            return null;
        //        return new CateLite { Language = category.Language, ParrentId = category.ParentId.Value, Name = category.Name };
        //    }
        //}
        public string SPublishDate
        {
            get;
            set;
        }
        public Document ConvertToBase()
        {
            Document content = new Document();
            content.Id = Id;
            content.CategoryId = CategoryId;
            content.Name = Name;
            content.Code = Code;
            content.Description = Description;
            content.FilePath = FilePath;
            content.EffectiveDate = EffectiveDate;
            content.ExpiryDate = ExpiryDate;
            content.CreatedBy = CreatedBy;
            content.CreatedDate = CreatedDate;
            content.CategoryPathway = CategoryPathway;
            content.PublishDate = PublishDate;
            content.SignedBy = SignedBy;
            content.SignedByDesc = SignedByDesc;
            content.Status = Status;
            content.Hits = Hits;
            content.Language = Language;
            content.Private = Private;
            content.PublishDate = PublishDate;
            //content.Language = Language;
            return content;
        }
        public string SPublishedTime
        {
            get;
            set;
        }
       
        public string CategoryName
        {
            //get { return CateLiteObj.Name; }
            get;
            set;
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
