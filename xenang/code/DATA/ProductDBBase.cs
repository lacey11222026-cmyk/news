using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class ProductDBBase: ShopOnlineDBBase
    {
        public static ProductDBBase Create ()
        {
            return ( ProductDBBase ) Activator.CreateInstance ( typeof ( ProductDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateProduct ( Product manuFactory );
        public abstract int UpdateOrder(int Id, bool upOrder);
        public abstract int UpdateOrderTop(int Id);
        public abstract int SetHot(int Id);
        
        public abstract int SetNew(int Id);
        
        public abstract int SetSell(int Id);

        public abstract int UpdateStatus(int Id);        
        #endregion

        #region READ STATEMENTs

        public abstract Product  GetProduct(int Id);
        public abstract IEnumerable<Product> GetAllPaged(string keyword, int categoryId, int manufactoryId,int pageIndex, int pageSize, ref int totalRecords, int? published, bool? isHot, bool? isNew, string lang = "", int min = 0, int max = 0, int orderType = 0);
        public abstract IEnumerable<Product> GetProductsDyn ( string select, string where, string orderBy );
        public abstract  IEnumerable<Product> GetTopLastest(int top, int categoryId, int manufactoryId, int? published, bool? isHot, bool? isNew, string lang = "");
        public abstract IEnumerable<Product> GetTopContentByIds(string ids, int top);
        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteProductDyn ( string where );

        public abstract int DeleteProduct ( int manuFactoryId );

        public abstract int DeleteManufactories ( string listProductId );

        #endregion



    }
}
