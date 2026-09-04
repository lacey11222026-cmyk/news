using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class ChannelDBBase: ShopOnlineDBBase
    {
        public static ChannelDBBase Create ()
        {
            return ( ChannelDBBase ) Activator.CreateInstance ( typeof ( ChannelDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateChannel ( Channel Channel );

        #endregion

        #region READ STATEMENTs

        public abstract Channel GetChannel ( int ChannelId );
        public abstract IEnumerable<Channel> GetChannelsDyn ( string select, string where, string orderBy );
        public abstract IEnumerable<Channel> GetAllChannelsPaged(string title,int pageIndex, int pageSize, ref int totalRecords, int status = -1);
        public abstract IEnumerable<Channel> GetAllChannelsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Channel> GetAllChannelsByFilter (  int? published );
        public abstract IEnumerable<Channel> GetChannelByIds(string ids, int top);
        public abstract IEnumerable<Channel> GetTopFiller(string title, int top);
        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteChannelDyn ( string where );
        public abstract int DeleteChannel ( int ChannelId );
        public abstract int DeleteChannels ( string lstChannelIds );

        #endregion
    }
}
