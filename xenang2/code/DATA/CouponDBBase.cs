using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class CouponDBBase: ShopOnlineDBBase
    {
        public static CouponDBBase Create ()
        {
            return ( CouponDBBase ) Activator.CreateInstance ( typeof ( CouponDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateCoupon ( Coupon manuFactory );

        #endregion

        #region READ STATEMENTs

        public abstract Coupon  GetCoupon(string code);
        public abstract Coupon GetCoupon(int Id);
        public abstract IEnumerable<Coupon> GetAllPaged ( int pageIndex, int pageSize, ref int totalRecords, int? published );
        public abstract IEnumerable<Coupon> GetCouponsDyn ( string select, string where, string orderBy );
      

        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteCouponDyn ( string where );

        public abstract int DeleteCoupon ( int manuFactoryId );

        public abstract int DeleteManufactories ( string listCouponId );

        #endregion



    }
}
