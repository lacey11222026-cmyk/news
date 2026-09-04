using System;
using System.Collections.Generic;
using System.Linq;
using UTILS;

namespace DATA
{
    public class BusInfoDBSproc : BusInfoDBBase
    {
        public override BusInfo GetBusInfo(int BusInfoId)
        {
            var select = "*";
            var where = "Id = " + BusInfoId;
            var orderBy = string.Empty;

            var results = GetBusInfosDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }

        public override IEnumerable<BusInfo> GetBusInfosDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext dc = DataContext)
                    return dc.sp_BusInfo_SelectDynamic(select, where, orderBy).ToArray();

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "GetBusInfosDyn select=" + select + "| where" + where);
                return null;
            }
        }

        public override IEnumerable<BusInfo> GetAllBusInfosPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            string select = "*";
            var where = "";
            string orderBy = "Name";

            return GetAllBusInfosPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }

        public override IEnumerable<BusInfo> GetAllBusInfosPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using (ShopOnlineDataContext dc = DataContext)
                {
                    var results = dc.sp_BusInfo_SelectPagedDynamic(select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecord);

                    return results;
                }

            }
            catch (Exception exp)
            {
                ExHandler.Handle(exp, "GetAllBusInfosPagedDyn");
                return null;
            }
        }

        public override IEnumerable<BusInfo> GetAllBusInfos(int cityId, int status)
        {
            var select = "*";
            var where = " CityId = " + cityId;
            string orderBy = "Number ASC";

            if (status >= 0)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " [Status] =" + status;
            }

            return GetBusInfosDyn(select, where, orderBy);
        }
    }
}
