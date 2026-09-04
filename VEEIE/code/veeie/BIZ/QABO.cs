using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using QA = DATA.QA;
namespace BIZ
{
    public class QABO
    {
       

        #region CREATE
        public int CreateUpdateQA(QA QA)
        {
            
            int returnVal = QADBBase.Create().CreateUpdateQA(QA);
          
            return returnVal;
        }
        public int UpdateStatus(int QAId)
        {
            try
            {
                return QADBBase.Create().UpdateStatus(QAId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "QABO", "UpdateStatus");
                return -1;
            }
        }
       
        public int UpdateOrder(int QAId, bool upOrder)
        {
            try
            {
                return QADBBase.Create().UpdateOrder(QAId, upOrder);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "QABO", "UpdateOrder");
                return -1;
            }
        }
        #endregion

        #region READ


        public QA GetQA(int QAId)
        {
            try
            {
                return QADBBase.Create().GetQA(QAId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "QABO", "GetQA");
                return null;
            }
        }


        public List<QA> GetTopQA(int top,int categoryId)
        {
            var data = QADBBase.Create().GetTopLastest(top, categoryId);
            if (data == null)
                return null;

            return data.ToList();
        }

        public List<QA> GetQAsPaged(int pageIndex, int pageSize, ref int totalRecords, int? published,int categoryId )
        {
            var data = QADBBase.Create().GetAllPaged(pageIndex, pageSize, ref  totalRecords, published, categoryId);
            if (data == null)
                return null;

            return data.ToList();
        }
       
       

        #endregion



        #region DELETE

       

        public int DeleteQA(int id)
        {
            var returnVal = QADBBase.Create().DeleteQA(id);
           
            return returnVal;
        }

        #endregion

      
    }
}
