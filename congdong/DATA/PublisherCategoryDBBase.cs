using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class PublisherCategoryDBBase: ShopOnlineDBBase
    {
        public static PublisherCategoryDBBase Create ()
        {
            return ( PublisherCategoryDBBase ) Activator.CreateInstance ( typeof ( PublisherCategoryDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdatePublisherCategory ( PublisherCategory PublisherCategory );

        #endregion

        #region READ STATEMENTs

        public abstract PublisherCategory GetPublisherCategory ( int PublisherCategoryId );
        public abstract IEnumerable<PublisherCategory> GetPublisherCategorysDyn ( string select, string where, string orderBy );
        public abstract PublisherCategory GetByUserName(string userName);

        #endregion






    }
}
