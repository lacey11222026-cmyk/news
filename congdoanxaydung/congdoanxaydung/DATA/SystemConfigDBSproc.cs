using System;
using System.Collections.Generic;
using System.Linq;
using UTILS;

namespace DATA
{

    public class SystemConfigDBSproc : SystemConfigDBBase
    {
        #region Overrides of SystemConfigDBBase

        public override int CreateUpdateSystemConfig(SystemConfig SystemConfig)
        {
            try
            {
                int _id = SystemConfig.Id;
                string ConfigKey = SystemConfig.ConfigKey;
                string ConfigValue = SystemConfig.ConfigValue;
               

                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_SystemConfig_InsertUpdate(_id, ConfigKey, ConfigValue);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return -1;
            }
        }

        public override SystemConfig GetSystemConfig(string ConfigKey)
        {
            var select = "*";

            var where = " ConfigKey =" + "'" + ConfigKey + "'";
            var orderBy = string.Empty;

            var results = GetSystemConfigsDyn(select, where, orderBy);
            if (results == null)
                return null;

            return results.FirstOrDefault();
        }

        public override IEnumerable<SystemConfig> GetSystemConfigsDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_SystemConfig_SelectDynamic(select, where, orderBy).ToArray();

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp);
                return null;
            }
        }

       

      


      

        #endregion
    }
}
