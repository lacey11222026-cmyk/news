using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace BIZ
{
    public class MarkLogBO
    {
       

        #region CREATE


        public int CreateUpdateMarkLog(MarkLog MarkLog)
        {

            int returnVal = MarkLogDBBase.Create().CreateUpdateMarkLog(MarkLog);
            
            return returnVal;
        }

        #endregion

        #region READ


        public List<MarkLog> GetMarkLogsByContentId(long ContentId)
        {
           

           return  MarkLogDBBase.Create().GetMarkLog(ContentId);

            
        }


       

       

        #endregion

      

    }
}
