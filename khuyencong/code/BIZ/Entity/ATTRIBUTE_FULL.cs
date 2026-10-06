using System;
using Newtonsoft.Json;
using UTILS;

namespace BIZ.Entity
{
    [Serializable]
    public class ATTRIBUTE_FULL: DATA.Attribute
    {
        public string CategoryName
        {
            get
            {
                var category = new CategoryBO ().GetCategoryFull ( Convert.ToInt32 ( CategoryId ) );
                if ( category == null )
                    return string.Empty;

                return category.Name;
            }
        }

        public string GroupName
        {            
            get
            {
                var group = new AttributeGroupBO().GetAttributeGroupFull(Convert.ToInt32(GroupId));
                if (group == null)
                    return string.Empty;

                return group.Title;
            }
        }

        public Filter FilterEntity
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<Filter> ( this.Filter );
                }
                catch ( Exception ex )
                {
                    ExHandler.Handle ( ex,"ATTRIBUTE_FULL","FilterEntity" );
                    return null;
                }
            }
        }



        public DATA.Attribute ConvertToBase ()
        {

            DATA.Attribute attribute = new DATA.Attribute ();
            attribute.Id = Id;
            attribute.GroupId = GroupId;
            attribute.CategoryId = CategoryId;
            attribute.Title = Title;
            attribute.FilterType = FilterType;
            attribute.Published = Published;
            attribute.Ordering = Ordering;
            attribute.Params = Params;
            attribute.DataType = DataType;
            attribute.Unit = Unit;
            attribute.Filter = Filter;
            
            return attribute;
        }
    }
}
