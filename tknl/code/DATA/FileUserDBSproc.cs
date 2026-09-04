using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class FileUserDBSproc : FileUserDBBase
    {
        public override int CreateUpdateFileUser(FileUser FileUser)
        {
            try
            {
                long? _id = FileUser.Id;


                string userName = FileUser.UserName;
                string fileName = FileUser.FileName;
                string keyword = FileUser.Keyword;



                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_FileUser_InsertUpdate(_id, userName, fileName, keyword);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "FileUserDBSproc", "CreateUpdateFileUser");
                return -1;
            }
        }
       
            
        public override FileUser GetFileUser(long FileUserId)
        {
            var select = "*";
            var where = "Id = " + FileUserId;
            var orderBy = string.Empty;

            var results = GetFileUsersDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }

        public override IEnumerable<FileUser> GetFileUsersDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_FileUser_SelectDynamic(select, where, orderBy).ToArray();

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "FileUserDBSproc", "GetFileUsersDyn: select" + select);
                return null;
            }
        }




        public override IEnumerable<FileUser> GetFileUsersByFilter(int top, string title, string filetype, string username, string fromdate = "", string todate = "")
        {
            var select = "TOP(" + top + ") [Id],[UserName],[FileName],[Keyword],[CreateTime]";



            var where = string.Empty;
            var orderBy = "CreateTime DESC";

            if (!string.IsNullOrEmpty(title))
                where += "(FileName LIKE N'%" + title + "%' OR [Keyword] LIKE N'%" + title + "%'  )";
            if (!string.IsNullOrEmpty(filetype))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where += "(FileName LIKE N'%" + filetype + "%'  )";
            }
            if (!string.IsNullOrEmpty(username) && username != "-1")
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " [UserName] =" + "'" + username + "'";
            }
            if (!string.IsNullOrEmpty(fromdate) || !string.IsNullOrEmpty(todate))
            {
                var culture = new CultureInfo("fr-FR", true);
                var _FormDate = new DateTime(1900, 1, 1);
                var _ToDate = new DateTime(9999, 1, 1);
                if (!string.IsNullOrEmpty(fromdate))
                    _FormDate = DateTime.Parse(fromdate, culture).Date;
                if (!string.IsNullOrEmpty(todate))
                    _ToDate = DateTime.Parse(todate, culture).Date.AddDays(1).AddSeconds(-1);

                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where +=
                    " (convert(nvarchar(23),[CreateTime],121) between '" + _FormDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and '" + _ToDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "')";

            }
            //ExHandler.Handle(new Exception(), where);
            return GetFileUsersDyn(select, where, orderBy);
        }
        public override int DeleteFileUserDyn(string where)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_FileUser_DeleteDynamic(where);

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "DeleteFileUserDyn");
                return -1;
            }
        }


        public override int DeleteFileUser(long FileUserId) { var where = "Id =" + FileUserId; return DeleteFileUserDyn(where); }
        public override int DeleteFileUsers(string lstFileUserIds) { var where = "Id IN (" + lstFileUserIds + ")"; return DeleteFileUserDyn(where); }


    }
}
