using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class OrderDBBase: ShopOnlineDBBase
    {
        public static OrderDBBase Create ()
        {
            return ( OrderDBBase ) Activator.CreateInstance ( typeof ( OrderDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int Create(Order manufactory);
        public abstract int Confirm(Order manufactory);
        public abstract int Update(Order manufactory);
        public abstract int InsertCoupon(OrderCouponMapping manufactory);
        public abstract int InsertProduct(OrderProductMapping manufactory);
        #endregion

        #region READ STATEMENTs

        public abstract Order  GetOrder(int Id);

        public abstract Order GetOrder(string OrderNo);
        public abstract IEnumerable<Order> GetAllPaged(string keyword, int pageIndex, int pageSize, int? status, string fromdate, string todate, ref int totalRecords);
        public abstract IEnumerable<Order> GetOrdersDyn ( string select, string where, string orderBy );
        public abstract List<OrderCouponMapping> GetCouponByOrder(long Id);
        public abstract List<OrderProductMapping> GetProductByOrder(long Id);
        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteOrderDyn ( string where );

        public abstract int DeleteOrder ( int manuFactoryId );

        public abstract int DeleteManufactories ( string listOrderId );

        #endregion



    }
}
