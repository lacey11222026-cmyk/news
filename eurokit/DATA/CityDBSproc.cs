using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class CityDBSproc: CityDBBase
    {


        public override IEnumerable<City> GetTopLastest(int top, int? published)
        {
            var select = " *";
            if (top > 1)
                select = "TOP(" + top + ") *";
            var where = "Status = 1";
            
            if (published >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "Status = " + published;
            }

            
            var orderBy = "[OrderSort] ASC, Name ASC";

            return GetCitysDyn(select, where, orderBy);
        }

        public override IEnumerable<City> GetCitysDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    return datacontext.sp_City_SelectDynamic ( select, where, orderBy ).ToArray ();
                }
            }
            catch ( Exception exp )
            {
                ExHandler.Handle(exp, "CityDBSproc", "GetCitysDyn");
                return null;
            }
        }

     
      

      

        
    }
}
