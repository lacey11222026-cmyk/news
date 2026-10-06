using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BIZ;
using BIZ.Entity;
using DATA;
using UTILS;

namespace WebMVC4.Post
{
    /// <summary>
    /// Summary description for ProductService
    /// </summary>
    public class ProductService : IHttpHandler, IReadOnlySessionState
    {
        private delegate int DelegateUpdate(PRODUCTATTRIBUTE_FULL productattributeFull);
        private delegate string DelegateDeleteImages(HttpRequest request, int id, string entityName);

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            ResponseMsg responseMsg = new ResponseMsg();
            ProductBO productBo = new ProductBO();
            try
            {

                if (!HttpContext.Current.User.IsInRole("Administrator") && !HttpContext.Current.User.IsInRole("Sale"))
                {
                    responseMsg.Success = false;
                    responseMsg.Text = "Không có quyền";
                    context.Response.Write(responseMsg.ToJsonString());
                    return;
                }
                string method = context.Request["__m"];
                int return_val;
                switch (method.ToLower())
                {
                    case "save":

                        var productId = context.Request.Form["id"];
                        var title = HttpUtility.UrlDecode(context.Request.Form["title"]);
                        var code = HttpUtility.UrlDecode(context.Request.Form["code"]);
                        var categoryId = context.Request.Form["categoryid"];
                        var categoryPathway = context.Request.Form["categorypathway"];
                        var manufactoryId = context.Request.Form["manufactoryid"];
                        var intro = HttpUtility.UrlDecode(context.Request.Form["intro"]);
                        var price = context.Request.Form["price"];
                        var count = context.Request.Form["count"];
                        var published = context.Request.Form["published"];
                        var ordering = context.Request.Form["ordering"];

                        if (!Utils.IsNumber(productId) || !Utils.IsNumber(categoryId) || !Utils.IsNumber(price) || !Utils.IsNumber(count) || !Utils.IsNumber(published) || !Utils.IsNumber(ordering))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình lấy thông tin";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        var product = new PRODUCT_FULL();

                        if (Convert.ToInt32(productId) > 0)
                        {
                            product = productBo.GetProductFull(Convert.ToInt32(productId));
                        }
                        else
                        {
                            product.CreatedDate = DateTime.Now;
                        }

                        product.Id = Convert.ToInt32(productId);
                        product.Title = title;
                        product.Name = Utils.ReplaceVietnameseChar(title).Replace(' ', '-');
                        product.Alias = Utils.ReplaceVietnameseChar(title).Replace(' ', '-');
                        product.ProductCode = HttpUtility.UrlDecode(code);

                        if (Convert.ToInt32(categoryId) != 0)
                        {
                            product.CategoryId = Convert.ToInt32(categoryId);
                            product.CategoryPathway = categoryPathway; 
                        }
                        else
                        {
                            product.CategoryId = 0;

                        }

                        if (Convert.ToInt32(manufactoryId) != 0)
                            product.ManufactoryId = Convert.ToInt32(manufactoryId);
                        else
                            product.ManufactoryId = null;

                        product.IntroText = intro;

                        // update price date
                        if (product.Price != Convert.ToDouble(price))
                            product.PriceModifyDate = DateTime.Now;

                        product.Price = Convert.ToDouble(price);
                        product.Count = Convert.ToInt32(count);
                        product.Published = Convert.ToByte(published);
                        product.Ordering = Convert.ToByte(ordering);
                        product.ModifiedDate = DateTime.Now;

                        //if new product is applied
                        if (product.Id > 0)
                        {
                            string[] arrAttr = context.Request.Form.GetValues("att[]");

                            if (arrAttr != null)
                            {
                                List<PRODUCTATTRIBUTE_FULL> lstProductAttribute = new List<PRODUCTATTRIBUTE_FULL>();
                                foreach (var attr in arrAttr)
                                {
                                    if (string.IsNullOrEmpty(attr))
                                        continue;

                                    var splitCommaArr = attr.Split('|'); // Eleemtn index: 0 : attributeid,  1: value 

                                    PRODUCTATTRIBUTE_FULL productAttribute = new PRODUCTATTRIBUTE_FULL();
                                    productAttribute.Id = 0;

                                    if (!Utils.IsNumber(splitCommaArr[0]) || string.IsNullOrEmpty(splitCommaArr[1]))
                                        continue;

                                    productAttribute.AttributeId = Convert.ToInt32(splitCommaArr[0]);
                                    productAttribute.ProductId = Convert.ToInt32(product.Id);

                                    if (Utils.IsNumber(splitCommaArr[1]))
                                        productAttribute.NumbericValue = Convert.ToDouble(splitCommaArr[1]);
                                    else
                                        productAttribute.NumbericValue = 0;

                                    productAttribute.TextValue = Utils.FormatTextValue(splitCommaArr[1]);
                                    productAttribute.Ordering = 0;

                                    // update product attribute
                                    //DelegateUpdate delegateUpdate = new ProductAttributeBO ().CreateUpdateProductAttribute;
                                    //delegateUpdate.BeginInvoke ( productAttribute, null, null );

                                    return_val = new ProductAttributeBO().CreateUpdateProductAttribute(productAttribute);
                                    if (return_val == -1)
                                        continue;

                                    // re-add od
                                    productAttribute.Id = return_val;
                                    lstProductAttribute.Add(productAttribute);
                                }

                                var json = Utils.ConvertToJson(lstProductAttribute, string.Empty);
                                product.Attributes = json;
                            }
                        }

                        product.FullText = HttpUtility.UrlDecode(context.Request.Form["fulltext"]);
                        //product.PromotionText = context.Request.Form["promotiontext"];

                        //var _params = context.Request.Form.GetValues("params[]");
                        //Param param = new Param();
                        //param.SiteTitle = _params[0];
                        //param.MetaDescription = _params[1];
                        //param.MetaKeywords = _params[2];
                        //product.Params = Utils.ConvertToJson(param, string.Empty);

                        var mainImage = context.Request.Form["mainimage"];
                        var images = context.Request.Form.GetValues("image[]");


                        var listImages = string.Empty;

                        if (images != null)
                        {
                            if (!string.IsNullOrEmpty(mainImage))
                                listImages = mainImage + ",";

                            foreach (var image in images)
                            {
                                if (image != mainImage)
                                    listImages += image + ",";
                            }

                            listImages = listImages.TrimEnd(',');
                        }

                        product.Images = listImages;

                        return_val = productBo.CreateUpdateProduct(product);

                        if (return_val != -1)
                        {
                            if (return_val > 0)
                            {
                                string[] arrAttr = context.Request.Form.GetValues("att[]");

                                if (arrAttr != null)
                                {
                                    List<PRODUCTATTRIBUTE_FULL> lstProductAttribute = new List<PRODUCTATTRIBUTE_FULL>();
                                    foreach (var attr in arrAttr)
                                    {
                                        if (string.IsNullOrEmpty(attr))
                                            continue;

                                        var splitCommaArr = attr.Split('|'); // Eleemtn index: 0 : attributeid,  1: value 

                                        PRODUCTATTRIBUTE_FULL productAttribute = new PRODUCTATTRIBUTE_FULL();
                                        productAttribute.Id = 0;

                                        if (!Utils.IsNumber(splitCommaArr[0]) || string.IsNullOrEmpty(splitCommaArr[1]))
                                            continue;

                                        productAttribute.AttributeId = Convert.ToInt32(splitCommaArr[0]);
                                        productAttribute.ProductId = Convert.ToInt32(return_val);

                                        if (Utils.IsNumber(splitCommaArr[1]))
                                            productAttribute.NumbericValue = Convert.ToDouble(splitCommaArr[1]);
                                        else
                                            productAttribute.NumbericValue = 0;

                                        productAttribute.TextValue = Utils.FormatTextValue(splitCommaArr[1]);
                                        productAttribute.Ordering = 0;

                                        // update product attribute
                                        //DelegateUpdate delegateUpdate = new ProductAttributeBO ().CreateUpdateProductAttribute;
                                        //delegateUpdate.BeginInvoke ( productAttribute, null, null );

                                        var return_val1 = new ProductAttributeBO().CreateUpdateProductAttribute(productAttribute);
                                        if (return_val1 == -1)
                                            continue;

                                        // re-add od
                                        productAttribute.Id = return_val1;
                                        lstProductAttribute.Add(productAttribute);
                                    }

                                    var json = Utils.ConvertToJson(lstProductAttribute, string.Empty);

                                    productBo.UpdateAttributes(return_val, json);
                                }

                            }


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
                        var arrProductId = context.Request.Form.GetValues("id[]");
                        if (arrProductId == null || arrProductId.Count() == 0)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Bạn phải chọn ít nhất 1 sản phẩm để xóa";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        if (arrProductId.Count() == 1)
                        {
                            return_val = productBo.DeleteProduct(Convert.ToInt32(arrProductId[0]));
                            if (return_val != -1)
                            {
                                var type = ConfigurationManager.AppSettings["EnableFTP"];

                                if (type == "1")
                                {
                                    DelegateDeleteImages delegateDeleteImages = Utils.DeleteFilesFTP;
                                    delegateDeleteImages.BeginInvoke(context.Request, Convert.ToInt32(arrProductId[0]), "Product", null, null);
                                }
                                else
                                {
                                    DelegateDeleteImages delegateDeleteImages = Utils.DeleteFiles;
                                    delegateDeleteImages.BeginInvoke(context.Request, Convert.ToInt32(arrProductId[0]), "Product", null, null);
                                }

                            }
                        }
                        else
                        {
                            string joinId = string.Empty;
                            foreach (var id in arrProductId)
                            {
                                if (Utils.IsNumber(id))
                                    joinId += "," + id;
                            }

                            if (string.IsNullOrEmpty(joinId))
                            {
                                responseMsg.Success = false;
                                responseMsg.Text = "Bạn phải chọn ít nhất 1 sản phẩm để xóa";
                                context.Response.Write(responseMsg.ToJsonString());
                                return;
                            }

                            joinId = joinId.TrimStart(',');
                            return_val = productBo.DeleteProducts(joinId);
                            if (return_val != -1)
                            {
                                DelegateDeleteImages delegateDeleteImages = Utils.DeleteFiles;
                                string[] arrJoinId = joinId.Split(',');
                                foreach (var joinid in arrJoinId)
                                {
                                    delegateDeleteImages.BeginInvoke(context.Request, Convert.ToInt32(joinid), "Product", null, null);
                                }

                            }
                        }

                        if (return_val != -1)
                        {
                            responseMsg.Success = true;
                            responseMsg.Text = "Xóa sản phẩm thành công";
                        }
                        else
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình xử lý : Có thể bạn chưa xóa hết các phần liên quan";
                        }

                        context.Response.Write(responseMsg.ToJsonString());
                        return;


                }
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "ProductServicePost", "ProductService");
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