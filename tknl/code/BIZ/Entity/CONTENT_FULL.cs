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
        public string LinkUrl
        {
            get
            {

                return Utils.FormatUrlRewrite(Id, Title, "ArticleDetail", CategoryName);
            }
        }
        public string MainImage
        {
            get
            {
                try
                {
                    if (Id <= 0)
                        return "https://media.tietkiemnangluong.com.vn/images/VNEPP.png";

                    if (Id >= 20000)
                    {
                        if (Image.Contains(Config.UploadUrl) || Image.Contains("http"))
                            return Image;
                        if (Image.Contains("/Upload/"))
                            return Config.UploadUrl + Image;

                        if (!string.IsNullOrEmpty(Image))
                        {
                            var images = Image.Split(',');
                            if (images.Length > 0)
                                return Utils.GetImageUrl(Id, EntityName.Article, false) + images[0];
                        }

                        return Utils.GetImageUrl(0, string.Empty, false);
                    }
                    else
                    {
                       if(string.IsNullOrEmpty(Image))
                            return "https://media.tietkiemnangluong.com.vn/eepmedia/" + Image;
                        if (Image.Contains(Config.UploadUrl) || Image.Contains("http"))
                            return Image;
                        return "https://media.tietkiemnangluong.com.vn/eepmedia/" + Image;
                    }
                }
                catch
                {
                    return "https://media.tietkiemnangluong.com.vn/images/VNEPP.png";
                }
               

            }
            
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
        public string SPublishDate
        {
            get;
            set;
        }
        public string TempImage
        {
            get;
            set;
        }
        public FileInfo FileParam
        {
            get;
            set;
        }
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
        public string CategoryName
        {
            //get { return CateLiteObj.Name; }
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
            content.Album = Album;
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
            content.Keywords = Keywords;
            content.CreatedRole = CreatedRole;
            content.Language = Language;
            content.SiteId = SiteId;
            //content.SiteUrl = SiteUrl;
            content.IsHot = IsHot;
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

        public string AudioContent { get; set; }
        public string AudioFile1 { get; set; }
        public string AudioFile2 { get; set; }
        public string AudioFile3 { get; set; }
        public string AudioFile4 { get; set; }

        public string MetaTitle { get; set; }
        public string SeoUrl { get; set; }
    }
    public class SpeechInfo
    {
        public string text { get; set; }
        public string type { get; set; }
        public string filename { get; set; }
        public string path { get; set; }

    }

}
