using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class CouponDBSproc: CouponDBBase
    {
        #region Overrides of CouponDBBase

        public override int CreateUpdateCoupon ( Coupon manufactory )
        {
            try
            {
                int _id = manufactory.Id;
                int? _costs = manufactory.Costs;
                string _code = manufactory.Code;
                string _createdUser = manufactory.CreatedUser;
                int? _maxNumberUsed = manufactory.MaxNumberUsed;
                int _status = manufactory.Status;
               

                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.SP_Coupon_InsertUpdate ( _id, _costs, _code, _createdUser, _maxNumberUsed, _status);

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "CouponDBSproc", "CreateUpdateCoupon");
                return -1;
            }
        }
        public override Coupon GetCoupon(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetCouponsDyn(select, where, order).FirstOrDefault();
        }
        public override Coupon GetCoupon ( string code )
        {
            var select = "*";
            var where = "Code = " + code;
            var order = string.Empty;

            return GetCouponsDyn ( select, where, order ).FirstOrDefault ();
        }

        public override IEnumerable<Coupon> GetAllPaged ( int pageIndex, int pageSize, ref int totalRecords, int? published )
        {
            var select = string.Empty;
            var where = string.Empty;
            if ( published >= 0 )
                where += "Status = " + published;
            var orderBy = "ID DESC";

            return GetAllCouponsPagedDyn ( select, where, orderBy, pageIndex, pageSize, ref totalRecords );
        }

        public IEnumerable<Coupon> GetAllCouponsPagedDyn ( string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords )
        {
            try
            {
                string _select = select;
                string _where = where;
                string _orderBy = orderBy;
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecords = 0;
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    var list = datacontext.sp_Coupon_SelectPagedDynamic ( _select, _where, _orderBy, _pageIndex, _pageSize, ref _totalRecords ).ToArray ();
                    totalRecords = Convert.ToInt32 ( _totalRecords );
                    return list;
                }
            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "CouponDBSproc", "GetAllCouponsPagedDyn");
                return null;
            }
        }

        public override IEnumerable<Coupon> GetCouponsDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    return datacontext.sp_Coupon_SelectDynamic ( select, where, orderBy ).ToArray ();
                }
            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "CouponDBSproc", "GetCouponsDyn");
                return null;
            }
        }

     
        public override int DeleteCouponDyn ( string where )
        {
            try
            {
                string _where = where;
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    return datacontext.sp_Coupon_DeleteDynamic ( _where );
                }
            }
            catch ( Exception exp )
            {
                ExHandler.Handle ( exp );
                return -1;
            }
        }

        public override int DeleteCoupon ( int manuFactoryId )
        {
            string where = "Id = " + manuFactoryId;
            return DeleteCouponDyn ( where );
        }

        public override int DeleteManufactories ( string listCouponId )
        {
            string where = "Id IN (" + listCouponId + ")";
            return DeleteCouponDyn ( where );
        }

        #endregion
    }
}
