using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class ExpertDBSproc : ExpertDBBase
    {
        #region Overrides of ExpertDBBase

        public override int CreateUpdateExpert(Expert manufactory)
        {
            int? responecode = 0;
            try
            {
                int? _id = manufactory.Id;
              
                string _name = manufactory.Name;
                string _address = manufactory.Address;
                string _description = manufactory.Description;
                string _phone = manufactory.Phone;
                string _role = manufactory.Role;
                string _image = manufactory.Image;
                string _learn = manufactory.Learning;
                int? _status = manufactory.Status;
                int? _order = manufactory.Order;
                
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    //datacontext.SP_Expert_InsertUpdate(_id, _name, _image, _address, _description,_phone, _learn, _role, _order, _status, ref responecode);
                    return responecode.GetValueOrDefault();
                   
                }
                   
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "ExpertDBSproc", "CreateUpdateExpert");
                return -1;
            }
        }
       
        public override Expert GetExpert(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetExpertsDyn(select, where, order).FirstOrDefault();
        }
        public override IEnumerable<Expert> GetTopLastest(int top,int type)
        {
            var select = " [Id],[Name] ,[Address] ,[Role],[Company],[Mobile],[Phone],[Description] ,[Status],[Image],[CityId],[Email]";
            if (top > 1)
                select = "TOP(" + top + ") *";
            var where = "Status = 1";
            if(type>0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CityId=" + type.ToString();
            }
            var orderBy = "[Order] DESC, Id DESC";

            return GetExpertsDyn(select, where, orderBy);
        }
        public override IEnumerable<Expert> GetAllPaged(string keyword,int pageIndex, int pageSize, ref int totalRecords, int? published, int type,string lang)
        {
            var select = "[Id],[Name] ,[Address] ,[Role],[Company],[Mobile],[Phone],[Description] ,[Status],[Image],[CityId],[Email]";
            var where = string.Empty;

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = Utils.FormatKeywordSearch(keyword);
                where += "( Name LIKE N'%" + keyword + "%' ";
                where += "OR Address LIKE N'%" + keyword + "%' ";
                where += "OR Mobile LIKE N'%" + keyword + "%' ";
                where += "OR Description LIKE N'%" + keyword + "%' )";

            }
            if (!string.IsNullOrEmpty(lang))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "[Language] = " + "'" + lang + "'";
            }
            if (published > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "Status = " + published;
            }
            if (type > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CityId=" + type.ToString();
            }
            var orderBy = "[Order] DESC, ID DESC";

            return GetAllExpertsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public IEnumerable<Expert> GetAllExpertsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                string _select = select;
                string _where = where;
                string _orderBy = orderBy;
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecords = 0;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var list = datacontext.sp_Expert_SelectPagedDynamic(_select, _where, _orderBy, _pageIndex, _pageSize, ref _totalRecords).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecords);
                    return list;
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "ExpertDBSproc", "GetAllExpertsPagedDyn");
                return null;
            }
        }

        public override IEnumerable<Expert> GetExpertsDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_Expert_SelectDynamic(select, where, orderBy).ToArray();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "ExpertDBSproc", "GetExpertsDyn");
                return null;
            }
        }
        public override int UpdateOrder(int Id,bool upOrder)
        {
            try
            {
               
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_Expert_UpdateSortOrder(Id, upOrder);
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
                     datacontext.SP_Expert_UpdateStatus(Id, ref responeCode);
                    return responeCode.GetValueOrDefault();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
      

        public override int DeleteExpert(int Id)
        {
            try
            {
                int? responeCode = -1;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_Expert_Delete(Id, ref responeCode);
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
