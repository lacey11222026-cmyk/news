using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class AlbumDBBase: ShopOnlineDBBase
    {
        public static AlbumDBBase Create ()
        {
            return ( AlbumDBBase ) Activator.CreateInstance ( typeof ( AlbumDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateAlbum ( Album Album );

        #endregion

        #region READ STATEMENTs

        public abstract Album GetAlbum ( int AlbumId );
      
        public abstract IEnumerable<Album> GetAllAlbumsPagedDyn ( string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords );
        public abstract IEnumerable<Album> GetTopAlbumByIds(string ids, int top);
        public abstract IEnumerable<Album> GetAlbumsByFilter(string keyword, int categoryId, int status, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Album> GetAlbumsDyn(string select, string where, string orderBy);
        public abstract IEnumerable<Album> GetTopLastestAlbums(int top, int categoryId);
        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteAlbumDyn ( string where );
        public abstract int DeleteAlbum ( int AlbumId );
        public abstract int DeleteAlbums ( string lstAlbumIds );

        #endregion
    }
}
