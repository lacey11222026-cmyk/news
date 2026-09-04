using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using Coupon = DATA.Coupon;
namespace BIZ
{
    public class CouponBO
    {
       

        #region CREATE
        public int Create(Coupon Coupon)
        {
            
            int returnVal = CouponDBBase.Create().CreateUpdateCoupon(Coupon);
          
            return returnVal;
        }
       
        #endregion

        #region READ

       
        public Coupon GetCoupon(int CouponId)
        {
            try
            {
                return CouponDBBase.Create().GetCoupon(CouponId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "CouponBO", "GetCoupon");
                return null;
            }
        }
       
        public Coupon GetCoupon(string code)
        {
            try
            {
                return CouponDBBase.Create().GetCoupon(code);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "CouponBO", "GetCoupon");
                return null;
            }
        }
    
        public List<Coupon> GetCouponsPaged(int? status, int pageIndex, int pageSize, ref int totalRecords)
        {
            var Documents = CouponDBBase.Create().GetAllPaged(pageIndex, pageSize, ref totalRecords, status);
            if (Documents == null)
                return null;

            return Documents.ToList();
        }
       
        public string GetCouponsPaged_JSON(int? status, int pageIndex, int pageSize)
        {
            int totalRecords = 0;
            List<Coupon> Coupons = GetCouponsPaged(status, pageIndex, pageSize,ref totalRecords);

            if (Coupons == null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{Total:").Append(totalRecords).Append(",Items:").Append(Utils.ConvertToJson(Coupons, string.Empty)).Append("}");

            var json = stringBuilder.ToString();
           

            return json;
        }


       





        #endregion



        #region DELETE

        public int DeleteCoupons(string listIds)
        {
            var returnVal = CouponDBBase.Create().DeleteManufactories(listIds);
         
            return returnVal;
        }

        public int DeleteCoupon(int id)
        {
            var returnVal = CouponDBBase.Create().DeleteCoupon(id);
           
            return returnVal;
        }

        #endregion

      
    }
}
