using BIZ;
using BIZ.Entity;
using DATA;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;
using WebMVC4.Filter;
using WebMVC4.Helper;
using WebMVC4.Models;

namespace WebMVC4.Controllers
{
    public class OrderController : Controller
    {

        [LocalizationActionFilter]
        public ActionResult PreOrderInfo()
        {
            return View();
        }
        [LocalizationActionFilter]
        public ActionResult ShoppingCart()
        {
            if (OrderHelper.GetShopingCart().ListProduct.Count <= 0)
                return RedirectToAction("CartEmpty");

            var request = System.Web.HttpContext.Current.Request;

            return View();
        }
        public ActionResult _ShoppingCart()
        {
            #region Kiểm tra truy cập
            var request = System.Web.HttpContext.Current.Request;


            var orderModel = WorkContext.GetSessionKey(OrderConstants.SessionOrderModelKey);
            if (orderModel != null)
            {
                var curentModel = orderModel as ShoppingCartModel;
                curentModel.ListProduct = GetMappingOrderProductModel();
                curentModel.ListCoupon = GetMappinpOrderCouponModel();
                curentModel.ExtraData = GetOrderExtraData();
                WorkContext.SetSessionKey(OrderConstants.SessionOrderModelKey, curentModel);
                return PartialView(curentModel);

            }

            #endregion

            var model = new ShoppingCartModel()
            {
                OderItem = new OrderCustomer { CustomerCity = "Hà Nội" },
                ListProduct = GetMappingOrderProductModel(),
                ListCoupon = GetMappinpOrderCouponModel(),
                ExtraData = GetOrderExtraData(),

            };
            WorkContext.SetSessionKey(OrderConstants.SessionOrderModelKey, model);


            return PartialView(model);
        }
        [HttpPost]
        public ActionResult ShoppingCart(ShoppingCartModel model)
        {
            #region Kiểm tra truy cập

            var shopingCart = OrderHelper.GetShopingCart();
            if (shopingCart.ListProduct.Count <= 0)
                return RedirectToAction("CartEmpty");
            #endregion

            if (ModelState.IsValid)
            {
                WorkContext.SetSessionKey(OrderConstants.SessionOrderModelKey, model);
                return RedirectToAction("OrderConfirmation");
            }

            return View(model);
        }
        [HttpGet]
        [LocalizationActionFilter]
        public ActionResult OrderConfirmation()
        {


            var shoppingCart = OrderHelper.GetShopingCartValidate();
            OrderHelper.SetShopingCart(shoppingCart);
            if (shoppingCart.ListProduct.Count <= 0)
                return RedirectToAction("CartEmpty");
            var orderModel = WorkContext.GetSessionKey(OrderConstants.SessionOrderModelKey);
            if (orderModel == null)
                return RedirectToAction("ShoppingCart");


            var modelData = (ShoppingCartModel)orderModel;

            modelData.ListProduct = GetMappingOrderProductModel();
            modelData.ListCoupon = GetMappinpOrderCouponModel();
            modelData.ExtraData = GetOrderExtraData();
            return View(modelData);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LocalizationActionFilter]
        public ActionResult OrderConfirmation(int PaymentMethod = 0)
        {
            //tạo đơn hàng
            #region Kiểm tra truy cập

            var shoppingCart = OrderHelper.GetShopingCartValidate();
            if (shoppingCart.ListProduct.Count <= 0)
                return RedirectToAction("CartEmpty");

            var sessionData = WorkContext.GetSessionKey(OrderConstants.SessionOrderModelKey);
            if (sessionData == null)
                return RedirectToAction("ShoppingCart");
            var orderModel = sessionData as ShoppingCartModel;
            if (orderModel == null)
                return RedirectToAction("CartEmpty");
            if (string.IsNullOrEmpty(orderModel.OderItem.FullName))
                return RedirectToAction("CartEmpty");
            #endregion


            var listProduct = GetMappingOrderProductModel();
            var listCoupon = GetMappinpOrderCouponModel();
            var extraData = GetOrderExtraData();


            TempData["Message"] = Resources.Order.MessageError;
            TempData["Code"] = -99;

            //tạo đơn hàng
            string saleCode = "";
            var request = System.Web.HttpContext.Current.Request;
            if (request.Cookies["Sale"] != null)
            {
                saleCode = request.Cookies["Sale"].Value.ToString();
            }
            var orderNo = OrderHelper.GenOrderCode("M");
            var orderObj = new Order
            {
                OrderNo = orderNo,
                PaymentMethod = 1,
                FullName = orderModel.OderItem.FullName,
                Email = orderModel.OderItem.Email,
                Mobile = orderModel.OderItem.Mobile,
                CustomerRegion = orderModel.OderItem.CustomerRegion,
                SaleCode = saleCode,
                CustomerCity = orderModel.OderItem.CustomerCity,
                CustomerComment = orderModel.OderItem.CustomerComment,
                CustomerAddress = orderModel.OderItem.CustomerAddress,
                CustomerIp = OrderHelper.ClientIP,
                OrderPrice = extraData.OrderPrice,
                OrderBilling = extraData.OrderBilling,
                OrderTotal = extraData.OrderBilling,
                Deleted = false,
                Status = 0,

            };
            //đặt hàng
            orderObj.PaymentMethod = 2;
            TempData["OrderInfo"] = String.Format(Resources.Order.MessageOrderInfo, orderNo);
            var orderId = new OrderBO().Create(orderObj);
            if (orderId < 0)
            {
                return RedirectToAction("PreOrderInfo");
            }
            //thêm mapping sản phẩm
            var productName = "";

            foreach (var product in shoppingCart.ListProduct)
            {
                product.OrderId = orderId;
                productName += " " + product.ProductName + ";";
                new OrderBO().InsertProduct(product);
            }
            if (shoppingCart.ListCoupon.Count > 0)
            {
                foreach (var coupon in shoppingCart.ListCoupon)
                {
                    coupon.OrderId = orderId;
                    new OrderBO().InsertCoupon(coupon);
                }
            }
            //gửi mail
            string mailsubject = "Thông báo đặt hàng";
            string mailform = System.IO.File.ReadAllText(Server.MapPath("/Content/MailFormat/mail.html"));
            productName = productName.Substring(0, productName.Length - 1);
            string mailbody = String.Format(mailform, orderObj.FullName, productName, DateTime.Now.ToString("HH:mm dd/MM/yyyy"), Utils.InsertCommaNoStyle(orderObj.OrderTotal.ToString()), orderObj.CustomerAddress, orderObj.Mobile);

            string mailformcustom = System.IO.File.ReadAllText(Server.MapPath("/Content/MailFormat/mailcustomer.html"));
            string mailbodycustom = String.Format(mailformcustom, orderObj.FullName, productName, DateTime.Now.ToString("HH:mm dd/MM/yyyy"), Utils.InsertCommaNoStyle(orderObj.OrderTotal.ToString()), orderObj.CustomerAddress, orderObj.Mobile, Utils.InsertCommaNoStyle(orderObj.OrderPrice.ToString()), orderNo);

            Action<string, string, string> send = (string subject, string body, string email) =>
            {
                EmailService.SendMail(subject, body, email);

            };
            send.BeginInvoke(mailsubject, mailbodycustom, orderObj.Email, null, null);
            send.BeginInvoke(mailsubject, mailbody, ConfigurationManager.AppSettings["WebsiteEmail"].ToString(), null, null);


            OrderHelper.ClearCart();
            WorkContext.RemoveSessionKey(OrderConstants.SessionOrderModelKey);
            return RedirectToAction("PreOrderInfo");


        }

