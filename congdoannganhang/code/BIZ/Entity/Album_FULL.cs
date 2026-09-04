using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace BIZ.Entity
{
    [Serializable]
    public class Album_FULL : DATA.Album
    {

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
        //public string CategoryName
        //{
        //    get { return CateLiteObj.Name; }
        //}
        public string MainImage
        {
            get
            {
                if (!string.IsNullOrEmpty(Images))
                {
                    try
                    {
                        var lstimages = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AlbumImage>>(Images);
                        if (lstimages.Count > 0)
                        {
                            if (Id > 146)
                            {
                                return Utils.GetImageUrl(Id, EntityName.Album, false) + lstimages.FirstOrDefault().Name;
                            }
                            else
                            {
                                return "https://media.vnubw.org.vn/" + Utils.GetImageUrl(Id, EntityName.Album, false).Replace("https://media02.vnubw.org.vn/", "") + lstimages.FirstOrDefault().Name;
                            }
                        }
                        //if (lstimages.Count > 0)
                        //    return Utils.GetImageUrl(Id, EntityName.Album, false) + lstimages.FirstOrDefault().Name;
                    }
                    catch 
                    {

                        return Utils.GetImageUrl(0, string.Empty, false);
                    }
                }

                return Utils.GetImageUrl(0, string.Empty, false);

            }
        }
        public string SPublishDate
        {
            get;
            set;
        }
        public List<AlbumImage> AlbumImage
        {
            get;
            set;
        }
    }
}
