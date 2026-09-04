using System;
using System.Collections.Generic;
using DATA;
using UTILS;

namespace BIZ.Entity
{
    public class CONTENT_API
    {
        public string MainImage
        {
            get;
            set;
        }
        public string LinkUrl
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string IntroText
        {
            get;
            set;
        }
        public int CategoryId
        {
            get;
            set;
        }
        public string CategoryName
        {
            get;
            set;
        }
        public long Id
        {
            get;
            set;
        }
        public DateTime PublishDate
        {
            get;
            set;
        }
    }
    [Serializable]
    public class CONTENT_FULL : Content
    {
        //public ArticleParam ArticleParam
        //{
        //    get
        //    {
        //        try
        //        {
        //            return Newtonsoft.Json.JsonConvert.DeserializeObject<ArticleParam>(this.Params);
        //        }
        //        catch (Exception)
        //        {
        //            return new ArticleParam();
        //        }
        //    }
        //}

        public string MainImage
        {
            get
            {
                if (!string.IsNullOrEmpty(Image))
                {
                    if (Image.Contains(Config.UploadUrl)|| Image.Contains("http"))
                        return Image;
                    if (Image.Contains("/Upload/"))
                        return Config.UploadUrl + Image;

                    var images = Image.Split(',');
                    if (images.Length > 0)
                        return Utils.GetImageUrl(Id, EntityName.Article, false) + images[0];
                }

                return "";

            }
        }
        public string LinkUrl
        {
            get
            {
                if (Type == 3)
                    return Url;
                return Config.Domain + Utils.FormatUrlRewrite(Id, Title, "ArticleDetail");
            }
        }
        public List<FileInfo> FileParam
        {
            get;
            set;
        }

        //public string ThumbImage
        //{
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(Image))
        //        {
        //            var images = Image.Split(',');
        //            if (images.Length > 0)
        //                return Utils.GetImageUrl(Id, EntityName.Article, true) + images[0];
        //        }

        //        return Utils.GetImageUrl(0, string.Empty, true);

        //    }
        //}
        public CateLite CateLiteObj
        {
            get
            {
                var category = new CategoryBO().GetCategoryFull(Convert.ToInt32(CategoryId));
                if (category == null)
                    return null;
                return new CateLite { Language = category.Language, ParrentId = category.ParentId.Value, Name = category.Name, Url = category.Url };
            }
        }
        public string CategoryName
        {
            get
            {

                if (CateLiteObj != null) return CateLiteObj.Name;
                return string.Empty;

            }
        }
        public string SPublishDate
        {
            get;
            set;
        }
        //public int ParrentCategoryId
        //{
        //    get
        //    {
        //        var category = new CategoryBO().GetCategoryFull(Convert.ToInt32(CategoryId));
        //        if (category == null)
        //            return 0;

        //        return category.ParentId.Value;
        //    }
        //}
        //public string CategoryImage
        //{
        //    get
        //    {
        //        var category = new CategoryBO().GetCategoryFull(Convert.ToInt32(CategoryId));
        //        if (category == null)
        //            return string.Empty;

        //        return category.Image;
        //    }
        //}

        public Content ConvertToBase()
        {
            Content content = new Content();
            content.Id = Id;
            content.CategoryId = CategoryId;
            content.Title = Title;
            content.Alias = Alias;
            content.IntroText = IntroText;
            content.Contents = Contents;
            content.Image = Image;
            content.Thumbnail = Thumbnail;
            content.Url = Url;
            content.CreatedBy = CreatedBy;
            content.CreatedDate = CreatedDate;
            content.CategoryPathway = CategoryPathway;
            content.PublishDate = PublishDate;
            content.Status = Status;
            content.Type = Type;
            content.Hits = Hits;
            content.Params = Params;
            content.SiteId = SiteId;
            content.SiteUrl = SiteUrl;
            return content;
        }
    }
    public class FileInfo
    {
        public string Name { get; set; }
        public string Path { get; set; }
       
    }
}