        public ActionResult ConfirmPayment()
        {

            return PartialView();
        }
        public ActionResult ShoppingCartDelete()
        {
            OrderHelper.ClearCart();
            WorkContext.RemoveSessionKey(OrderConstants.SessionOrderModelKey);
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// giỏ hàng trống
        /// </summary>
        /// <returns></returns>
        public ActionResult CartEmpty()
        {
            var shoppingCart = OrderHelper.GetShopingCart();
            if (shoppingCart.ListProduct.Count > 0)
                return Redirect("Conrfirm");
            return RedirectToAction("Index", "Home");

        }

        #region MappingProduct
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CartProductUpdate(int productId, int count)
        {
            try
            {
                var cart = OrderHelper.GetShopingCart();
                cart.SetProductQuantity(productId, count);
                OrderHelper.SetShopingCart(cart);
                var orderModel = WorkContext.GetSessionKey(OrderConstants.SessionOrderModelKey);
                if (orderModel != null)
                {
                    var curentModel = orderModel as ShoppingCartModel;
                    curentModel.ListProduct = GetMappingOrderProductModel();
                    curentModel.ListCoupon = GetMappinpOrderCouponModel();
                    curentModel.ExtraData = GetOrderExtraData();
                    WorkContext.SetSessionKey(OrderConstants.SessionOrderModelKey, curentModel);
                }
                //return PartialView("_ShoppingCart");
                var returnData = new ReturnData
                {
                    ResponseCode = 1,
                    Description = "Success"
                };
                return Json(returnData);
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "Order", "CartProductUpdate");
                return Json(new ReturnData
                {
                    ResponseCode = -99,
                    Description = "Error"
                });
            }

        }
        [ValidateAntiForgeryToken]
        public ActionResult CartProductUpdateInfo(OrderCustomer data)
        {


            var orderModel = WorkContext.GetSessionKey(OrderConstants.SessionOrderModelKey);
            if (orderModel != null)
            {
                var curentModel = orderModel as ShoppingCartModel;
                curentModel.ListProduct = GetMappingOrderProductModel();
                curentModel.ListCoupon = GetMappinpOrderCouponModel();
                curentModel.OderItem = data;
                WorkContext.SetSessionKey(OrderConstants.SessionOrderModelKey, curentModel);
            }
            var returnData = new ReturnData
            {
                ResponseCode = 1,
                Description = "Success"
            };
            return Json(returnData);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CartProductDelete(int productId)
        {
            var cart = OrderHelper.GetShopingCart();
            cart.RemoveProduct(productId);
            OrderHelper.SetShopingCart(cart);
            var orderModel = WorkContext.GetSessionKey(OrderConstants.SessionOrderModelKey);
            if (orderModel != null)
            {
                var curentModel = orderModel as ShoppingCartModel;
                curentModel.ListProduct = GetMappingOrderProductModel();
                curentModel.ListCoupon = GetMappinpOrderCouponModel();
                curentModel.ExtraData = GetOrderExtraData();
                WorkContext.SetSessionKey(OrderConstants.SessionOrderModelKey, curentModel);

            }
            var returnData = new ReturnData
            {
                ResponseCode = 1,
                Description = "Thành công"
            };
            return Json(returnData);
        }

        public ActionResult AddProduct(int productId, int amount)
        {

            var product = new ProductBO().GetProduct(productId);
            if (product == null || product.Status.GetValueOrDefault() == 0)
                return RedirectToAction("Error", "Home");



            ShoppingCart cart = OrderHelper.GetShopingCart();
            var newitem = new OrderProductMapping_Full
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                ProductImage = product.DescriptImage,
                Amount = amount
            };
            cart.AddProductItem(newitem);
            OrderHelper.SetShopingCart(cart);
            return RedirectToAction("ShoppingCart");

        }
        #endregion
        #region MappingCoupon
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CartCouponDelete(int id)
        {
            var cart = OrderHelper.GetShopingCart();
            cart.RemoveCoupon(id);
            OrderHelper.SetShopingCart(cart);

            var orderModel = WorkContext.GetSessionKey(OrderConstants.SessionOrderModelKey);
            if (orderModel != null)
            {
                var curentModel = orderModel as ShoppingCartModel;
                curentModel.ListProduct = GetMappingOrderProductModel();
                curentModel.ListCoupon = GetMappinpOrderCouponModel();
                curentModel.ExtraData = GetOrderExtraData();
                WorkContext.SetSessionKey(OrderConstants.SessionOrderModelKey, curentModel);

            }
            var returnData = new ReturnData
            {
                ResponseCode = 1,
                Description = "Thành công"
            };
            return Json(returnData);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CartCouponAdd(string code)
        {
            var returnData = new ReturnData
            {
                ResponseCode = -1,
                Description = "Lỗi hệ thống"
            };
            //string message = "";
            try
            {
                var extraData = GetOrderExtraData();
                ShoppingCart cart = OrderHelper.GetShopingCart();
                //if (cart.ListProduct.Count == 0 || cart.ListCoupon.Count > 0)
                //{
                //    return RedirectToAction("Index", "Product");
                //}
                var coupon = new CouponBO().GetCoupon(code);
                if (coupon == null)
                {
                    returnData.Description = "Mã giảm giá không đúng";
                }

                else
                {
                    if (coupon.Status == 0)
                    {
                        returnData.Description = "Mã giảm giá đã bị khóa";

                    }

                    else if (coupon.CurentNumberUsed >= coupon.MaxNumberUsed)
                    {
                        returnData.Description = "Mã giảm giá đã hết lượt sử dụng";
                    }

                    else
                    {

                        if (cart.ListCoupon.Count > 1)
                        {
                            returnData.Description = "Mã giảm giá đã hết lượt sử dụng";
                        }
                        else
                        {
                            var newitem = new OrderCouponMapping
                            {
                                CouponCode = coupon.Code,
                                CouponId = coupon.Id,
                                CurrencyCosts = coupon.Costs
                            };
                            cart.AddCouponItem(newitem);
                            OrderHelper.SetShopingCart(cart);
                            returnData.ResponseCode = 1;
                            var orderModel = WorkContext.GetSessionKey(OrderConstants.SessionOrderModelKey);
                            if (orderModel != null)
                            {
                                var curentModel = orderModel as ShoppingCartModel;
                                curentModel.ListProduct = GetMappingOrderProductModel();
                                curentModel.ListCoupon = GetMappinpOrderCouponModel();
                                curentModel.ExtraData = GetOrderExtraData();
                                WorkContext.SetSessionKey(OrderConstants.SessionOrderModelKey, curentModel);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "Order", "CartCouponAdd");
            }

            return Json(returnData);
        }
        #endregion
        private OrderExtraData GetOrderExtraData()
        {

            var productItems = GetMappingOrderProductModel();

            var dataExtra = new OrderExtraData();
            dataExtra.OrderPrice = productItems.Sum(q => q.Price.GetValueOrDefault() * q.Amount);
            dataExtra.OrderBilling = dataExtra.OrderPrice;
            #region Tính Giá trị coupon nếu có
            foreach (var item in OrderHelper.GetShopingCart().ListCoupon)
            {
                dataExtra.OrderBilling = dataExtra.OrderBilling - ((dataExtra.OrderPrice * item.CurrencyCosts.GetValueOrDefault()) / 100);

            }
            #endregion

            return dataExtra;
        }
        private List<OrderProductMapping_Full> GetMappingOrderProductModel()
        {

            var listCartItem = OrderHelper.GetShopingCart().ListProduct;
            var listData = new List<OrderProductMapping_Full>();
            foreach (OrderProductMapping_Full item in listCartItem)
            {
                var productItem = new OrderProductMapping_Full();

                productItem.ProductId = item.ProductId;
                productItem.ProductName = item.ProductName;
                productItem.Amount = item.Amount;
                productItem.Price = item.Price;
                productItem.ProductImage = item.ProductImage;

                listData.Add(productItem);
            }
            return listData;
        }

        private List<OrderCouponMapping> GetMappinpOrderCouponModel()
        {

            var listCartCoupon = OrderHelper.GetShopingCart().ListCoupon;
            var listData = new List<OrderCouponMapping>();
            foreach (var item in listCartCoupon)
            {
                var couponItem = new OrderCouponMapping
                {
                    CouponCode = item.CouponCode,
                    CouponId = item.CouponId,
                    CurrencyCosts = item.CurrencyCosts,
                };

                listData.Add(couponItem);
            }
            return listData;
        }



    }
}
