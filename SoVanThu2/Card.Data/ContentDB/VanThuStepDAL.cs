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
    // Thêm phương thức này vào VanThuDAL hoặc dùng class riêng như dưới.
    public static class VanThuStepDAL
    {
        private static string[] FieldsForStep(int step)
        {
            switch (step)
            {
                case 1: return new string[] { "NgayBaoCao", "NgayCvd", "SoCvDen", "NguonTiepNhan", "SoKyHieuVb", "CbDuocGiao", "TenNguoiGui", "Mst", "AffectedUnits", "NoiDungDon", "DonDaCoBanAn", "RelatedField1", "RelatedField2", "RelatedField3", "RelatedField4", "RelatedField5", "RelatedField6", "RelatedField7", "RelatedField8", "RelatedField9", "RelatedField10", "RelatedField11", "RelatedField12", "RelatedField13", "RelatedField14", "RelatedField15", "RelatedField16", "RelatedField17", "RelatedField18", "RelatedField19", "RelatedFieldOtherText", "CccdNguoiGuiDon", "NgayDonThu", "TransferSource", "TransferSourceNo", "TransferSourceDate" };
                case 2: return new string[] { "DieuKienXuLy", "Kdk01", "Kdk02", "Kdk03", "Kdk04", "Kdk05", "GhiChuDieuKien" };
                case 3: return new string[] { "ThamQuyenDon", "GhiChuThamQuyen" };
                case 4: return new string[] { "LoaiDon", "PhanLoaiTc", "LanTc", "UyQuyenKn", "LanKn", "LinhVuc02Xld", "KNPADetail" };
                case 5: return new string[] { "KetQuaXuLyChinh", "SoPhieuChuyen", "NgayPhieuChuyen", "NoiNhanPhieuChuyen", "SoPhieuHuongDan", "NgayPhieuHuongDan", "SoQdTbThuLy", "NgayQdTbThuLy", "NonAcceptanceNo", "NonAcceptanceDate", "SuspensionNo", "SuspensionDate", "TemporarySuspensionNo", "TemporarySuspensionDate" };
                case 6: return new string[] { "ResolutionNo", "ResolutionDate", "CaseStatus", "DeadlineStatus", "PendingReason", "RecommendedRecoveryMoney", "RecommendedRecoveryLand", "RecommendedReturnMoney", "RecommendedReturnLand", "RecommendedAdministrativeAction", "ActualRecoveryMoney", "ActualRecoveryLand", "InvestigationReferral", "ResolutionResult", "ActualReturnMoney", "ActualReturnLand", "ActualAdministrativeAction", "ProsecutionStatus", "ResponseLetterNo", "ResponseLetterDate", "RecommendedAdministrativeOrganizations", "RecommendedAdministrativePersons", "RecommendedAdministrativeOfficials", "ActualAdministrativeOrganizations", "ActualAdministrativePersons", "ActualAdministrativeOfficials", "InvestigationCases", "InvestigationPersons", "InvestigationOfficials", "ProsecutionCases", "ProsecutionPersons", "ProsecutionOfficials" };
                case 7: return new string[] { "PhieuChuyenBiTraLai", "LyDoTraLaiPhieuChuyen", "FollowupReportNo", "FollowupReportDate" };
                default: throw new ArgumentOutOfRangeException("step");
            }
        }


        private static void ValidateStep(int step, System.Collections.Specialized.NameValueCollection f)
        {
            string[][] required = {
                new string[0],
                new[]{"NgayBaoCao","NgayCvd","SoCvDen","NguonTiepNhan","NoiDungDon"},
                new[]{"DieuKienXuLy"},new[]{"ThamQuyenDon"},new[]{"LoaiDon"},
                new[]{"KetQuaXuLyChinh"},new[]{"CaseStatus","DeadlineStatus"},new string[0]
            };
            foreach (string n in required[step]) Require(f, n);
            if (step == 1)
            {
                bool any = false; for (int i = 1; i <= 19; i++) if (IsChecked(f["RelatedField" + i])) any = true;
                if (!any) throw new FormatException("Phải chọn ít nhất một lĩnh vực");
                if (IsChecked(f["RelatedField19"])) Require(f, "RelatedFieldOtherText");
                if (f["NguonTiepNhan"] == "2") foreach (string n in new[] { "TransferSource", "TransferSourceNo", "TransferSourceDate" }) Require(f, n);
            }
            if (step == 2 && f["DieuKienXuLy"] == "2")
            {
                bool any = false; for (int i = 1; i <= 5; i++) if (IsChecked(f["Kdk0" + i])) any = true;
                if (!any) throw new FormatException("Không đủ điều kiện: chọn ít nhất một lý do");
            }
            if (step == 4)
            {
                if (f["LoaiDon"] == "1") foreach (string n in new[] { "PhanLoaiTc", "LanTc", "LinhVuc02Xld" }) Require(f, n);
                if (f["LoaiDon"] == "2") foreach (string n in new[] { "LanKn", "LinhVuc02Xld" }) Require(f, n);
                if (f["LoaiDon"] == "3") foreach (string n in new[] { "KNPADetail", "LinhVuc02Xld" }) Require(f, n);
            }
            if (step == 5)
            {
                string[] extra = f["KetQuaXuLyChinh"] == "2" ? new[] { "SoPhieuChuyen", "NgayPhieuChuyen", "NoiNhanPhieuChuyen" } :
                    f["KetQuaXuLyChinh"] == "3" ? new[] { "SoPhieuHuongDan", "NgayPhieuHuongDan" } :
                    f["KetQuaXuLyChinh"] == "4" ? new[] { "SoQdTbThuLy", "NgayQdTbThuLy" } :
                    f["KetQuaXuLyChinh"] == "5" ? new[] { "NonAcceptanceNo", "NonAcceptanceDate" } : new string[0];
                foreach (string n in extra) Require(f, n);
            }
            if (step == 6 && f["CaseStatus"] == "2") Require(f, "PendingReason");
            if (step == 7 && IsChecked(f["PhieuChuyenBiTraLai"])) Require(f, "LyDoTraLaiPhieuChuyen");
        }
        private static void Require(System.Collections.Specialized.NameValueCollection f, string n)
        { if (String.IsNullOrWhiteSpace(f[n])) throw new FormatException("Thiếu trường bắt buộc: " + n); }
        private static bool IsChecked(string v)
        { return v != null && v.Split(',').Any(x => x.Equals("true", StringComparison.OrdinalIgnoreCase) || x == "on"); }

        private static readonly string[] SharedFields = new string[0];

        public static int SaveStep(VanThu posted, int currentStep, string direction, System.Collections.Specialized.NameValueCollection form, int? trustedGroupId, string username)
        {
            if (currentStep < 1 || currentStep > 7) throw new InvalidOperationException("Step không hợp lệ");
            if (direction != "next" && direction != "back" && direction != "draft" && direction != "finish")
                throw new InvalidOperationException("Hướng chuyển không hợp lệ");
            if (direction == "finish" && currentStep != 7) throw new InvalidOperationException("Chỉ kết thúc tại bước 7");
            int nextStep = direction == "next" ? Math.Min(7, currentStep + 1) :
                           direction == "back" ? Math.Max(1, currentStep - 1) : currentStep;
            if (posted.Id == 0 && (!trustedGroupId.HasValue || String.IsNullOrWhiteSpace(username)))
                throw new InvalidOperationException("Thiếu đơn vị hoặc tài khoản đăng nhập");
            // Kiểm tra bắt buộc phía server, không phụ thuộc JavaScript.
            if (direction == "next" || direction == "finish") ValidateStep(currentStep, form);
            var names = FieldsForStep(currentStep).ToArray();
            // Chỉ nhận các cột trong allowlist. Không cho client truyền tên cột SQL tùy ý.
            var values = new Dictionary<string, object>();
            foreach (var name in names)
            {
                var property = typeof(VanThu).GetProperty(name);
                if (property == null) continue;
                var raw = form[name];
                var underlying = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                object value = DBNull.Value;
                if (underlying == typeof(bool))
                {
                    // Checkbox không gửi key khi bỏ chọn => false, kể cả sửa từ true sang false.
                    value = raw != null && (raw.Split(',').Any(v => v.Equals("true", StringComparison.OrdinalIgnoreCase) || v == "on"));
                }
                else if (!String.IsNullOrWhiteSpace(raw))
                {
                    if (underlying == typeof(DateTime)) value = DateTime.ParseExact(raw.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    else if (underlying == typeof(int)) value = int.Parse(raw, CultureInfo.InvariantCulture);
                    else if (underlying == typeof(decimal)) value = decimal.Parse(raw, CultureInfo.InvariantCulture);
                    else value = raw;
                }
                values.Add(name, value);
            }
            // Các nhánh ẩn không được gửi => SQL NULL/false.
            // Nguồn tiếp nhận trực tiếp: không giữ dữ liệu phiếu chuyển nguồn cũ.
            if (currentStep == 1 && form["NguonTiepNhan"] != "2")
                foreach (var col in new[] { "TransferSource", "TransferSourceNo", "TransferSourceDate" }) values[col] = DBNull.Value;
            // Thay bằng Configuration.HomeConnectionString nếu đó là connection string thực tế trong dự án.
            using (var conn = new SqlConnection(Config.MainConnectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        int id = posted.Id;
                        if (id == 0)
                        {
                            if (currentStep != 1 || direction == "back") throw new InvalidOperationException("Hồ sơ mới phải bắt đầu ở bước 1");
                            DateTime reportDate = DateTime.ParseExact(form["NgayBaoCao"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            // Mã báo cáo theo quy tắc đã thống nhất. Nếu cần duy nhất từng đơn, phải bổ sung hậu tố.
                            string reportCode = username; ;
                            values["GroupId"] = trustedGroupId.Value;
                            values["ReportCode"] = reportCode;
                            var columns = values.Keys.ToArray();
                            string sql = "INSERT INTO dbo.VanThu_v2 ([Step],[IsCompleted]," +
                                String.Join(",", columns.Select(c => "[" + c + "]")) + ") VALUES (@Step,0," +
                                String.Join(",", columns.Select(c => "@" + c)) + "); SELECT CONVERT(INT,SCOPE_IDENTITY());";
                            using (var cmd = new SqlCommand(sql, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@Step", nextStep);
                                foreach (var item in values) cmd.Parameters.AddWithValue("@" + item.Key, item.Value);
                                id = Convert.ToInt32(cmd.ExecuteScalar());
                            }
                        }
                        else
                        {
                            int dbStep; bool completed;
                            using (var cmd = new SqlCommand("SELECT [Step],ISNULL(IsCompleted,0),[GroupId] FROM dbo.VanThu_v2 WITH (UPDLOCK,HOLDLOCK) WHERE Id=@Id", conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@Id", id);
                                using (var reader = cmd.ExecuteReader())
                                {
                                    if (!reader.Read()) throw new InvalidOperationException("Không tìm thấy hồ sơ");
                                    dbStep = reader.IsDBNull(0) ? 1 : reader.GetInt32(0);
                                    completed = reader.GetBoolean(1);
                                    if (!trustedGroupId.HasValue || reader.IsDBNull(2) || reader.GetInt32(2) != trustedGroupId.Value)
                                        throw new UnauthorizedAccessException("Không có quyền sửa hồ sơ của đơn vị khác");
                                }
                            }
                            if (completed) throw new InvalidOperationException("Hồ sơ đã kết thúc, không được chỉnh sửa");
                            // PHẢI thay điều kiện này bằng phân quyền từ server theo GroupId/user.
                            // Không nhận GroupId từ request. Xem Controller_Actions.cs.txt.
                            if (dbStep != currentStep) throw new InvalidOperationException("Bước hồ sơ đã thay đổi, vui lòng tải lại trang");
                            if (currentStep == 4 && form["LoaiDon"] == "4")
                            {
                                using (var check = new SqlCommand("SELECT ThamQuyenDon FROM dbo.VanThu_v2 WHERE Id=@Id", conn, tran))
                                {
                                    check.Parameters.AddWithValue("@Id", id);
                                    if (Convert.ToString(check.ExecuteScalar()) != "2")
                                        throw new InvalidOperationException("Đơn nhiều nội dung chỉ áp dụng khi không thuộc thẩm quyền");
                                }
                            }
                            if (currentStep == 5)
                            {
                                using (var check = new SqlCommand("SELECT DieuKienXuLy,ThamQuyenDon,LoaiDon FROM dbo.VanThu_v2 WHERE Id=@Id", conn, tran))
                                {
                                    check.Parameters.AddWithValue("@Id", id);
                                    using (var r = check.ExecuteReader()) if (r.Read())
                                    {
                                        int dk = r.IsDBNull(0) ? 0 : r.GetInt32(0), tq = r.IsDBNull(1) ? 0 : r.GetInt32(1), loai = r.IsDBNull(2) ? 0 : r.GetInt32(2);
                                        string forced = (dk == 2 || dk == 3) ? "1" : (loai == 4 || (loai == 2 && tq == 2)) ? "3" : null;
                                        if (forced != null && form["KetQuaXuLyChinh"] != forced)
                                            throw new InvalidOperationException("Kết quả xử lý không phù hợp với các bước trước");
                                    }
                                }
                            }
                            var sets = values.Keys.Select(c => "[" + c + "]= @" + c).ToList();
                            sets.Add("[Step]=@Step");
                            if (direction == "finish") { sets.Add("[IsCompleted]=1"); sets.Add("[CompletedAt]=GETDATE()"); }
                            using (var cmd = new SqlCommand("UPDATE dbo.VanThu_v2 SET " + String.Join(",", sets) + " WHERE Id=@Id", conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@Id", id);
                                cmd.Parameters.AddWithValue("@Step", nextStep);
                                foreach (var item in values) cmd.Parameters.AddWithValue("@" + item.Key, item.Value);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        tran.Commit(); return id;
                    }
                    catch { tran.Rollback(); throw; }
                }
            }
        }
    }
}
