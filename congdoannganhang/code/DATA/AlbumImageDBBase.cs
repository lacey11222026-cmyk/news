using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class AlbumImageDBBase: ShopOnlineDBBase
    {
        public static AlbumImageDBBase Create ()
        {
            return ( AlbumImageDBBase ) Activator.CreateInstance ( typeof ( AlbumImageDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateAlbumImage(AlbumImage Album);

        #endregion

        #region READ STATEMENTs

        public abstract AlbumImage GetAlbum(int AlbumId);

        public abstract IEnumerable<AlbumImage> GetAllAlbumsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<AlbumImage> GetTopAlbumByIds(string ids, int top);
        public abstract IEnumerable<AlbumImage> GetAlbumsByFilter(string keyword, int categoryId, int status, int type, int pageIndex, int pageSize, ref int totalRecords, string fromdate, string todate,string orderBy);
        public abstract IEnumerable<AlbumImage> GetAlbumsDyn(string select, string where, string orderBy);
        public abstract IEnumerable<AlbumImage> GetTopLastestAlbums(int top, int categoryId);
        #endregion

        #region DELETE STATEMENTs

        public abstract int UpdateAlbumDyn(string where, string updatestr);
        public abstract int Vote(long albumId, int point);
        public abstract int Vote2(long albumId, int point);
        public abstract int DeleteAlbumDyn ( string where );
        public abstract int DeleteAlbum ( int albumId );
        public abstract int DeleteAlbums ( string lstAlbumIds );

        #endregion
    }
}
