using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class ProjectDBSproc : ProjectDBBase
    {
        #region Overrides of ProjectDBBase

        public override int CreateUpdateProject(Project manufactory)
        {
            int? responecode = 0;
            try
            {
                int? _id = manufactory.Id;
              
                string _name = manufactory.Name;
                string _des = manufactory.Description;
                string _Infomation = manufactory.Infomation;
                string _CreatedBy = manufactory.CreatedBy;
                string _Image = manufactory.Image;
                
                
                int? _status = manufactory.Status;
                int? _order = manufactory.Order;
               // int? _isBlank = manufactory.IsBlank;

                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_Project_InsertUpdate(_id, _name, _CreatedBy, _des, _Image, _Infomation, _order, _status ,ref responecode);
                    return responecode.GetValueOrDefault();
                }
                   
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "ProjectDBSproc", "CreateUpdateProject");
                return -1;
            }
        }
       
        public override Project GetProject(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetProjectsDyn(select, where, order).FirstOrDefault();
        }
        public override IEnumerable<Project> GetTopLastest(int top,int status)
        {
            var select = " *";
            if (top > 1)
                select = "TOP(" + top + ") *";
            var where = "";
            if (status >-1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "Status =" + status;
            }
            var orderBy = "[Order] DESC, Id DESC";

            return GetProjectsDyn(select, where, orderBy);
        }
        public override IEnumerable<Project> GetProjectsByFilter(string keyword, int status, int pageIndex, int pageSize, ref int totalRecords)
        {
            var select = "*";



            var where = string.Empty;
            var orderBy = "[Order] DESC, Id DESC";

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = Utils.FormatKeywordSearch(keyword);
                where += "( Name LIKE N'%" + keyword + "%' ";
                where += "OR CreatedBy LIKE N'%" + keyword + "%' )";

            }

        
            if (status >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Status =" + status;
            }

            return GetAllProjectsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }
        public override IEnumerable<Project> GetAllProjectsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var results = datacontext.sp_Project_SelectPagedDynamic(select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecord);

                    return results;
                }

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }

        public override IEnumerable<Project> GetProjectsDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_Project_SelectDynamic(select, where, orderBy).ToArray();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "ProjectDBSproc", "GetProjectsDyn");
                return null;
            }
        }
        public override int UpdateOrder(int Id,bool upOrder)
        {
            try
            {
               
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_Project_UpdateSortOrder(Id, upOrder);
                    return 1;
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
       
       
        public override int UpdateStatus(int Id)
        {
            try
            {
                int? responeCode = -1;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                     datacontext.SP_ProjectUpdateStatus(Id, ref responeCode);
                    return responeCode.GetValueOrDefault();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
      

        public override int DeleteProject(int Id)
        {
            try
            {
                int? responeCode = -1;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_Project_Delete(Id, ref responeCode);
                    return responeCode.GetValueOrDefault();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }

   

        #endregion
    }
}
