using System;
using System.Linq;
using System.Text;
using System.Web;
using BIZ;
using BIZ.Entity;
using UTILS;

namespace Local.Post
{
    /// <summary>
    /// Summary description for ProductOrderService
    /// </summary>
    public class ProductOrderService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            ResponseMsg responseMsg = new ResponseMsg();
            //StringBuilder stringBuilder;
            ProductOrderBO productOrderBo = new ProductOrderBO();
            try
            {
                string method = context.Request["__m"];
                int return_val = 0;
                //string title;
                string published;
                //string ordering;
                switch (method.ToLower())
                {
                    case "save":
                        var productOrderId = context.Request.Form["id"];
                        published = context.Request.Form["published"];

                        if (!Utils.IsNumber(productOrderId))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình lấy thông tin";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        var productOrder = new PRODUCTORDER_FULL();

                        if (Convert.ToInt32(productOrderId) > 0)
                            productOrder = productOrderBo.GetProductOrderFull(Convert.ToInt32(productOrderId));

                        productOrder.Id = Convert.ToInt32(productOrderId);

                        productOrder.Published = Convert.ToByte(published);                 

                        return_val = productOrderBo.CreateUpdateProductOrder(productOrder);

                        if (return_val != -1)
                        {
                            responseMsg.Success = true;
                            responseMsg.Value = Convert.ToString(return_val);
                            responseMsg.Text = "Lưu thông tin thành công";
                        }
                        else
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình xử lý";
                        }

                        context.Response.Write(responseMsg.ToJsonString());
                        return;

                    case "delete":
                        var arrProductOrderId = context.Request.Form.GetValues("id[]");
                        if (arrProductOrderId == null || arrProductOrderId.Count() == 0)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Bạn phải chọn ít nhất 1 chuyên mục";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        if (arrProductOrderId.Count() == 1)
                        {
                            return_val = productOrderBo.DeleteProductOrder(Convert.ToInt32(arrProductOrderId[0]));
                        }
                        else
                        {
                            string joinId = string.Empty;
                            foreach (var id in arrProductOrderId)
                            {
                                if (Utils.IsNumber(id))
                                    joinId += "," + id;
                            }

                            if (string.IsNullOrEmpty(joinId))
                            {
                                responseMsg.Success = false;
                                responseMsg.Text = "Bạn phải chọn ít nhất 1 đơn hàng";
                                context.Response.Write(responseMsg.ToJsonString());
                                return;
                            }

                            joinId = joinId.TrimStart(',');
                            return_val = productOrderBo.DeleteProductOrders(joinId);
                        }

                        if (return_val != -1)
                        {
                            responseMsg.Success = true;

                            responseMsg.Text = "Xóa đơn hàng sản phẩm thành công";
                        }
                        else
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình xử lý";
                        }

                        context.Response.Write(responseMsg.ToJsonString());
                        return;
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                responseMsg.Success = false;
                responseMsg.Text = "Có lỗi trong quá trình xử lý - execption";
                context.Response.Write(responseMsg.ToJsonString());
                return;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}