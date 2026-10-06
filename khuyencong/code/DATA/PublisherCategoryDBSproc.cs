using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class PublisherCategoryDBSproc: PublisherCategoryDBBase
    {
        #region Overrides of PublisherCategoryDBBase

        public override int CreateUpdatePublisherCategory ( PublisherCategory PublisherCategory )
        {
            try
            {

                string _UserName = PublisherCategory.UserName;
                string _CategoryPath = PublisherCategory.CategoryPath;
               

                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_PublisherCategory_InsertUpdate(0, _UserName, _CategoryPath);

            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }

        public override PublisherCategory GetPublisherCategory ( int PublisherCategoryId )
        {
            var select = "*";
            var where = "Id = " + PublisherCategoryId;
            var order = string.Empty;

            return GetPublisherCategorysDyn ( select, where, order ).FirstOrDefault ();
        }

        

        public override IEnumerable<PublisherCategory> GetPublisherCategorysDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    return datacontext.sp_PublisherCategory_SelectDynamic ( select, where, orderBy ).ToArray ();
                }
            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }

        public override PublisherCategory GetByUserName(string userName)
        {
            var select = "*";
            var where = string.Empty;
            if (!string.IsNullOrEmpty(userName))
                where += " UserName =" + "'" + userName + "'";
            var order = "Id DESC";

            return GetPublisherCategorysDyn(select, where, order).FirstOrDefault();

        }

       

       

        #endregion
    }
}
