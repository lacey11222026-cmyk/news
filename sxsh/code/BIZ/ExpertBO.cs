using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using Expert = DATA.Expert;
namespace BIZ
{
    public class ExpertBO
    {
       

        #region CREATE
        public int CreateUpdateExpert(Expert Expert)
        {
            
            int returnVal = ExpertDBBase.Create().CreateUpdateExpert(Expert);
          
            return returnVal;
        }
        public int UpdateStatus(int ExpertId)
        {
            try
            {
                return ExpertDBBase.Create().UpdateStatus(ExpertId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ExpertBO", "UpdateStatus");
                return -1;
            }
        }
       
        public int UpdateOrder(int ExpertId, bool upOrder)
        {
            try
            {
                return ExpertDBBase.Create().UpdateOrder(ExpertId, upOrder);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ExpertBO", "UpdateOrder");
                return -1;
            }
        }
        #endregion

        #region READ


        public Expert GetExpert(int ExpertId)
        {
            try
            {
                return ExpertDBBase.Create().GetExpert(ExpertId);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ExpertBO", "GetExpert");
                return null;
            }
        }


        public List<Expert> GetTopExpert(int top,int type)
        {
            var data = ExpertDBBase.Create().GetTopLastest(top,type);
            if (data == null)
                return null;

            return data.ToList();
        }

        public List<Expert> GetExpertsPaged(string keyword, int pageIndex, int pageSize, ref int totalRecords, int? published,int type, string lang)
        {
            var data = ExpertDBBase.Create().GetAllPaged(keyword,pageIndex, pageSize, ref  totalRecords, published, type,lang);
            if (data == null)
                return null;

            return data.ToList();
        }
       
       

        #endregion



        #region DELETE

       

        public int DeleteExpert(int id)
        {
            var returnVal = ExpertDBBase.Create().DeleteExpert(id);
           
            return returnVal;
        }

        #endregion

      
    }
}
