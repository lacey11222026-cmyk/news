using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class NotiReadDBSproc: NotiReadDBBase
    {
        #region Overrides of NotiReadDBBase

        public override int Read ( NotiRead manufactory )
        {
            try
            {
                long _id = manufactory.Id;
                int? _expireDate = manufactory.ExpireDate;
                long? _notiId = manufactory.NotiId;
                string _userName = manufactory.UserName;
               
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_NotiRead_Insert (_userName,_expireDate,_notiId);

            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }

        public override int ReadMulti(int expireDate,string userName,string notiIds)
        {
            try
            {
               

                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_NotiRead_InsertMulti(userName, expireDate, notiIds);

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }




        public override IEnumerable<NotiRead> GetNotiReadsDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    return datacontext.sp_NotiRead_SelectDynamic ( select, where, orderBy ).ToArray ();
                }
            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }

        public override IEnumerable<NotiRead> GetNotiRead (string CreateUser, int ExpireDate)
        {
            var select = "*";
            var where = string.Empty;
            if (!string.IsNullOrEmpty(CreateUser))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " UserName =" + "'" + CreateUser + "'";
            }
            if (ExpireDate > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "ExpireDate >=" + ExpireDate;
            }
            var order = "Id DESC";

            return GetNotiReadsDyn ( select, where, order );

        }

      

        #endregion
    }
}
