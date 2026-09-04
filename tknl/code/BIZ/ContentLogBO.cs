using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace BIZ
{
    public class ContentLogBO
    {
        

        


        public int CreateUpdateContentLog(ContentLog ContentLog)
        {

            int returnVal = ContentLogDBBase.Create().CreateUpdateContentLog(ContentLog);
            
            return returnVal;
        }

      

        public ContentLog GetById(long id)
        {
            try
            {
                

                var obj = ContentLogDBBase.Create().GetById(id);


                return obj;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "GetById");
                return null;
            }
        }
        public List<ContentLog> GetContentLogsByContentId(long contentId,int type)
        {
           

            return  ContentLogDBBase.Create().GetContentLog(contentId, type);

           

        }

        public List<ContentLog> GetByFilter(string UserName, int itemtType, long itemid, string itemName, int pageIndex, int pageSize, ref int totalRecords, string fromdate = "", string todate = "")
        {
            return ContentLogDBBase.Create().GetByFilter(UserName, itemtType, itemid, itemName, pageIndex, pageSize, ref totalRecords, fromdate, todate).ToList();
        }




    }
}
