using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using Newtonsoft.Json;
using UTILS;
using TechProcess = DATA.TechProcess;
namespace BIZ
{
    public class TechProcessBO
    {
        

        #region CREATE


        public int CreateUpdateTechProcess(TechProcess TechProcess)
        {
            
            int returnVal = TechProcessDBBase.Create().CreateUpdateTechProcess(TechProcess);
            
           
            return returnVal;
        }

        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get TechProcess by TechProcess id
        /// </summary>
        /// <param name="TechProcessId">The TechProcess id.</param>
        /// <returns></returns>
        //public TechProcess GetTechProcess(int TechProcessId)
        //{
        //    return TechProcessDBBase.Create().GetTechProcess(TechProcessId);
        //}

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get TechProcess by id => add to local cache
        /// </summary>
        /// <param name="TechProcessId">The TechProcess id.</param>
        /// <returns></returns>
        public TechProcess GetTechProcess(int TechProcessId)
        {
            try
            {
               

                var item = TechProcessDBBase.Create().GetTechProcess(TechProcessId);
                
               

                return item;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }
      
        private IEnumerable<TechProcess> GetTopLastestTechProcesss(int top)
        {
            var result = TechProcessDBBase.Create().GetTopLastestTechProcesss(top);
            if (result == null)
                return null;
            return result;
        }
        public List<TechProcess> GetTopLastestTechProcesssFull(int top)
        {
         
          
            var lstItemBase = GetTopLastestTechProcesss(top);
            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

          

            return lstItemBase.ToList();
        }
        public int UpdateOrder(int ProjectId, bool upOrder)
        {
            try
            {
                return TechProcessDBBase.Create().UpdateOrder(ProjectId, upOrder);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "TechProcessBO", "UpdateOrder");
                return -1;
            }
        }
        public List<TechProcess> GetTechProcesssFuLLPaged(string title, int pageIndex, int pageSize, ref int totalRecords)
        {
           
            var TechProcesss = GetTechProcesssPaged(title, pageIndex, pageSize,ref totalRecords);

            
            return TechProcesss.ToList();
        }

        private IEnumerable<TechProcess>  GetTechProcesssPaged(string title, int pageIndex, int pageSize, ref int totalRecords)
        {
            return TechProcessDBBase.Create().GetTechProcesssByFilter(title,pageIndex, pageSize,ref totalRecords);
        }


        #endregion

        #region UPDATE
       
        

        #endregion

        #region DELETE

        public int DeleteTechProcesss(string listIds)
        {
            var returnVal = TechProcessDBBase.Create().DeleteTechProcesss(listIds);
           
            return returnVal;
        }

        public int DeleteTechProcess(int id)
        {
            var returnVal = TechProcessDBBase.Create().DeleteTechProcess(id);
            
            return returnVal;
        }

        #endregion

      

    }
}
