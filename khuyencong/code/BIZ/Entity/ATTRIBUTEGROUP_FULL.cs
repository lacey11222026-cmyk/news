using System;
using DATA;

namespace BIZ.Entity
{
    [Serializable]
    public class ATTRIBUTEGROUP_FULL: DATA.AttributeGroup
    {       
        public AttributeGroup ConvertToBase ()
        {
            AttributeGroup attributeGroup = new AttributeGroup ();
            attributeGroup.Id = Id;
            attributeGroup.CategoryId = CategoryId;
            attributeGroup.Title = Title;
            attributeGroup.Published = Published;
            attributeGroup.Ordering = Ordering;
            attributeGroup.Language = Language;

            return attributeGroup;

        }
    }
}
