using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace BIZ.Entity
{
    //[Serializable]
    public class AlbumImageInfo
    {
       
        public int IsHot
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public string Url
        {
            get;
            set;
        }
    }
    [Serializable]
    public class AlbumImage_FULL : DATA.AlbumImage
    {
        public string MainImage
        {
            get
            {
                if (!string.IsNullOrEmpty(Description))
                {
                    try
                    {
                        var lstimages = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AlbumImageInfo>>(Description);
                        if (lstimages.Count > 0)
                            return lstimages.FirstOrDefault().Url;
                    }
                    catch
                    {

                        return Utils.GetImageUrl(0, string.Empty, false);
                    }
                }

                return Utils.GetImageUrl(0, string.Empty, false);

            }
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
        public List<AlbumImageInfo> Album
        {
            get;
            set;
        }
        public string SPublishDate
        {
            get;
            set;
        }
        public string CategoryName
        {
            get { return CateLiteObj.Name; }
        }
        public string TypeName
        {
            get
            {
                if (Type == 1)
                    return "Công việc";
                return "Gia đình";
            }
        }

    }
}
