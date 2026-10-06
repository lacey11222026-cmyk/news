using System;
using DATA;

namespace BIZ.Entity
{
    [Serializable]
    public class PRODUCTORDER_FULL: ProductOrder
    {
        public DATA.ProductOrder ConvertToBase ()
        {
            ProductOrder productOrder = new ProductOrder ();
            productOrder.Id = Id;
            productOrder.ProductId = ProductId;
            productOrder.ProductCode = ProductCode;
            productOrder.ProductTitle = ProductTitle;
            productOrder.ProductPrice = ProductPrice;
            //productOrder.UserId = UserId;
            productOrder.UserName = UserName;
            productOrder.UserEmail = UserEmail;
            productOrder.UserPhone = UserPhone;
            productOrder.UserMobile = UserMobile;
            productOrder.UserAddress = UserAddress;
            productOrder.OrderDate = OrderDate;
            productOrder.OrderDateStamp = OrderDateStamp;
            productOrder.State = State;
            productOrder.Published = Published;
           // productOrder.Ordering = Ordering;
            //productOrder.Params = Params;
            return productOrder;
        }
    }
}
