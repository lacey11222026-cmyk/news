using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class NotifiDBSproc: NotifiDBBase
    {
        #region Overrides of NotifiDBBase

        public override int CreateUpdateNotifi ( Notifi manufactory )
        {
            try
            {
                long _id = manufactory.Id;
                string _title = manufactory.Title;
                string _createUser = manufactory.CreateUser;
                string _link = manufactory.Link;
                string _role = manufactory.Role;
                int? _expireDate = manufactory.ExpireDate;
                

                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_NotifiInsert (_createUser, _expireDate, _title, _link, _role);

            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }
        

        public override IEnumerable<Notifi> GetNotifisDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    return datacontext.sp_Notifi_SelectDynamic ( select, where, orderBy ).ToArray ();
                }
            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }

        public override IEnumerable<Notifi> GetNotifi ( string CreateUser, int ExpireDate)
        {
            var select = "*";
            var where = string.Empty;
            if ( !string.IsNullOrEmpty (CreateUser) )
                where += " Role Like '%," + CreateUser + ",%' ";
           
            //if (!string.IsNullOrEmpty(CreateUser))
            //{
            //    if (!string.IsNullOrEmpty(where))
            //        where += " AND ";

            //    where += " CreateUser <>" + "'" + CreateUser + "'";
            //}
            if (ExpireDate > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "ExpireDate >=" + ExpireDate;
            }
            var order = "Id DESC";

            return GetNotifisDyn ( select, where, order );

        }

       
        #endregion
    }
}
