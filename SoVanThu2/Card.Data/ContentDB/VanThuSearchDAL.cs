using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using Card.Utility;
using DATA.ContentDB;


namespace DATA.ContentDB
{
    public static class VanThuSearchDAL
    {
        // Dùng cùng connection string với VanThuStepDAL. Đổi tên "ConnectionString" cho đúng Web.config.
        private static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString; } }
        public static List<VanThu> GetSearch(string keyword, int groupId, int pageIndex, int pageSize,
            ref int totalRecords, string fromdate = "", string todate = "")
        {
            DateTime from, to;
            DateTime? start = ParseDate(fromdate, out from) ? (DateTime?)from.Date : null;
            DateTime? endExclusive = ParseDate(todate, out to) ? (DateTime?)to.Date.AddDays(1) : null;
            if(start.HasValue && endExclusive.HasValue && start.Value >= endExclusive.Value)
                throw new ArgumentException("Từ ngày phải nhỏ hơn hoặc bằng đến ngày");
            var result = new List<VanThu>();
            using (var cn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand("dbo.sp_VanThu_v2_Search", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Keyword", SqlDbType.NVarChar, 200).Value = (object)(keyword ?? "").Trim();
                cmd.Parameters.Add("@GroupId", SqlDbType.Int).Value = groupId;
                cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = Math.Max(1, pageIndex);
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = Math.Max(1, Math.Min(200, pageSize));
                cmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = (object)start ?? DBNull.Value;
                cmd.Parameters.Add("@ToDateExclusive", SqlDbType.DateTime2).Value = (object)endExclusive ?? DBNull.Value;
                var count = cmd.Parameters.Add("@TotalRecords", SqlDbType.Int); count.Direction = ParameterDirection.Output;
                cn.Open();
                using(var rd = cmd.ExecuteReader())
                {
                    while(rd.Read()) result.Add(new VanThu {
                        Id = Convert.ToInt32(rd["Id"]),
                        GroupId = rd["GroupId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rd["GroupId"]),
                        ReportCode = rd["ReportCode"] == DBNull.Value ? null : Convert.ToString(rd["ReportCode"]),
                        NgayBaoCao = rd["NgayBaoCao"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rd["NgayBaoCao"]),
                        NgayCvd = rd["NgayCvd"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rd["NgayCvd"]),
                        SoCvDen = rd["SoCvDen"] == DBNull.Value ? null : Convert.ToString(rd["SoCvDen"]),
                        NguonTiepNhan = rd["NguonTiepNhan"] == DBNull.Value ? (int?)null : Convert.ToInt32(rd["NguonTiepNhan"]),
                        LoaiDon = rd["LoaiDon"] == DBNull.Value ? (int?)null : Convert.ToInt32(rd["LoaiDon"]),
                        Step = rd["Step"] == DBNull.Value ? (int?)null : Convert.ToInt32(rd["Step"]),
                        IsCompleted = rd["IsCompleted"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(rd["IsCompleted"])
                    });
                }
                totalRecords = count.Value == DBNull.Value ? 0 : Convert.ToInt32(count.Value);
            }
            return result;
        }
        private static bool ParseDate(string value, out DateTime date)
        {
            date = default(DateTime);
            if (string.IsNullOrWhiteSpace(value)) return false;
            if (!DateTime.TryParseExact(value.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out date)) throw new FormatException("Ngày không hợp lệ: " + value);
            return true;
        }
    }
}
