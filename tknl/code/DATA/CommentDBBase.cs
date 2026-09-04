using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    public abstract class CommentDBBase: ShopOnlineDBBase
    {
        public static CommentDBBase Create ()
        {
            return ( CommentDBBase ) Activator.CreateInstance ( typeof ( CommentDBSproc ) );
        }

        #region CREATE UPDATE STATEMENTs

        public abstract int CreateUpdateComment ( Comment Comment );

        #endregion

        #region READ STATEMENTs

        public abstract Comment GetComment ( long CommentId );
      
        public abstract IEnumerable<Comment> GetCommentsDyn(string select, string where, string orderBy);
        public abstract IEnumerable<Comment> GetTopLastestComments(int top, int type,long itemid, int status);
        public abstract IEnumerable<Comment> GetAllCommentsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords);
        public abstract IEnumerable<Comment> GetCommentsByFilter(string title, int type, long itemid, int status, int pageIndex, int pageSize, ref int totalRecords);
        #endregion

        #region DELETE STATEMENTs

        public abstract int DeleteCommentDyn ( string where );
        public abstract int DeleteComment ( long CommentId );
        public abstract int DeleteComments ( string lstCommentIds );
        public abstract int UpdateCommentDyn(string update, string where);
        public abstract int PublishedComments(string lstCommentIds);
        public abstract int UpdateComment(long CommentId, int status);

        #endregion
    }
}
