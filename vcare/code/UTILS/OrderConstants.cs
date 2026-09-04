using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UTILS
{
    public static class OrderConstants
    {
        /// <summary>
        /// Khóa lưu giỏ hàng vào giỏ hàng
        /// </summary>
        public static string ShopingCartCustomerKey = "PSC_CMK";
        /// <summary>
        /// Khóa lưu giỏ hàng vào giỏ hàng
        /// </summary>
        public static string SessionOrderModelKey = "Order_Mode_Key_Session";
        /// <summary>
        /// Khóa lưu giỏ hàng vào giỏ hàng
        /// </summary>
        public static string TempDataAddCouponKey = "TempDataAddCouponKey";


        public static int BuyProduct = 1;
        public static int BuyPacket = 2;

        public static int PaymentByCard = 2;
        public static int PaymentByPay = 1;
        public static int PaymentByBank = 3;

        public static int PaymentByGoogle = 4;
        public static int PaymentByApple = 5;
        public static int PaymentByVisa = 6;
        //gia han
        public static int PacketExtend = 1;

        //nang cap
        public static int PacketPromote = 2;


        // gian han call/sms
        public static int PacketSMSExtend = 3;

        // khuyen mai
        public static int PacketPromotion = 4;


    }
}
