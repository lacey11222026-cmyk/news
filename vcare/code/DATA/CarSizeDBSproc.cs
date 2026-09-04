using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class CarSizeDBSproc: CarSizeDBBase
    {
        public override CarSize Get(int id)
        {
            var select = "*";
            var where = "";
            where += "Id=" + id;
            var orderBy = "[Order] ASC";

            var results = GetCarSizesDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }
        public override IEnumerable<CarSize> GetTopCarSizes(int cateId,int size,int status)
        {
            var select = "*";
            var where = "";
            if (status > -1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "Status=" + status;
            }
            if (cateId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "CategoryId=" + cateId;
            }
            if (size > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "Size=" + size;
            }
            var orderBy = "[Order] ASC";

            var results = GetCarSizesDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results;
        }
        public override IEnumerable<CarSize> GetCarSizesDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_CarSize_SelectDynamic ( select, where, orderBy ).ToArray ();

            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "CarSizeDBSproc", "GetCarSizesDyn: select" + select);
                return null;
            }
        }

      

        
        
        public override int UpdateCarSizeDyn(string update, string where)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_CarSize_UpdateDynamic(update,where);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "UpdateCarSizeDyn " + update + where);
                return -1;
            }
        }

       

    }
}
