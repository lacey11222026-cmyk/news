using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class CarSizeDBBase: ShopOnlineDBBase
    {
        public static CarSizeDBBase Create ()
        {
            return ( CarSizeDBBase ) Activator.CreateInstance ( typeof ( CarSizeDBSproc ) );
        }



        #region READ STATEMENTs



        public abstract CarSize Get(int id);
        public abstract IEnumerable<CarSize> GetCarSizesDyn(string select, string where, string orderBy);
        public abstract IEnumerable<CarSize> GetTopCarSizes(int groupId,int size,int status);
        #endregion

        #region DELETE STATEMENTs

       
        public abstract int UpdateCarSizeDyn(string update, string where);
        #endregion
    }
}
