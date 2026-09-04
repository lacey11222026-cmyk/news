using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace BIZ.Entity
{

    public class DocExtend
    {
        public string Name1 { get; set; }
        public string Path1 { get; set; }
        public string Name2 { get; set; }
        public string Path2 { get; set; }
        public string Name3 { get; set; }
        public string Path3 { get; set; }

        public string Method { get; set; }
        public string Time { get; set; }
        public string Subject { get; set; }

        public string DirectAgency { get; set; }
        public string Address { get; set; }
        public string Result { get; set; }
        public string Status { get; set; }

        public string Step { get; set; }
        public string Profile { get; set; }
        public string Request { get; set; }
        public string Number { get; set; }

    }
    [Serializable]
    public class DOCUMENT_FULL : DATA.Document
    {
        public DocExtend Extend
        {
            get;
            set;
        }
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
