using BIZ;
using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UTILS;
using WebMVC4.Models;

namespace WebMVC4.Helper
{
    public static class OrderHelper
    {
        #region "Order Service"

        /// <summary>
        /// Lấy giỏ hàng và validate lại thông tin
        /// </summary>
        /// <returns></returns>
        public static ShoppingCart GetShopingCartValidate()
        {
            ShoppingCart cart = new ShoppingCart();
            var cartSessionObject = WorkContext.GetSessionKey(OrderConstants.ShopingCartCustomerKey);
            if (cartSessionObject != null)
            {
                cart = (ShoppingCart)cartSessionObject;

                cart = CheckProductInCart(cart);
                cart = CheckCouponInCart(cart);
                return cart;
            }

            return new ShoppingCart();
        }
        /// <summary>
        /// Lấy giỏ hàng
        /// </summary>
        /// <returns>ShopingCart</returns>
        public static ShoppingCart GetShopingCart()
        {
            ShoppingCart cart = new ShoppingCart();
            var cartSessionObject = WorkContext.GetSessionKey(OrderConstants.ShopingCartCustomerKey);
            if (cartSessionObject != null)
                return (ShoppingCart)cartSessionObject;
            return new ShoppingCart();
        }
        /// <summary>
        /// Ghi giỏ hàng vào db hoặc cache hoặc session
        /// </summary>
        /// <param name="cart">THông tin giỏ hàng</param>
        /// <param name="isSaveToDb">Có lưu vào db hay không</param>
        public static void SetShopingCart(ShoppingCart cart, bool isSaveToDb = false)
        {
            //Đảm bảo k bao giờ được sét null
            if (cart == null)
                cart = new ShoppingCart();

            //Kiểu gì cũng sét vào trong session
            WorkContext.SetSessionKey(OrderConstants.ShopingCartCustomerKey, cart);
        }

        /// <summary>
        /// Hàm này lấy toàn bộ mã giảm giá của đơn hàng
        /// </summary>
        /// <returns>List Coupon</returns>
        public static List<Coupon> GetListCouponInCart()
        {
            var listCoupon = new List<Coupon>();
            var cart = GetShopingCart();
            foreach (var item in cart.ListCoupon)
            {
                var validCoupon = new CouponBO().GetCoupon(item.CouponId);
                if (validCoupon != null)
                {
                    if (validCoupon.Status == 1 && validCoupon.CurentNumberUsed < validCoupon.MaxNumberUsed)
                        listCoupon.Add(validCoupon);
                }
            }
            return listCoupon;
        }
        public static ShoppingCart CheckCouponInCart(ShoppingCart cart)
        {
            var listCoupon = new List<Coupon>();

            foreach (var item in cart.ListCoupon)
            {
                var validCoupon = new CouponBO().GetCoupon(item.CouponId);
                if (validCoupon != null && validCoupon.Status == 1 && validCoupon.CurentNumberUsed < validCoupon.MaxNumberUsed)
                    continue;
                cart.RemoveCoupon(item.CouponId);

            }

            return cart;
        }
        /// <summary>
        /// Kiểm tra tình hợp lệ của giỏ hàng
        /// </summary>
        /// <param name="cart">Giỏ hàng</param>
        /// <returns>ShoppingCart</returns>
        public static ShoppingCart CheckProductInCart(ShoppingCart cart)
        {
            //foreach (OrderProductMapping item in cart.ListProduct)
            //{
            //    var product = new ProductBO().GetProduct(item.ProductId);
            //    if (product != null && product.AvailableSell.GetValueOrDefault() && product.Status.GetValueOrDefault() == 1 && product.Price == item.Price)
            //        continue;
            //    cart.RemoveProduct(item.ProductId);
            //}
            return cart;
        }

        /// <summary>
        /// Loai bo gio hang
        /// </summary>
        public static void ClearCart()
        {

            WorkContext.RemoveSessionKey(OrderConstants.ShopingCartCustomerKey);
        }
        public static string GenOrderCode(string prefix)
        {
            string[] pp = ("q,w,e,r,t,y,u,i,o,p,a,s,d,f,g,h,j,k,l,z,x,c,v,b,n,m,1,2,3,4,5,6,7,8,9").Split(',');
            string tmp = "";
            Random rd = new Random();
            for (int i = 1; i <= 2; i++)
            {
                tmp += pp[rd.Next(0, pp.Length - 1)];
            }
            var timeSpan = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            return prefix + DateTime.Now.ToString("yyMMddHHmm") + tmp.ToUpper();
            //return prefix + DateTime.Now.ToString("yyMMddHHmmss");
        }
        public static string ClientIP
        {
            get
            {
                string IP = "";

                
                if (HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null)
                {
                    IP = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
                    return IP;
                }

                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    return IP;
                }

                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED"] != null)
                {
                    IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED"];
                    return IP;
                }

                if (HttpContext.Current.Request.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"] != null)
                {
                    IP = HttpContext.Current.Request.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"];
                    return IP;
                }

                if (HttpContext.Current.Request.ServerVariables["HTTP_FORWARDED_FOR"] != null)
                {
                    IP = HttpContext.Current.Request.ServerVariables["HTTP_FORWARDED_FOR"];
                    return IP;
                }

                if (HttpContext.Current.Request.ServerVariables["HTTP_FORWARDED"] != null)
                {
                    IP = HttpContext.Current.Request.ServerVariables["HTTP_FORWARDED"];
                    return IP;
                }

                if (IP == "")
                {
                    IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                return IP;
            }
        }
        #endregion
    }
}