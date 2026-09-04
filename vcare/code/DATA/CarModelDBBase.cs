using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class CarModelDBBase: ShopOnlineDBBase
    {
        public static CarModelDBBase Create ()
        {
            return ( CarModelDBBase ) Activator.CreateInstance ( typeof ( CarModelDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateCarModel ( CarModel CarModel );

        #endregion

        #region READ STATEMENTs

        public abstract CarModel GetCarModel ( int CarModelId );
        public abstract CarModel GetByUrl(string Url);
        

        public abstract IEnumerable<CarModel> GetCarModelsDyn(string select, string where, string orderBy);
        public abstract IEnumerable<CarModel> GetTopCarModels(int groupId,int status);
        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteCarModelDyn ( string where );
        public abstract int DeleteCarModel ( int CarModelId );
        public abstract int UpdateCarModelDyn(string update, string where);
        #endregion
    }
}
