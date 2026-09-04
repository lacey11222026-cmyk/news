using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class CommentDBSproc : CommentDBBase
    {
        public override int CreateUpdateComment(Comment Comment)
        {
            try
            {
                long? _id = Comment.Id;

                int? _type = Comment.Type;
                long? _itemid = Comment.ItemId;
                string _itemname = Comment.ItemName;
                string _message = Comment.Message;
                string _username = Comment.UserName;
                string _email = Comment.Email;
                DateTime? _createdTime = Comment.CreatedTime;
                byte? _status = Comment.Published;




                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Comment_InsertUpdate(_id, _type, _itemid, _username, _email, _itemname, _message, _status, _createdTime);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CommentDBSproc", "CreateUpdateComment");
                return -1;
            }
        }
        public override IEnumerable<Comment> GetTopLastestComments(int top, int type, long itemId, int status)
        {
            var select = " Id,ItemName,UserName,Message,Published,CreatedTime";
            if (top > 1)
                select = "TOP(" + top + ") Id,ItemName,UserName,Message,Published,CreatedTime";
            var where = String.Empty;
            if (status > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Published=" + status.ToString();
            }
            if (type > -1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Type=" + type.ToString();
            }
            if (itemId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " ItemId=" + itemId.ToString();
            }
            var orderBy = "CreatedTime DESC";

            return GetCommentsDyn(select, where, orderBy);
        }
        public override Comment GetComment(long CommentId)
        {
            var select = "*";
            var where = "Id = " + CommentId;
            var orderBy = string.Empty;

            var results = GetCommentsDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }

        public override IEnumerable<Comment> GetCommentsDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Comment_SelectDynamic(select, where, orderBy).ToArray();

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CommentDBSproc", "GetCommentsDyn: select" + select);
                return null;
            }
        }


        public override IEnumerable<Comment> GetAllCommentsPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var results = datacontext.sp_Comment_SelectPagedDynamic(select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecord);

                    return results;
                }

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "CommentDBSproc", "GetAllCommentsPagedDyn: select" + select);
                return null;
            }
        }

        public override IEnumerable<Comment> GetCommentsByFilter(string title, int type, long itemid, int status, int pageIndex, int pageSize, ref int totalRecords)
        {
            var select = "*";

			title = Utils.FormatKeywordSearch(title);

            var where = string.Empty;
            var orderBy = "CreatedTime DESC";

            if (!string.IsNullOrEmpty(title))
                where += "ItemName LIKE N'%" + title + "%' ";

            if (status > -1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Published=" + status.ToString();
            }
            if (type > -1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Type=" + type.ToString();
            }
            if (itemid > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " ItemId=" + itemid.ToString();
            }

            return GetAllCommentsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }
        public override int DeleteCommentDyn(string where)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Comment_DeleteDynamic(where);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "DeleteCommentDyn");
                return -1;
            }
        }
        public override int UpdateCommentDyn(string update, string where)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_Comment_UpdateDynamic(update, where);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "UpdateCommentDyn " + update + " | " + where);
                return -1;
            }
        }
        public override int UpdateComment(long CommentId, int status)
        {
            var update = " Set Published= " + status;
            var where = "Id =" + CommentId;
            return UpdateCommentDyn(update, where);

        }
        public override int PublishedComments(string lstCommentIds)
        {
            var update = " Set Published=1 ";
            var where = "Id IN (" + lstCommentIds + ")";
            return UpdateCommentDyn(update, where);

        }

        public override int DeleteComment(long CommentId) { var where = "Id =" + CommentId; return DeleteCommentDyn(where); }
        public override int DeleteComments(string lstCommentIds) { var where = "Id IN (" + lstCommentIds + ")"; return DeleteCommentDyn(where); }


    }
}
