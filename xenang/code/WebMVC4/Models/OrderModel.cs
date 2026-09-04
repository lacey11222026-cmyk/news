using BIZ.Entity;
using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMVC4.Models
{
    public class OrderModel
    {
        public Order Order { get; set; }
        public List<OrderCouponMapping> ListCoupon { get; set; }

        public List<OrderProductMapping_Full> ListProduct { get; set; }
    }
}