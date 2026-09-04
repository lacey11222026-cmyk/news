using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using Order = DATA.Order;
namespace BIZ
{
    public class OrderBO
    {
       

        #region CREATE
        public int Create(Order Order)
        {
            
            int returnVal = OrderDBBase.Create().Create(Order);
          
            return returnVal;
        }
        public int Confirm(Order Order)
        {

            int returnVal = OrderDBBase.Create().Confirm(Order);

            return returnVal;
        }
        public int Update(Order Order)
        {

            int returnVal = OrderDBBase.Create().Update(Order);

            return returnVal;
        }
        public int InsertProduct(OrderProductMapping manufactory)
        {

            int returnVal = OrderDBBase.Create().InsertProduct(manufactory);

            return returnVal;
        }

        public int InsertCoupon(OrderCouponMapping manufactory)
        {
            int returnVal = OrderDBBase.Create().InsertCoupon(manufactory);
            return returnVal;
        }
        #endregion

        #region READ

       
        public Order GetOrder(int OrderId)
        {
            try
            {
                return OrderDBBase.Create().GetOrder(OrderId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "OrderBO", "GetOrder");
                return null;
            }
        }
       
        public Order GetOrder(string OrderNo)
        {
            try
            {
                return OrderDBBase.Create().GetOrder(OrderNo);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "OrderBO", "GetOrder");
                return null;
            }
        }
        public List<OrderCouponMapping> GetCouponByOrder(long Id)
        {
            var data = OrderDBBase.Create().GetCouponByOrder(Id);
            if (data == null)
                return null;

            return data.ToList();
        }
        public List<OrderProductMapping> GetProductByOrder(long Id)
        {
            var data = OrderDBBase.Create().GetProductByOrder(Id);
            if (data == null)
                return null;

            return data.ToList();
        }

        public List<Order> GetOrdersPaged(string title, int status, int pageIndex, int pageSize, string fromdate, string todate, ref int totalRecords)
        {
            var Documents = OrderDBBase.Create().GetAllPaged(title, pageIndex,  pageSize, status, fromdate, todate,ref totalRecords);
            if (Documents == null)
                return null;

            return Documents.ToList();
        }
       
        public string GetOrdersPaged_JSON(string title,int status, int pageIndex, int pageSize, string fromdate, string todate)
        {
            int totalRecords = 0;
            List<Order> Orders = GetOrdersPaged(title, status, pageIndex, pageSize, fromdate,todate,ref totalRecords);

            if (Orders == null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{Total:").Append(totalRecords).Append(",Items:").Append(Utils.ConvertToJson(Orders, string.Empty)).Append("}");

            var json = stringBuilder.ToString();
           

            return json;
        }


        public List<City> GetCity(int Top,int? stattus)
        {
            var data = CityDBBase.Create().GetTopLastest(Top, stattus);
            if (data == null)
                return null;
            return data.ToList();
        }





        #endregion



        #region DELETE

        public int DeleteOrders(string listIds)
        {
            var returnVal = OrderDBBase.Create().DeleteManufactories(listIds);
         
            return returnVal;
        }

        public int DeleteOrder(int id)
        {
            var returnVal = OrderDBBase.Create().DeleteOrder(id);
           
            return returnVal;
        }

        #endregion

      
    }
}
