using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class AttributeGroupDBBase: ShopOnlineDBBase
    {
        public static AttributeGroupDBBase Create ()
        {
            return ( AttributeGroupDBBase ) Activator.CreateInstance ( typeof ( AttributeGroupDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateAttributeGroup ( AttributeGroup attributeGroup );

        #endregion

        #region READ STATEMENTs

        public abstract AttributeGroup GetAttributeGroup ( int attributeGroupId );
        public abstract IEnumerable<AttributeGroup> GetAttributeGroupsDyn ( string select, string where, string orderBy );
        public abstract IEnumerable<AttributeGroup> GetAllAttributeGroups ();        

        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteAttributeGroupDyn ( string where );

        public abstract int DeleteAttributeGroup ( int attributeGroupId );

        public abstract int DeleteAttributeGroups ( string lstAttributeGroupIds );

        #endregion


    }
}
