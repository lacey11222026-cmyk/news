using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;

namespace BIZ
{
    public class GoNewsBO
    {
       

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get attribute by attribute id
        /// </summary>
        /// <param name="attributeId">The attribute id.</param>
        /// <returns></returns>
        public GoNew GetGoNews(int attributeId)
        {
            return GoNewsDBBase.Create().GetGoNews(attributeId);
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get attribute by id => add to local cache
        /// </summary>
        /// <param name="attributeId">The attribute id.</param>
        /// <returns></returns>
       

        public List<GoNew> GetAllGoNewsPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            var attributes = GoNewsDBBase.Create().GetAllGoNewssPaged(pageIndex, pageSize, ref totalRecords);
            if (attributes == null)
                return null;

            return attributes.ToList();
        }

       

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 23/08/2011 05:01 PM
        /// todo: get list of attributes have paged and in json format
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
      

        public List<GoNew> FilterGoNews(int categoryId,string lstcate,  int pageIndex, int pageSize, ref int totalRecords,string fromdate="",string todate="")
        {
            var attributes = GoNewsDBBase.Create().GetAllGoNewssByFilter(categoryId, lstcate,pageIndex, pageSize, ref totalRecords, fromdate, todate);
            
            return attributes.ToList();
        }

     

     

     

        #endregion

       

        #region DELETE

        public int DeleteGoNewss(string listIds)
        {
            var returnVal = GoNewsDBBase.Create().DeleteGoNewss(listIds);
          
            return returnVal;
        }

        public int DeleteGoNews(int id)
        {
            var returnVal = GoNewsDBBase.Create().DeleteGoNews(id);
          
            return returnVal;
        }

        #endregion

     

    }
}
