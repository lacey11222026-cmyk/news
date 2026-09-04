using System;
using DATA;

namespace BIZ.Entity
{
    public class Contact_FULL:Contact
    {
        public string CategoryName
        {
            get
            {
                var category = new CategoryBO().GetCategoryFull(Convert.ToInt32(CategoryId));
                if (category == null)
                    return string.Empty;
                return category.Name;
            }
        }

       
    }
}
