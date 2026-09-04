using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using Newtonsoft.Json;
using UTILS;
using NhaMay = DATA.NhaMay;
namespace BIZ
{
    public class NhaMayBO
    {


        #region CREATE


        public int CreateUpdateNhaMay(NhaMay NhaMay)
        {

            int returnVal = NhaMayDBBase.Create().CreateUpdateNhaMay(NhaMay);


            return returnVal;
        }

        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get NhaMay by NhaMay id
        /// </summary>
        /// <param name="NhaMayId">The NhaMay id.</param>
        /// <returns></returns>
        //public NhaMay GetNhaMay(int NhaMayId)
        //{
        //    return NhaMayDBBase.Create().GetNhaMay(NhaMayId);
        //}

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get NhaMay by id => add to local cache
        /// </summary>
        /// <param name="NhaMayId">The NhaMay id.</param>
        /// <returns></returns>
        public NhaMay GetNhaMay(int NhaMayId)
        {
            try
            {
               

                var item = NhaMayDBBase.Create().GetNhaMay(NhaMayId);
                
               

                return item;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }
      
        private IEnumerable<NhaMay> GetTopLastestNhaMays(int top)
        {
            var result = NhaMayDBBase.Create().GetTopLastestNhaMays(top);
            if (result == null)
                return null;
            return result;
        }
        public List<NhaMay> GetTopLastestNhaMaysFull(int top)
        {
         
          
            var lstItemBase = GetTopLastestNhaMays(top);
            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

          

            return lstItemBase.ToList();
        }
        //public int UpdateOrder(int ProjectId, bool upOrder)
        //{
        //    try
        //    {
        //        return NhaMayDBBase.Create().UpdateOrder(ProjectId, upOrder);
        //    }
        //    catch (Exception e)
        //    {
        //        ExHandler.Handle(e, "NhaMayBO", "UpdateOrder");
        //        return -1;
        //    }
        //}
        public List<NhaMay> GetNhaMaysFuLLPaged(string title,int pageIndex, int pageSize, ref int totalRecords, int loai = -1, int hinhthuc = -1, int status = -1, string fromdate = "", string todate = "")
        {
           
            var NhaMays = GetNhaMaysPaged(title,pageIndex, pageSize,ref totalRecords,loai,hinhthuc,status,fromdate,todate);

            
            return NhaMays.ToList();
        }

        private IEnumerable<NhaMay>  GetNhaMaysPaged(string title, int pageIndex, int pageSize, ref int totalRecords, int loai = -1, int hinhthuc = -1, int status = -1, string fromdate = "", string todate = "")
        {
            return NhaMayDBBase.Create().GetNhaMaysByFilter(title,pageIndex, pageSize,ref totalRecords,loai,hinhthuc,status,fromdate,todate);
        }


        #endregion

        #region UPDATE
       
        

        #endregion

        #region DELETE

        public int DeleteNhaMays(string listIds)
        {
            var returnVal = NhaMayDBBase.Create().DeleteNhaMays(listIds);
           
            return returnVal;
        }

        public int DeleteNhaMay(int id)
        {
            var returnVal = NhaMayDBBase.Create().DeleteNhaMay(id);
            
            return returnVal;
        }

        #endregion

      

    }
}
