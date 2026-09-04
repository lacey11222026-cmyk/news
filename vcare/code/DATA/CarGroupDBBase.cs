using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class CarGroupDBBase: ShopOnlineDBBase
    {
        public static CarGroupDBBase Create ()
        {
            return ( CarGroupDBBase ) Activator.CreateInstance ( typeof ( CarGroupDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateCarGroup ( CarGroup CarGroup );

        #endregion

        #region READ STATEMENTs

        public abstract CarGroup GetCarGroup ( int CarGroupId );
      
        
        
        public abstract IEnumerable<CarGroup> GetCarGroupsDyn(string select, string where, string orderBy);
        public abstract IEnumerable<CarGroup> GetTopCarGroups(int status);
        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteCarGroupDyn ( string where );
        public abstract int DeleteCarGroup ( int CarGroupId );
       
        #endregion
    }
}
