using System;
using System.Collections.Generic;
using UTILS;
using System.Data;
using System.Data.SqlClient;
using DATA.SMS;
using System.Globalization;
using System.Linq;
namespace DATA.DocumentDB
{
    public class LicensingDAL
    {
       
        public static List<Licensing> SelectDynamic()
        {
            try
            {
             

                var list = new DBHelper(Configuration.ViaConnectionString).GetListSP<Licensing>("sp_GetTin");
               
                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                
                return new List<Licensing>();
            }
        }
       
        

    }
}
