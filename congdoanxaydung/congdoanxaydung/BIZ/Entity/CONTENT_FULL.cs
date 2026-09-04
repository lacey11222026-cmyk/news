using System;
using DATA;
using UTILS;

namespace BIZ.Entity
{
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
                    if (Image.StartsWith("http"))
                        return Image;
                    if (Image.Contains("/Upload/"))
                        return Image;
                    if (Image.Contains("/xdmedia/"))
                        return Image;
                    if (Image.Contains("2010/") || Image.Contains("2011/") || Image.Contains("2012/") || Image.Contains("2013/"))
                        return "/xdmedia/" + Image;
                    var images = Image.Split(',');
                    if (images.Length > 0)
                        return Utils.GetImageUrl(Id, EntityName.Article, false) + images[0];
                }

                return "/Images/Upload/no_image.jpg";

            }
        }
        public string LinkUrl
        {
            get
            {
                return  Utils.FormatUrlRewrite(Id, Title, "ArticleDetail");
            }
        }
        public FileInfo FileParam
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
                return new CateLite { Language = category.Language, ParrentId = category.ParentId.Value, Name = category.Name,Url=category.Url };
            }
        }
        public string CategoryName
        {
            get
            {

                if (CateLiteObj!= null) return CateLiteObj.Name;
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
            content.ChannelId = ChannelId;
            content.Url = Url;
            content.CreatedBy = CreatedBy;
            content.CreatedDate = CreatedDate;
            content.CategoryPathway = CategoryPathway;
            content.PublishDate = PublishDate;
            content.Status = Status;
            content.Type = Type;
            content.Hits = Hits;
            content.Params = Params;
            //content.SiteId = SiteId;
            //content.SiteUrl = SiteUrl;
            return content;
        }
    }
    public class FileInfo
    {
        public string Name1 { get; set; }
        public string Path1 { get; set; }
        public string Name2 { get; set; }
        public string Path2 { get; set; }
        public string Name3 { get; set; }
        public string Path3 { get; set; }
    }
}
