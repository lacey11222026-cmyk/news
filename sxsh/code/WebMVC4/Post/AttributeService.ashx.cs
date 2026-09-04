using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using BIZ;
using BIZ.Entity;
using UTILS;
using Constants = UTILS.Constants;

namespace WebMVC4.Post
{
    /// <summary>
    /// Summary description for AttributeService
    /// </summary>
    public class AttributeService : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            ResponseMsg responseMsg = new ResponseMsg();
            //StringBuilder stringBuilder;
            AttributeBO attributeBo = new AttributeBO();
            AttributeGroupBO attributeGroupBo = new AttributeGroupBO();
            try
            {
                string method = context.Request["__m"];
                int return_val = 0;
                string title;
                string published;
                string ordering;
                switch (method.ToLower())
                {
                    case "save":
                        var attributeId = context.Request.Form["id"];
                        var groupId = context.Request.Form["groupid"];
                        var categoryId = context.Request.Form["categoryid"];
                        title = HttpUtility.UrlDecode(context.Request.Form["title"]);
                        //var language = context.Request.Form["language"];
                        published = context.Request.Form["published"];
                        ordering = context.Request.Form["ordering"];
                        var filterType = context.Request.Form["filtertype"];
                        var dataType = context.Request.Form["datatype"];
                        var unit = context.Request.Form["unit"];
                        var _filter = context.Request.Form.GetValues("filter[]");

                        if (!Utils.IsNumber(attributeId) || !Utils.IsNumber(ordering))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình lấy thông tin";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        var attribute = new ATTRIBUTE_FULL();

                        if (Convert.ToInt32(attributeId) > 0)
                            attribute = attributeBo.GetAttributeFull(Convert.ToInt32(attributeId));

                        attribute.Id = Convert.ToInt32(attributeId);
                        if (groupId != "0")
                            attribute.GroupId = Convert.ToInt32(groupId);
                        else
                            attribute.GroupId = null;

                        if (categoryId != "0")
                            attribute.CategoryId = Convert.ToInt32(categoryId);
                        else
                            attribute.CategoryId = null;

                        attribute.Title = title;
                        attribute.FilterType = Convert.ToByte(filterType);
                        switch (dataType)
                        {
                            case "0":
                                attribute.DataType = (byte)Constants.FilterDataType.String;
                                break;
                            case "1":
                                attribute.DataType = (byte)Constants.FilterDataType.Double;
                                break;
                            case "2":
                                attribute.DataType = (byte)Constants.FilterDataType.Bit;
                                attribute.FilterType = (int)Constants.FilterType.ByMultiValue;
                                break;
                            default:
                                attribute.DataType = (byte)Constants.FilterDataType.String;
                                break;
                        }

                        if (Convert.ToByte(filterType) == (byte)Constants.FilterType.ByRange)
                            attribute.DataType = (byte)Constants.FilterDataType.Double;

                        attribute.Published = Convert.ToByte(published);
                        attribute.Ordering = Convert.ToByte(ordering);
                        attribute.Unit = unit;
                        //attribute.Language = language;
                        

                        //attribute.Filter = Utils.ConvertToJson(filter, string.Empty);

                        return_val = attributeBo.CreateUpdateAttribute(attribute);

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
                    case "save_attrgroup":
                        var attributeGroupId = context.Request.Form["id"];
                        title = HttpUtility.UrlDecode(context.Request.Form["title"]);
                        published = context.Request.Form["published"];
                        ordering = context.Request.Form["ordering"];
                        var language = context.Request.Form["language"];
                        if (!Utils.IsNumber(attributeGroupId) || !Utils.IsNumber(ordering))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình lấy thông tin";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        var attributeGroup = new ATTRIBUTEGROUP_FULL();

                        if (Convert.ToInt32(attributeGroupId) > 0)
                            attributeGroup = attributeGroupBo.GetAttributeGroupFull(Convert.ToInt32(attributeGroupId));

                        attributeGroup.Id = Convert.ToInt32(attributeGroupId);
                        attributeGroup.Title = title;
                        attributeGroup.Published = Convert.ToByte(published);
                        attributeGroup.Ordering = Convert.ToByte(ordering);
                        attributeGroup.Language = language;
                        return_val = attributeGroupBo.CreateUpdateAttributeGroup(attributeGroup);

                        if (return_val != -1)
                        {
                            responseMsg.Success = true;
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
                        var arrAttributeId = context.Request.Form.GetValues("id[]");
                        if (arrAttributeId == null || arrAttributeId.Count() == 0)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Bạn phải chọn ít nhất 1 thuộc tính";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        if (arrAttributeId.Count() == 1)
                        {
                            return_val = attributeBo.DeleteAttribute(Convert.ToInt32(arrAttributeId[0]));
                        }
                        else
                        {
                            string joinId = string.Empty;
                            foreach (var id in arrAttributeId)
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
                            return_val = attributeBo.DeleteAttributes(joinId);
                        }

                        if (return_val != -1)
                        {
                            responseMsg.Success = true;
                            responseMsg.Text = "Xóa thuộc tính sản phẩm thành công";
                        }
                        else
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình xử lý";
                        }

                        context.Response.Write(responseMsg.ToJsonString());
                        return;
                    case "delete-attributegroup":
                        var arrAttributeGroupId = context.Request.Form.GetValues("id[]");
                        if (arrAttributeGroupId == null || arrAttributeGroupId.Count() == 0)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Bạn phải chọn ít nhất 1 nhóm thuộc tính sản phẩm";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        if (arrAttributeGroupId.Count() == 1)
                        {
                            return_val = attributeGroupBo.DeleteAttributeGroup(Convert.ToInt32(arrAttributeGroupId[0]));
                        }
                        else
                        {
                            string joinId = string.Empty;
                            foreach (var id in arrAttributeGroupId)
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
                            return_val = attributeGroupBo.DeleteAttributeGroups(joinId);
                        }

                        if (return_val != -1)
                        {
                            responseMsg.Success = true;
                            responseMsg.Text = "Xóa nhóm thuộc tính sản phẩm thành công";
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
                ExHandler.Handle(ex, "AttributeServicePost", "AttributeService");
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