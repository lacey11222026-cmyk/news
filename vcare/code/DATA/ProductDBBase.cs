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
        public abstract Product GetByUrl(string url);
        public abstract IEnumerable<Product> GetAllPaged(string keyword, int categoryId, int manufactoryId, string manufactory, int s, int v, int pageIndex, int pageSize, ref int totalRecords, int? published, bool? isHot, bool? isNew, int model = 0, int min = 0, int max = 0, int orderType = 0, int carId = 0);
        public abstract IEnumerable<Product> GetProductsDyn ( string select, string where, string orderBy );
        public abstract IEnumerable<Product> GetTopLastest(int top, int categoryId, int manufactoryId, int size, int v, int id, decimal price,int notManuId);

        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteProductDyn ( string where );

        public abstract int DeleteProduct ( int manuFactoryId );

        public abstract int DeleteManufactories ( string listProductId );

        #endregion



    }
}
