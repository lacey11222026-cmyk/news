using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class AttributeDBBase: ShopOnlineDBBase
    {
        public static AttributeDBBase Create ()
        {
            return ( AttributeDBBase ) Activator.CreateInstance ( typeof ( AttributeDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateAttribute ( Attribute attribute );

        #endregion

        #region READ STATEMENTs

        public abstract Attribute GetAttribute ( int attributeId );
        public abstract IEnumerable<Attribute> GetAttributesDyn ( string select, string where, string orderBy );
        public abstract IEnumerable<Attribute> GetAllAttributesPaged ( int pageIndex, int pageSize, ref int totalRecords );
        public abstract IEnumerable<Attribute> GetAllAttributesPagedDyn ( string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords );
        public abstract IEnumerable<Attribute> GetAllAttributes ( string name, int categoryId, int groupId );
        public abstract IEnumerable<Attribute> GetAllAttributesByFilter ( int categoryId, byte? published );


        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteAttributeDyn ( string where );
        public abstract int DeleteAttribute ( int attributeId );
        public abstract int DeleteAttributes ( string lstAttributeIds );

        #endregion
    }
}
