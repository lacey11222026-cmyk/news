using System;
using System.Collections.Generic;

namespace DATA
{
    public abstract class ProductAttributeDBBase: ShopOnlineDBBase
    {
        public static ProductAttributeDBBase Create ()
        {
            return ( ProductAttributeDBBase ) Activator.CreateInstance ( typeof ( ProductAttributeDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateProductAttribute ( ProductAttribute productAttribute );

        #endregion

        #region READ STATEMENTs

        public abstract ProductAttribute GetProductAttribute ( int categoryId );
        public abstract IEnumerable<ProductAttribute> GetProductAttributesDyn ( string select, string where, string orderBy );
        public abstract IEnumerable<ProductAttribute> GetAllProductAttributes ( int attributeId );
        public abstract IEnumerable<ProductAttribute> GetAllProductAttributes ( int attributeId, List<string> lstTextValue, List<double> lstNumberValue );
        public abstract IEnumerable<ProductAttribute> GetAllProductAttributes ( int attributeId, double fromVal, double toVal );
        public abstract IEnumerable<ProductAttribute> GetAllProductAttributesByProduct ( int productId );

        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteProductAttributeDyn ( string where );
        public abstract int DeleteProductAttribute ( int productAttributeId );
        public abstract int DeleteProductAttributeByProductId ( int productId );
        public abstract int DeleteProductAttributeByProductId ( string listProductIds );

        #endregion
    }
}
