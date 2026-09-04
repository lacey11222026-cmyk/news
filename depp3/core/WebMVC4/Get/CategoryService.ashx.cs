using System;
using System.Web;
using System.Web.Services;
using BIZ;
using UTILS;
using Constants = UTILS.Constants;

namespace WebMVC4.Get
{

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CategoryService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                // get current page and record per page                               
                string method = context.Request["__m"];
               
                string categoryId;
                switch (method.ToLower())
                {
                    case "get_all_categories":
                        var _categoryType = context.Request["ctype"];
                       
                        if (string.IsNullOrEmpty(_categoryType))
                            _categoryType = "-1";

                        UTILS.Constants.CategoryType categoryType;
                        switch (Convert.ToInt32(_categoryType))
                        {
                            case -1:
                                categoryType = Constants.CategoryType.None;
                                break;
                            case 0:
                                categoryType = Constants.CategoryType.Product;
                                break;
                            case 1:
                                categoryType = Constants.CategoryType.Intro;
                                break;
                            case 2:
                                categoryType = Constants.CategoryType.News;
                                break;
                            case 4:
                                categoryType = Constants.CategoryType.Album;
                                break;
                            case 3:
                                categoryType = Constants.CategoryType.Other;
                                break;
                            case 5:
                                categoryType = Constants.CategoryType.Doc;
                                break;
                            case 6:
                                categoryType = Constants.CategoryType.Contact;
                                break;
                            default:
                                categoryType = Constants.CategoryType.None;
                                break;
                        }


                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(new CategoryBO().GetAllCategoriesFull(categoryType), string.Empty) + ")");
                        return;
                    case "get_all_categories_user":
                        var _categoryType2 = context.Request["ctype"];

                        if (string.IsNullOrEmpty(_categoryType2))
                            _categoryType2 = "-1";

                        UTILS.Constants.CategoryType categoryType2;
                        switch (Convert.ToInt32(_categoryType2))
                        {
                            case -1:
                                categoryType2 = Constants.CategoryType.None;
                                break;
                            case 0:
                                categoryType2 = Constants.CategoryType.Product;
                                break;
                            case 1:
                                categoryType2 = Constants.CategoryType.Intro;
                                break;
                            case 2:
                                categoryType2 = Constants.CategoryType.News;
                                break;
                            case 4:
                                categoryType2 = Constants.CategoryType.Album;
                                break;
                            case 3:
                                categoryType2 = Constants.CategoryType.Other;
                                break;
                            case 5:
                                categoryType2 = Constants.CategoryType.Doc;
                                break;
                            case 6:
                                categoryType2 = Constants.CategoryType.Contact;
                                break;
                            default:
                                categoryType2 = Constants.CategoryType.None;
                                break;
                        }

                        var data = new CategoryBO().GetAllCategoriesFull(categoryType2);
                        //data = new CategoryBO().GetCategoryByUserName(data, HttpContext.Current.User.Identity.Name,
                                                                              //HttpContext.Current.User.IsInRole("Administrator"));
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(data, string.Empty) + ")");
                        return;
                    case "get_category":
                        categoryId = context.Request["_id"];
                        if (!Utils.IsNumber(categoryId))
                        {
                            context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
                            return;
                        }

                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(new CategoryBO().GetCategoryFull(Convert.ToInt32(categoryId)), string.Empty) + ")");
                        return;

                    case "get_categories_paged":
                        var pageIndex = context.Request["_pi"];
                        var pageSize = context.Request["_ps"];
                        if (!Utils.IsNumber(pageIndex))
                            pageIndex = "1";
                        if (!Utils.IsNumber(pageSize))
                            pageSize = "5";

                        //var totalRecords = 0;
                        var json = new CategoryBO().GetAllCategoriesFullPaged_Json(Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize));
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + json + ")");
                        return;

                }
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "CategoryServiceGet", "CategoryService");
                context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
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