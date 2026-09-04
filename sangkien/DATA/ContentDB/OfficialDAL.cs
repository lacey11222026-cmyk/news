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
    public class OfficialDAL
    {

        public static List<Official> Get()
        {
            try
            {


                var list = new DBHelper(Configuration.ViaConnectionString).GetListSP<Official>("sp_GetDoc");

                return list;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);

                return new List<Official>();
            }
        }
    }
}
