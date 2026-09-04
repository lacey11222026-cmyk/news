using BIZ.Entity;
using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMVC4.Models
{
    [Serializable]
    public class ShoppingCartModel
    {
        public List<OrderCouponMapping> ListCoupon { get; set; }

        public List<OrderProductMapping_Full> ListProduct { get; set; }

        public OrderCustomer OderItem { get; set; }

        public OrderExtraData ExtraData { get; set; }



    }
    [Serializable]
    public class OrderExtraData
    {
        public decimal OrderPrice { get; set; }

        public decimal OrderBilling { get; set; }
    }
    [Serializable]
    public class OrderCustomer
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public int CustomerRegion { get; set; }

        public string CustomerCity { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerIp { get; set; }

        public string CustomerComment { get; set; }

        //public string SaleCode { get; set; }
    }
   
}