using DATA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace BIZ
{
    public class SystemConfigBO
    {
      

        #region CREATE


        public int CreateUpdateSystemConfig(SystemConfig SystemConfig)
        {

            int returnVal = SystemConfigDBBase.Create().CreateUpdateSystemConfig(SystemConfig);
            
            return returnVal;
        }

        #endregion

        #region READ
         public int SetByKey(string Key,string Value)
         {
             var data = GetByKey(Key);
             if(data!=null)
             {
                 data.ConfigValue = Value;
                 return CreateUpdateSystemConfig(data);
             }
             else
             {
                 var newData = new SystemConfig();
                 newData.ConfigValue = Value;
                 newData.ConfigKey = Key;
                 return CreateUpdateSystemConfig(newData);
             }

         }

        public SystemConfig GetByKey(string  key)
        {
            return SystemConfigDBBase.Create().GetSystemConfig(key);
        }
        public string GetValueByKey(string key)
        {
            var data= SystemConfigDBBase.Create().GetSystemConfig(key);
            if (data != null)
                return data.ConfigValue;
            return "";
        }

        #region UPDATE

       

        #endregion

       
        #endregion

      

    }
}
