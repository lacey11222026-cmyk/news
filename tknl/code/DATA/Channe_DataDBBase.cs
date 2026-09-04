using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class Channel_DataDBBase: ShopOnlineDBBase
    {
        public static Channel_DataDBBase Create ()
        {
            return ( Channel_DataDBBase ) Activator.CreateInstance ( typeof ( Channel_DataDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateChannel_Data ( Channel_Data Channel_Data );

        #endregion

        #region READ STATEMENTs

        public abstract Channel_Data GetById(int Id);
        public abstract IEnumerable<Channel_Data> GetChannel_DatasDyn ( string select, string where, string orderBy );
        public abstract IEnumerable<Channel_Data> GetAllChannel_DatasPaged(int channelId, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Channel_Data> GetAllChannel_DatasPagedDyn ( string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords );
        public abstract IEnumerable<Channel_Data> GetByContentId(long ContentId);

        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteChannel_DataDyn ( string where );
        public abstract int DeleteById(int ChannelId, long ContenId);
        public abstract int DeleteByChannelId(int ChannelId);
        public abstract int DeleteById(long Id);

        #endregion
    }
}
