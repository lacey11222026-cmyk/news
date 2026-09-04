using System;
using System.Collections.Generic;
using System.Linq;
using UTILS;

namespace DATA
{
    public class MarkLogDBSproc : MarkLogDBBase
    {
        public override int CreateUpdateMarkLog(MarkLog MarkLog)
        {
            try
            {
                long? _id = MarkLog.Id;
                long? _contentId = MarkLog.ContentId;
                double _mark = MarkLog.Mark;
                string _reason = MarkLog.Reason;
                string _userName = MarkLog.UserName;
                


                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_MarkLog_InsertUpdate(_id, _userName, _contentId, _mark, _reason);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CreateUpdateMarkLog");
                return -1;
            }
        }

        public override List<MarkLog> GetMarkLog(long ContentId)
        {
            var select = "*";
            var where = "ContentId = " + ContentId;
            var orderBy = "Id DESC";

            var results = GetMarkLogsDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results.ToList();
        }

        public override IEnumerable<MarkLog> GetMarkLogsDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_MarkLog_SelectDynamic(select, where, orderBy).ToArray();

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "GetAllMarkLogsPagedDyn select=" + select + "| where" + where);
                return null;
            }
        }

       

       

        


       

    }
}
