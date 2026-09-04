using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class BannerDBBase: ShopOnlineDBBase
    {
        public static BannerDBBase Create ()
        {
            return ( BannerDBBase ) Activator.CreateInstance ( typeof ( BannerDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateBanner ( Banner Banner );

        #endregion

        #region READ STATEMENTs

        public abstract Banner GetBanner ( int BannerId );
      
        public abstract IEnumerable<Banner> GetBannersDyn(string select, string where, string orderBy);
        public abstract IEnumerable<Banner> GetTopLastestBanners(int top, int Region, int status,int site,int categoryId);
        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteBannerDyn ( string where );
        public abstract int DeleteBanner ( int BannerId );
        public abstract int DeleteBanners ( string lstBannerIds );

        #endregion
    }
}
