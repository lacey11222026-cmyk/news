using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace BIZ.Entity
{
    [Serializable]
    public class AlbumImage_FULL : DATA.AlbumImage
    {

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
