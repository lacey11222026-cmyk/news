using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class CityDBBase: ShopOnlineDBBase
    {
        public static CityDBBase Create ()
        {
            return ( CityDBBase ) Activator.CreateInstance ( typeof ( CityDBSproc ) );
        }



        #region READ STATEMENTs

        public abstract IEnumerable<City> GetTopLastest(int top, int? published);
        public abstract IEnumerable<City> GetCitysDyn ( string select, string where, string orderBy );
      

        #endregion

      



    }
}
