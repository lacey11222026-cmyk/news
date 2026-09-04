using System;
using DATA;

namespace BIZ.Entity
{
    public class SUPPORT_FULL:Support
    {
        //public string CategoryName
        //{
        //    get
        //    {
        //        var category = new CategoryBO().GetCategoryFull(Convert.ToInt32(CategoryId));
        //        if (category == null)
        //            return string.Empty;
        //        return category.Name;
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
        public Support ConvertToBase()
        {
            Support support = new Support();
            support.Id = Id;
            support.CategoryId = CategoryId;
            support.Supporter = Supporter;
            support.Yahoo = Yahoo;
            support.Skype = Skype;
            support.Mail = Mail;
            support.Phone = Phone;
            support.Mobile = Mobile;
            support.Published = Published;
            support.Ordering = Ordering;
            support.Params = Params;

            return support;
        }
    }
}
