using System;
using System.Collections.Generic;

namespace DATA
{
    public abstract class ProductOrderDBBase:ShopOnlineDBBase
    {
        public static ProductOrderDBBase Create ()
        {
            return ( ProductOrderDBBase ) Activator.CreateInstance ( typeof ( ProductOrderDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateProductOrder ( ProductOrder productOrder );

        #endregion

        #region READ STATEMENTs

        public abstract ProductOrder GetProductOrder ( int productOrderId );
        public abstract IEnumerable<ProductOrder> GetProductOrdersDyn ( string select, string where, string orderBy );
        public abstract IEnumerable<ProductOrder> GetAllProductOrdersPaged ( int pageIndex, int pageSize, ref int totalRecords );
        public abstract IEnumerable<ProductOrder> GetAllProductOrdersPagedDyn ( string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords );
        public abstract IEnumerable<ProductOrder> GetAllProductOrders ( string name, int categoryId );


        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteProductOrderDyn ( string where );
        public abstract int DeleteProductOrder ( int productOrderId );
        public abstract int DeleteProductOrders ( string lstProductOrderIds );

        #endregion
    }
}
