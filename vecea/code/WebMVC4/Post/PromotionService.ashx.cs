using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using BIZ;
using BIZ.Entity;
using UTILS;

namespace WebMVC4.Post
{
    /// <summary>
    /// Summary description for PromotionService
    /// </summary>
    public class PromotionService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            ResponseMsg responseMsg = new ResponseMsg();
            //StringBuilder stringBuilder;
            PromotionBO promotionBo = new PromotionBO();
            try
            {
                string method = context.Request["__m"];
                int return_val;
                switch (method.ToLower())
                {
                    case "save":
                        var promotionId = context.Request.Form["id"];
                        var categoryId = context.Request.Form["categoryid"];
                        var promotionCode = HttpUtility.HtmlDecode(context.Request.Form["promotioncode"]);
                        string published = context.Request.Form["published"];
                        var intro = context.Request.Form["intro"];
                        var fulltext = context.Request.Form["fulltext"];
                        var startdate = context.Request.Form["startdate"];
                        var enddate = context.Request.Form["enddate"];
                        var bonustype = context.Request.Form["bonustype"];
                        var bonusval = context.Request.Form["bonusval"];
                        var productIds = context.Request.Form["productids"];

                        if (!Utils.IsNumber(promotionId) || !Utils.IsNumber(bonusval))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình lấy thông tin";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        var promotion = new PROMOTION_FULL();
                        if (Convert.ToInt32(promotionId) > 0)
                            promotion = promotionBo.GetPromotionFull(Convert.ToInt32(promotionId));

                        promotion.Id = Convert.ToInt32(promotionId);
                        if (Convert.ToInt32(categoryId) != 0)
                            promotion.CategoryId = Convert.ToInt32(categoryId);
                        else
                            promotion.CategoryId = null;
                        //promotion.CategoryId = Convert.ToInt32 ( categoryId );
                        promotion.PromotionCode = promotionCode;
                        promotion.IntroText = intro;
                        promotion.FullText = fulltext;

                        // format datetime
                        CultureInfo culInfo = new CultureInfo("vi-VN", true);
                        string today = DateTime.Now.ToString("dd/MM/yyyy");
                        try
                        {
                            promotion.StartDate = Convert.ToDateTime(startdate, culInfo);
                        }
                        catch (Exception)
                        {
                            promotion.StartDate = Convert.ToDateTime(today, culInfo);
                        }
                        try
                        {
                            promotion.EndDate = Convert.ToDateTime(enddate, culInfo);
                        }
                        catch (Exception)
                        {
                            promotion.EndDate = Convert.ToDateTime(today, culInfo);
                        }

                        promotion.BonusType = Convert.ToByte(bonustype);
                        promotion.BonusValue = Convert.ToByte(bonusval);
                        promotion.Published = Convert.ToByte(published);

                        string _productIds = string.Empty;
                        if (!string.IsNullOrEmpty(productIds))
                        {
                            string[] arrProductIds = productIds.Split(',');
                            foreach (var productId in arrProductIds)
                            {
                                if (Utils.IsNumber(productId))
                                {
                                    _productIds += "," + productId;
                                }

                                _productIds = _productIds.TrimStart(',');
                            }
                        }

                        promotion.ProductId = _productIds;

                        return_val = promotionBo.CreateUpdatePromotion(promotion);

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
                        var arrPromotionId = context.Request.Form.GetValues("id[]");
                        if (arrPromotionId == null || arrPromotionId.Count() == 0)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Bạn phải chọn ít nhất 1";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        if (arrPromotionId.Count() == 1)
                        {
                            return_val = promotionBo.DeletePromotion(Convert.ToInt32(arrPromotionId[0]));
                        }
                        else
                        {
                            string joinId = string.Empty;
                            foreach (var id in arrPromotionId)
                            {
                                if (Utils.IsNumber(id))
                                    joinId += "," + id;
                            }

                            if (string.IsNullOrEmpty(joinId))
                            {
                                responseMsg.Success = false;
                                responseMsg.Text = "Bạn phải chọn ít nhất 1 thuộc tính";
                                context.Response.Write(responseMsg.ToJsonString());
                                return;
                            }

                            joinId = joinId.TrimStart(',');
                            return_val = promotionBo.DeletePromotions(joinId);
                        }

                        if (return_val != -1)
                        {
                            responseMsg.Success = true;
                            responseMsg.Text = "Xóa thành công";
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