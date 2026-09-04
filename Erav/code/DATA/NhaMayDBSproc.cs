using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UTILS;

namespace DATA
{
    public class NhaMayDBSproc : NhaMayDBBase
    {
        public override int CreateUpdateNhaMay(NhaMay NhaMay)
        {
            try
            {
                int? _id = NhaMay.Id;

                string TenNhaMay = NhaMay.TenNhaMay;

                string GiamDoc = NhaMay.GiamDoc;
                string GhiChu = NhaMay.GhiChu;
                string DienThoai = NhaMay.DienThoai;
                decimal? CongSuat = NhaMay.CongSuat;
                bool? DangThamGia = NhaMay.DangThamGia;

                bool? DaXoa = NhaMay.DaXoa;

                string DiaChi = NhaMay.DiaChi;
                int? LoaiNhaMay = NhaMay.LoaiNhaMay;


                int? LoaiThamGia = NhaMay.LoaiThamGia;

                DateTime? NgayThamGia = NhaMay.NgayThamGia;

                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_NhaMay_InsertUpdate(_id, TenNhaMay, LoaiThamGia, NgayThamGia,LoaiNhaMay,DienThoai,DiaChi,GhiChu,DangThamGia,1,CongSuat,GiamDoc );

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }
        public override IEnumerable<NhaMay> GetTopLastestNhaMays(int top)
        {
            var select = " *";
            if (top > 1)
                select = "TOP(" + top + ") *";
            var where = "";

            var orderBy = "[ThuTuHienThi] ASC, Id DESC";

            return GetNhaMaysDyn(select, where, orderBy);
        }
        public override NhaMay GetNhaMay(int NhaMayId)
        {
            var select = "*";
            var where = "Id = " + NhaMayId;
            var orderBy = string.Empty;

            var results = GetNhaMaysDyn(select, where, orderBy);

            if (results == null)
                return null;

            return results.FirstOrDefault();
        }

        public override IEnumerable<NhaMay> GetNhaMaysDyn(string select, string where, string orderBy)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    return datacontext.sp_NhaMay_SelectDynamic(select, where, orderBy).ToArray();

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }



        public override IEnumerable<NhaMay> GetAllNhaMaysPagedDyn(string select, string where, string orderBy, int pageIndex, int pageSize, ref int totalRecords)
        {
            try
            {
                int? _pageIndex = pageIndex;
                int? _pageSize = pageSize;
                int? _totalRecord = totalRecords;
                using (ShopOnlineDataContext datacontext = DataContext)
                {
                    var results = datacontext.sp_NhaMay_SelectPagedDynamic(select, where, orderBy, _pageIndex, _pageSize, ref _totalRecord).ToArray();
                    totalRecords = Convert.ToInt32(_totalRecord);

                    return results;
                }

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return null;
            }
        }
        
        public override IEnumerable<NhaMay> GetNhaMaysByFilter( string keyword, int pageIndex, int pageSize, ref int totalRecords, int loai = -1, int hinhthuc = -1, int status = -1, string fromdate = "", string todate = "")
        {
            var select = "*";



            var where = string.Empty;
            var orderBy = "ThuTuHienThi ASC, [TenNhaMay] ASC, Id DESC";


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
                    " (convert(nvarchar(23),NgayThamGia,121) between '" + _FormDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and '" + _ToDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "')";



            }
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = Utils.FormatKeywordSearch(keyword);
                where += "( TenNhaMay LIKE N'%" + keyword + "%' ";
                where += "OR GiamDoc LIKE N'%" + keyword + "%' )";

            }
            if (loai > -1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " LoaiNhaMay =" + loai;
            }
            if (hinhthuc > -1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " LoaiThamGia =" + hinhthuc;
            }
            if (status > -1)
            {
                if (!string.IsNullOrEmpty(where))
                    where += " AND ";

                where += " DangThamGia =" + status;
            }
            return GetAllNhaMaysPagedDyn(select, where, orderBy, pageIndex, pageSize, ref totalRecords);
        }


        public override int DeleteNhaMayDyn(string where)
        {
            try
            {
                using (ShopOnlineDataContext datacontext = DataContext)
                    //return datacontext.sp_NhaMay_DeleteDynamic(where);
                    return 0;

            }
            catch (Exception exp)
            {
                NLogLogger.PublishException(exp);
                return -1;
            }
        }


        public override int DeleteNhaMay(int NhaMayId) { var where = "Id =" + NhaMayId; return DeleteNhaMayDyn(where); }
        public override int DeleteNhaMays(string lstNhaMayIds) { var where = "Id IN (" + lstNhaMayIds + ")"; return DeleteNhaMayDyn(where); }


    }
}
