using System;
using System.Collections.Generic;

namespace DATA
{
    public abstract class PromotionDBBase:ShopOnlineDBBase
    {
        public static PromotionDBBase Create ()
        {
            return ( PromotionDBBase ) Activator.CreateInstance ( typeof ( PromotionDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdatePromotion ( Promotion promotion );

        #endregion

        #region READ STATEMENTs

        public abstract Promotion GetPromotion ( int promotionId );
        public abstract IEnumerable<Promotion> GetPromotionsDyn ( string select, string where, string orderBy );
        public abstract IEnumerable<Promotion> GetAllPromotionsPaged ( int pageIndex, int pageSize, ref int totalRecords );
        public abstract IEnumerable<Promotion> GetAllPromotionsPagedDyn ( string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords );
        public abstract IEnumerable<Promotion> GetAllPromotions ( string promotionCode, int categoryId, int published);
        public abstract IEnumerable<Promotion> GetAllPromotions (byte published);        


        #endregion

        #region DELETE STATEMENTs

        public abstract int DeletePromotionDyn ( string where );
        public abstract int DeletePromotion ( int promotionId );
        public abstract int DeletePromotions ( string lstPromotionIds );

        #endregion
    }
}
