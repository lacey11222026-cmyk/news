using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class QADBSproc : QADBBase
    {
        #region Overrides of QADBBase

        public override int CreateUpdateQA(QA manufactory)
        {
            int? responecode = 0;
            try
            {
                int _id = manufactory.Id;
              
                string _name = manufactory.Name;
                string _question = manufactory.Question;
                string _answer = manufactory.Answer;
                int? _status = manufactory.Status;
                int? _order = manufactory.Order;

               
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_QA_InsertUpdate(_id, _name, _question, _answer, _order, _status, ref responecode);
                    return responecode.GetValueOrDefault();
                }
                   
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "QADBSproc", "CreateUpdateQA");
                return -1;
            }
        }
       
        public override QA GetQA(int Id)
        {
            var select = "*";
            var where = "Id = " + Id;
            var order = string.Empty;

            return GetQAsDyn(select, where, order).FirstOrDefault();
        }
        public override IEnumerable<QA> GetTopLastest(int top)
        {
            var select = " *";
            if (top > 1)
                select = "TOP(" + top + ") *";
            var where = "Status = 1";
            var orderBy = "[Order] DESC, Id DESC";

            return GetQAsDyn(select, where, orderBy);
        }
        public override IEnumerable<QA> GetAllPaged(int pageIndex, int pageSize, ref int totalRecords, int? published)
        {
            var select = string.Empty;
            var where = string.Empty;
           
           
            if (published >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "Status = " + published;
            }
          
            var orderBy = "[Order] DESC, ID DESC";

            return GetAllQAsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public IEnumerable<QA> GetAllQAsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
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
                    var list = datacontext.sp_QA_SelectPagedDynamic(_select, _where, _orderBy, _pageIndex, _pageSize, ref _totalRecords).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecords);
                    return list;
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "QADBSproc", "GetAllQAsPagedDyn");
                return null;
            }
        }

        public override IEnumerable<QA> GetQAsDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    return datacontext.sp_QA_SelectDynamic(select, where, orderBy).ToArray();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "QADBSproc", "GetQAsDyn");
                return null;
            }
        }
        public override int UpdateOrder(int Id,bool upOrder)
        {
            try
            {
               
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_QA_UpdateSortOrder(Id, upOrder);
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
                     datacontext.SP_QA_UpdateStatus(Id, ref responeCode);
                    return responeCode.GetValueOrDefault();
                }
            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }
      

        public override int DeleteQA(int Id)
        {
            try
            {
                int? responeCode = -1;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    datacontext.SP_QA_Delete(Id, ref responeCode);
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
