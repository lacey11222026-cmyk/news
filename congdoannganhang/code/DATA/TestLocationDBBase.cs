using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class TestLocationDBBase: ShopOnlineDBBase
    {
        public static TestLocationDBBase Create ()
        {
            return ( TestLocationDBBase ) Activator.CreateInstance ( typeof ( TestLocationDBSproc ) );
        }

       

        #region READ STATEMENTs

       
        public abstract IEnumerable<TestLocation> GetTestLocationsDyn ( string select, string where, string orderBy );
        public abstract IEnumerable<TestLocation> GetTestLocation();
        

        #endregion




    }
}
