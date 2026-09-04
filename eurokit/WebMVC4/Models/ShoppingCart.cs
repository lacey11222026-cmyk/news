using BIZ.Entity;
using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMVC4.Models
{
    [Serializable]

    public class ShoppingCart
    {
        public List<OrderCouponMapping> ListCoupon { get; set; }

        public List<OrderProductMapping_Full> ListProduct { get; set; }
        public ShoppingCart()
        {
            ListCoupon = new List<OrderCouponMapping>();
            ListProduct = new List<OrderProductMapping_Full>();
        }
        public void RemoveProduct(int productId)
        {

            ListProduct = ListProduct.Where(x => x.ProductId != productId).ToList();
        }
        public void SetProductQuantity(int productId, int quantity)
        {
            if (quantity == 0)
            {
                RemoveProduct(productId);
                return;
            }

            foreach (OrderProductMapping_Full item in ListProduct)
            {
                if (item.ProductId == productId)
                {
                    item.Amount = quantity;
                    return;
                }
            }
        }
        public void AddProductItem(OrderProductMapping_Full newItem)
        {

            if (ListProduct.Where(x => x.ProductId == newItem.ProductId).Count() > 0)
            {
                foreach (OrderProductMapping_Full item in ListProduct)
                {
                    if (item.ProductId == newItem.ProductId)
                    {
                        item.Amount += newItem.Amount;
                        return;
                    }
                }

            }
            else
            {
                newItem.Amount = newItem.Amount;
                ListProduct.Add(newItem);
            }
        }
        public void AddCouponItem(OrderCouponMapping newItem)
        {

            ListCoupon.Add(newItem);

        }
        public void RemoveCoupon(int CouponId)
        {
            ListCoupon = ListCoupon.Where(x => x.CouponId != CouponId).ToList();
        }
    }
}