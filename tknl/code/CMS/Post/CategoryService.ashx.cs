using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using BIZ;
using BIZ.Entity;
using UTILS;
using DATA;

namespace CMS.Post
{
    /// <summary>
    /// Summary description for CategoryService
    /// </summary>
    public class CategoryService : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            ResponseMsg responseMsg = new ResponseMsg();
            StringBuilder stringBuilder;
            CategoryBO categoryBo = new CategoryBO();
            try
            {
                string method = context.Request["__m"];
                int return_val;
                var published = context.Request.Form["published"];
                var ordering = context.Request.Form["ordering"];
                var language = context.Request.Form["language"];

                if (!HttpContext.Current.User.IsInRole("Administrator") && !HttpContext.Current.User.IsInRole("Category"))
                {
                    responseMsg.Success = false;
                    responseMsg.Text = "Không có quyền";
                    context.Response.Write(responseMsg.ToJsonString());
                    return;
                }
                switch (method.ToLower())
                {
                    case "updatecontent":

                        var cateId = context.Request.Form["id"];
                        var content = HttpUtility.UrlDecode(context.Request.Form["content"]);

                        var categoryobj = categoryBo.GetCategoryFull(Convert.ToInt32(cateId));
                        
                        categoryobj.Contents = content;
                        categoryobj.Published = Convert.ToByte(published);
                        categoryobj.Ordering = Convert.ToByte(ordering);
                        categoryobj.Language = language;
                        return_val = categoryBo.UpdateContent(categoryobj);

                        if (return_val != -1)
                        {
                            responseMsg.Success = true;
                            responseMsg.Value = return_val.ToString();
                            responseMsg.Text = "Lưu thông tin thành công";
                        }
                        else
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình xử lý";
                        }

                        context.Response.Write(responseMsg.ToJsonString());
                        return;
                    case "save":

                        var categoryId = context.Request.Form["id"];
                        var title = HttpUtility.UrlDecode(context.Request.Form["title"]);
                        var parentId = context.Request.Form["parentid"];
                        var type = context.Request.Form["type"];
                        var description = HttpUtility.UrlDecode(context.Request.Form["description"]);
                        var link = HttpUtility.UrlDecode(context.Request.Form["link"]);
                        //var published = context.Request.Form["published"];
                        //var ordering = context.Request.Form["ordering"];
                        //var language = context.Request.Form["language"];
                        var _params = context.Request.Form.GetValues("params[]");

                        if (!Utils.IsNumber(categoryId) || !Utils.IsNumber(categoryId) || !Utils.IsNumber(published) || !Utils.IsNumber(ordering) || !Utils.IsNumber(type))
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình lấy thông tin";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        var category = new CATEGORY_FULL();

                        if (Convert.ToInt32(categoryId) > 0)
                        {
                            category = categoryBo.GetCategoryFull(Convert.ToInt32(categoryId));
                        }
                        else
                        {
                            category.CreateDate = DateTime.Now;
                        }

                        category.Id = Convert.ToInt32(categoryId);
                        category.ParentId = Convert.ToInt32(parentId);
                        category.Type = Convert.ToByte(type);
                        stringBuilder = new StringBuilder();
                        stringBuilder.Append(categoryBo.GetPathway(int.Parse(category.ParentId.ToString()))).Append("/").Append(parentId);

                        category.Name = title;

                        category.Link = link;
                        category.Description = description;
                        category.Language = language;
                        category.Published = Convert.ToByte(published);
                        category.Ordering = Convert.ToByte(ordering);
                        category.ModifiedDate = DateTime.Now;
                        //category.Count = 0;

                        CategoryParam param = new CategoryParam();
                        


                        //if (Utils.IsNumber(_params[0]))
                        //    param.IsHomepage = Convert.ToByte(_params[0]);
                        //else
                        //    param.IsHomepage = 0;

                        //if (Utils.IsNumber(_params[1]))
                        //    param.IsRightCol = Convert.ToByte(_params[1]);
                        //else
                        //    param.IsRightCol = 0;

                        //if (Utils.IsNumber(_params[2]))
                        //    param.IsMainMenu = Convert.ToByte(_params[2]);
                        //else
                        //    param.IsMainMenu = 0;

                        //if (Utils.IsNumber(_params[3]))
                        //    param.IsTopMenu = Convert.ToByte(_params[3]);
                        //else
                        //    param.IsTopMenu = 0;

                        //if (Utils.IsNumber(_params[4]))
                        //    param.IsFooter = Convert.ToByte(_params[4]);
                        //else
                        //    param.IsFooter = 0;

                        //category.Params = Utils.ConvertToJson(param, string.Empty);




                        return_val = categoryBo.CreateUpdateCategory(category);

                        if (return_val != -1)
                        {
                            responseMsg.Success = true;
                            responseMsg.Value = return_val.ToString();
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
                        var arrCategoryId = context.Request.Form.GetValues("id[]");
                        if (arrCategoryId == null || arrCategoryId.Count() == 0)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Bạn phải chọn ít nhất 1 chuyên mục";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        if (arrCategoryId.Count() == 1)
                        {
                            var lstChildCategories = categoryBo.GetAllChildCategories(Convert.ToInt32(arrCategoryId[0]), 0, false);
                            if (lstChildCategories != null && lstChildCategories.Count > 0)
                            {
                                var lstChildTitle = string.Empty;
                                foreach (var lstChildCategory in lstChildCategories)
                                {
                                    lstChildTitle += categoryBo.GetTitle(lstChildCategory.Id) + ",";
                                }

                                lstChildTitle.Trim().TrimEnd(',');

                                responseMsg.Success = false;
                                responseMsg.Text = "chuyên mục này đang là đang có liên quan đến chuyên mục <b>" + lstChildTitle + "</b>";
                                context.Response.Write(responseMsg.ToJsonString());
                                return;
                            }
                            return_val = categoryBo.DeleteCategory(Convert.ToInt32(arrCategoryId[0]));
                        }
                        else
                        {
                            string joinId = string.Empty;
                            foreach (var id in arrCategoryId)
                            {
                                var lstChildCategories = categoryBo.GetAllChildCategories(Convert.ToInt32(id), 0, false);
                                if (lstChildCategories != null && lstChildCategories.Count > 0)
                                    continue;
                                if (Utils.IsNumber(id))
                                    joinId += "," + id;
                            }

                            if (string.IsNullOrEmpty(joinId))
                            {
                                responseMsg.Success = false;
                                responseMsg.Text = "Bạn phải chọn ít nhất 1 chuyên mục";
                                context.Response.Write(responseMsg.ToJsonString());
                                return;
                            }

                            joinId = joinId.TrimStart(',');
                            return_val = categoryBo.DeleteCategories(joinId);
                        }

                        if (return_val == -1)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Có lỗi trong quá trình xử lý";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;

                        }

                        if (return_val == -2)
                        {
                            responseMsg.Success = false;
                            responseMsg.Text = "Một trong số chuyên mục đang liên quan đến một số chức năng khác ! Hãy xóa hoặc thay đổi các chức năng liên quan đến chuyên mục để thực hiện việc xóa thành thành công !";
                            context.Response.Write(responseMsg.ToJsonString());
                            return;
                        }

                        responseMsg.Success = true;
                        responseMsg.Text = "Xóa chuyên mục thành công";
                        context.Response.Write(responseMsg.ToJsonString());
                        return;


                }
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "CategoryServicePost", "CategoryService");
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