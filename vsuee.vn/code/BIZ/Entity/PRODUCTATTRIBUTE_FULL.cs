using System;

namespace BIZ.Entity
{
    [Serializable]
    public class PRODUCTATTRIBUTE_FULL: DATA.ProductAttribute
    {
        public ATTRIBUTE_FULL AttributeInfo
        {
            get
            {
                return new AttributeBO ().GetAttributeFull ( AttributeId );                                
            }
        }

        public DATA.ProductAttribute ConvertToBase ()
        {
            DATA.ProductAttribute productAttribute = new DATA.ProductAttribute ();
            productAttribute.Id = Id;
            productAttribute.AttributeId = AttributeId;
            productAttribute.ProductId = ProductId;
            productAttribute.NumbericValue = NumbericValue;
            productAttribute.TextValue = TextValue;
            productAttribute.Ordering = Ordering;
            productAttribute.Params = Params;


            return productAttribute;
        }


    }
}
