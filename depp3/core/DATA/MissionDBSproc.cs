using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class MissionDBSproc: MissionDBBase
    {
        public override int CreateUpdateMission ( Mission Mission )
        {
            try
            {
                int? _id = Mission.Id;
                
                int? _categoryid = Mission.CategoryId;

                string _createdBy = Mission.CreatedBy;
                string _name = Mission.Name;
                string _code = Mission.Code;
                string _description = Mission.Description;
                int? _result = Mission.Result;

                string _organ = Mission.Organ;
               
                int? _status = Mission.Status;
                int? _Accept = Mission.Accept;
                DateTime? _publishDate = Mission.PublishDate;
                DateTime? _createdDate = Mission.CreatedDate;

                int? _FromDate = Mission.FromDate;

                int? _ToDate = Mission.ToDate;

                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Mission_InsertUpdate(_id, _categoryid, _name, _description,_createdBy, _publishDate, _status,  _result, _Accept,_code, _organ, _FromDate,_ToDate);

            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }
        public override IEnumerable<Mission> GetTopLastestMissions(int top, int categoryId)
        {
            var select = " *";
            if (top > 1)
                select = "TOP(" + top + ") *";
            var where = "Status = 1";
            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "CategoryId = " + categoryId;
            }
            var orderBy = "CategoryId ASC";

            return GetMissionsDyn(select, where, orderBy);
        }
        public override Mission GetMission ( int MissionId )
        {
            var select = "*";
            var where = "Id = " + MissionId;
            var orderBy = string.Empty;

            var results = GetMissionsDyn ( select, where, orderBy );

            if ( results == null )
                return null;

            return results.FirstOrDefault ();
        }

        public override IEnumerable<Mission> GetMissionsDyn ( string select, string where, string orderBy )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Mission_SelectDynamic ( select, where, orderBy ).ToArray ();

            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }

      

        public override IEnumerable<Mission> GetAllMissionsPagedDyn ( string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords )
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using ( ShopOnlineDataContext datacontext = DataContext )
                {
                    var results = datacontext.sp_Mission_SelectPagedDynamic ( select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord ).ToArray ();
                    totalRecords = Convert.ToInt32 ( _totalRecord );

                    return results;
                }

            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }


        public override IEnumerable<Mission> GetMissionsByFilter(string keyword, int categoryId, int status, int pageIndex, int pageSize, int year,int createdBy, ref int totalRecords)
        {
            var select = "*";



            var where = string.Empty;
            var orderBy = "CategoryId ASC";

            if (!string.IsNullOrEmpty(keyword))
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where +=
                    " Code =" + "'" + keyword + "'";

            }

            if (year>0)
            {
                

                if (!string.IsNullOrEmpty(where))
                    where += " AND ";
                where +=
                    " FromDate =" + year;
                
            }

            if (categoryId > 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += "CategoryId = " + categoryId;
            }
            if (status >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " Status =" + status;
            }
           
            if (createdBy >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " CreatedBy =" + createdBy;
            }
            return GetAllMissionsPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

     
        public override int DeleteMissionDyn ( string where )
        {
            try
            {
                using ( ShopOnlineDataContext datacontext = DataContext )
                    return datacontext.sp_Mission_DeleteDynamic ( where );

            }
            catch ( Exception exp )
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }
        public override int ViewAdd(long Id)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    //return datacontext.sp_Mission_ViewAdd(Id);
                    return 1;

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }

        public override int DeleteMission ( int MissionId ) { var where = "Id =" + MissionId; return DeleteMissionDyn ( where ); }
        public override int DeleteMissions ( string lstMissionIds ) { var where = "Id IN (" + lstMissionIds + ")"; return DeleteMissionDyn ( where ); }


    }
}
