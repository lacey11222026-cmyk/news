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

        public abstract int CreateUpdateCity(City manufactory);

        #region READ STATEMENTs
        public abstract City GetCity(int id);
        public abstract IEnumerable<City> GetTopLastest(int top, int? published, int type);
        public abstract IEnumerable<City> GetCitysDyn ( string select, string where, string orderBy );
      

        #endregion

      



    }
}
