using System;
using System.Collections.Generic;

namespace DATA
{
    public abstract class SystemConfigDBBase:ShopOnlineDBBase
    {
        public static SystemConfigDBBase Create ()
        {
            return ( SystemConfigDBBase ) Activator.CreateInstance ( typeof ( SystemConfigDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateSystemConfig ( SystemConfig SystemConfig );

        #endregion

        #region READ STATEMENTs

        public abstract SystemConfig GetSystemConfig(string ConfigKey);
        public abstract IEnumerable<SystemConfig> GetSystemConfigsDyn ( string select, string where, string orderBy );
        


        #endregion

       
    }
}
