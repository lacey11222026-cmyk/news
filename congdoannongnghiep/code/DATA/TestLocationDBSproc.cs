using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class TestLocationDBSproc: TestLocationDBBase
    {
        #region Overrides of TestLocationDBBase

        

        public override IEnumerable<TestLocation> GetTestLocationsDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    return datacontext.sp_TestLocation_SelectDynamic ( select, where, orderBy ).ToArray ();
                }
            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }
        
        public override IEnumerable<TestLocation> GetTestLocation( )
        {
            var select = "*";
            var where = "1=1 ";
    
            var order = "Id ASC";

            return GetTestLocationsDyn ( select, where, order );

        }
       
        

        #endregion
    }
}
